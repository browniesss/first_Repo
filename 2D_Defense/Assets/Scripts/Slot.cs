using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, 
    IEndDragHandler, IDropHandler
{
    [SerializeField]
    private Dice cur_dice;
    public Dice Cur_Dice { get { return cur_dice; } set { cur_dice = value; } }

    [SerializeField]
    public Image dice_Image;
    [SerializeField]
    public Image dice_LevelImage;
    [SerializeField]
    public GameObject dice;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cur_dice != null) // �������� �ش� ���Կ� �ִٸ� 
        {
            DragSlot.instance.dragSlot = this;  // DragSlot ��ũ��Ʈ�� ���Կ� �ش� ������ �־���
            DragSlot.instance.DragSetImage(dice_Image,dice_LevelImage); // �̹����� �ش� �̹����� ���� �� 
            DragSlot.instance.transform.position = eventData.position; // ��ǥ�� ����
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cur_dice != null)
        {
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            DragSlot.instance.transform.position = eventData.position;
        }
    }
   
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (cur_dice != null)
        {
            if (DragSlot.instance.dragSlot == this) // ���� ���Կ� ������ ���.
            {
                DragSlot.instance.SetColor(0); // ������ ������ 0���� �� ȭ�鿡 ������ �ʰ� �� �� 
                DragSlot.instance.dragSlot = null; // dragSlot �� null �� ����� ��
                dice_Image.color = new Color(255, 255, 255, 1);  // ������ �̹����� ������ ���� ������� ���� �� 
                dice_LevelImage.color = new Color(255, 255, 255, 1);
                return; // return 
            }

            if (DragSlot.instance.dragSlot != null) // ���� �巡�װ� �����µ� dragSlot �� null �� �ƴ϶�� - ���������� �巡�װ� ������� �ʾҴٸ� 
            {
                DragSlot.instance.SetColor(0); // ������ 0������ ȭ�鿡 ������ �ʰ� ���� �� 
                DragSlot.instance.dragSlot = null; // dragSlot �� null �� �����.
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) // �������� �κ��丮 ������ �ִٸ� 
        {
            Slot tempSlot = DragSlot.instance.dragSlot; // tempSlot �̶�� Slot ������ ���� ���� �巡�� ���� ������ �־���. 

            if (tempSlot == this) // ���� ���Կ� ������ ���.
            {
                Debug.Log("���� ����");
                DragSlot.instance.SetColor(0); // �巡�� ���� ������ ������ 0���� ����� �� .
                DragSlot.instance.dragSlot = null; // dragSlot �� null�� ��  �����.
                dice_Image.color = new Color(255, 255, 255, 1); // ������ �̹����� ������ ������� ���� �� 
                return; // return 
            }
            else if(tempSlot.cur_dice.dice_Name != this.cur_dice.dice_Name 
                || tempSlot.cur_dice.level != this.cur_dice.level)
            {
                Debug.Log("�̸� �ٸ�"); 
                DragSlot.instance.SetOrigin(1); // �巡�� ���� ������ ������ 0���� ����� �� .
                DragSlot.instance.dragSlot = null; // dragSlot �� null�� ��  �����.
                dice_Image.color = new Color(255, 255, 255, 1); // ������ �̹����� ������ ������� ���� �� 
                return; // return
            }
            else // ���� ������ �ƴ϶�� 
            {
                Upgrade(this.cur_dice.level); // ������ �ٲ��� �� 
                DragSlot.instance.SetColor(0); // �巡�� ������ ������ 0���� ����.
                Debug.Log("���׷��̵�");
            }
        }
    }

    private void Upgrade(int cur_Level)
    {
        int rand_Dice = Random.Range(0, 2);

        Slot tempSlot = this;

        tempSlot.cur_dice = GameManager.Resource.
            Instantiate(GameManager.Instance.dices[rand_Dice].gameObject.name)
            .GetComponent<Dice>();

        tempSlot.dice_Image.sprite = GameManager.Instance.dices[rand_Dice].
            GetComponent<SpriteRenderer>().sprite;
        Destroy(tempSlot.dice);
        tempSlot.dice = GameManager.Resource.Instantiate
            (GameManager.Instance.dices[rand_Dice].name, tempSlot.transform);
        tempSlot.dice.SetActive(true);
        tempSlot.cur_dice.level = ++cur_Level;
        tempSlot.cur_dice.damage *= tempSlot.cur_dice.level;
        Debug.Log(tempSlot.cur_dice.level);

        cur_dice = tempSlot.cur_dice;
        dice_Image.sprite = tempSlot.dice_Image.sprite;
        dice_LevelImage.sprite = Resources.Load<Sprite>($"Sprites/Dice_" + tempSlot.cur_dice.level);
        dice_LevelImage.SetNativeSize();
        SetColor(1);

        DragSlot.instance.dragSlot.ClearSlot();
        GameManager.Instance.temp_List.Add(DragSlot.instance.dragSlot);
    }

    // ������ �̹����� ���� ����
    public void SetColor(float _alpha)
    {
        Color color1 = dice_Image.color;
        Color color2 = dice_LevelImage.color;
        color1.a = _alpha;
        color2.a = _alpha;
        dice_Image.color = color1;
        dice_LevelImage.color = color2;
    }

    // �ش� ���� �ϳ� ����
    private void ClearSlot()
    {
        cur_dice = null;
        dice_Image.sprite = null;
        Destroy(dice);
        //SetColor(0);
    }
}
