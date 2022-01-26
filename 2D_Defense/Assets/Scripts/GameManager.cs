using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Player
{
    public int have_Money;
    public int cur_KillCount;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    ObjectManager _objManager = new ObjectManager();
    ResourceManager _resourceManager = new ResourceManager();

    public static ObjectManager ObjManager { get { return Instance._objManager; } }
    public static ResourceManager Resource { get { return Instance._resourceManager; } }

    [SerializeField]
    private Transform[] destination;
    [SerializeField]
    private Slot[] slots;

    public Dice[] dices;

    public Transform[] Destination { get { return destination; } }

    public Player Cur_Player { get => player; set => player = value; }

    private int cur_Level = 0;

    private Player player = new Player();

    [SerializeField]
    private Text price_Text;
    [SerializeField]
    private Text stage_Text;

    private bool isStage_Play = false; // 현재 스테이지 진행중인지. 

    static void Init()
    {
        Instance._objManager.Init();
    }

    public static void Clear()
    {
        ObjManager.Clear();
    }

    private void Awake()
    {
        Instance = this;
    }

    List<StageInfo> stageInfo = new List<StageInfo>();
    void Read_Stage()
    {
        TextAsset textFile = Resources.Load("StageInfo") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
                break;

            // 데이터 저장
            StageInfo newInfo = new StageInfo();

            newInfo.stageLevel = int.Parse(line.Split(',')[0]);
            newInfo.enemyCount = int.Parse(line.Split(',')[1]);
            newInfo.enemyHP = float.Parse(line.Split(',')[2]);

            stageInfo.Add(newInfo);
        }

        stringReader.Close(); // 텍스트 파일 닫기
    }

    public Queue<GameObject> enemy_List = new Queue<GameObject>();
    IEnumerator enemyCreate_Coroutine(int level)
    {
        enemy_List.Clear();

        for (int i = 0; i < stageInfo[level].enemyCount; i++)
        {
            GameObject obj = Resource.Instantiate("Enemy");
            enemy_List.Enqueue(obj);

            obj.transform.position = Destination[0].position;
            obj.GetComponent<Enemy>().Enemy_Move();

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Start_Stage()
    {
        if (isStage_Play)
            return;

        Debug.Log("스테이지 시작");

        player.cur_KillCount = 0;
        isStage_Play = true;

        StartCoroutine(enemyCreate_Coroutine(cur_Level));
    }

    int cur_Dice_Price = 20;
    public List<Slot> temp_List = new List<Slot>();
    public void Create_Dice()
    {
        if (player.have_Money < cur_Dice_Price)
            return;

        int randSlot = Random.Range(0, temp_List.Count);
        int randDice = Random.Range(0, 2);

        Slot[] temp_Array = temp_List.ToArray();

        Slot tempSlot = temp_Array[randSlot];
        temp_List.RemoveAt(randSlot);

        tempSlot.Cur_Dice = dices[randDice];
        tempSlot.dice = Resource.Instantiate(dices[randDice].name, tempSlot.transform);
        tempSlot.dice_Image.sprite = dices[randDice].
            GetComponent<SpriteRenderer>().sprite;

        tempSlot.dice_Image.gameObject.SetActive(true);
        tempSlot.dice_LevelImage.sprite = Resource.Load<Sprite>($"Sprites/Dice_1");
        tempSlot.dice_LevelImage.SetNativeSize();
        tempSlot.dice_LevelImage.gameObject.SetActive(true);
        tempSlot.dice.SetActive(true);
        tempSlot.SetColor(1);

        slots[randSlot] = tempSlot;

        player.have_Money -= cur_Dice_Price;

    }

    void Init_Slot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            temp_List.Add(slots[i]);
        }
    }

    void Text_Update()
    {
        price_Text.text = player.have_Money.ToString();
        stage_Text.text = "Stage : " + (cur_Level + 1);
    }

    public void Stage_Clear_Check()
    {
        Debug.Log(player.cur_KillCount + "," + stageInfo[cur_Level].enemyCount);

        if (player.cur_KillCount >= stageInfo[cur_Level].enemyCount)
        {
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            isStage_Play = false;

            cur_Level++;

            Debug.Log("스테이지 레벨 업");

            foreach (Slot slot in slots)
            {
                if (slot.dice != null)
                    slot.dice.GetComponent<Dice>().target = null;
            }

        }
        else
            Debug.Log("아직 다 못잡음");
    }

    void Start()
    {
        player.have_Money = 100;

        Read_Stage();

        Init_Slot();
    }

    void Update()
    {
        Text_Update();
    }
}
