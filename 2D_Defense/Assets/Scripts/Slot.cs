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
        if (cur_dice != null) // 아이템이 해당 슬롯에 있다면 
        {
            DragSlot.instance.dragSlot = this;  // DragSlot 스크립트의 슬롯에 해당 슬롯을 넣어줌
            DragSlot.instance.DragSetImage(dice_Image,dice_LevelImage); // 이미지를 해당 이미지로 변경 후 
            DragSlot.instance.transform.position = eventData.position; // 좌표를 변경
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
            if (DragSlot.instance.dragSlot == this) // 원래 슬롯에 놓았을 경우.
            {
                DragSlot.instance.SetColor(0); // 슬롯의 투명도를 0으로 해 화면에 보이지 않게 한 후 
                DragSlot.instance.dragSlot = null; // dragSlot 을 null 로 비워준 후
                dice_Image.color = new Color(255, 255, 255, 1);  // 아이템 이미지의 투명도와 색을 원래대로 해준 후 
                dice_LevelImage.color = new Color(255, 255, 255, 1);
                return; // return 
            }

            if (DragSlot.instance.dragSlot != null) // 만약 드래그가 끝났는데 dragSlot 이 null 이 아니라면 - 정상적으로 드래그가 종료되지 않았다면 
            {
                DragSlot.instance.SetColor(0); // 투명도를 0으로해 화면에 보이지 않게 해준 후 
                DragSlot.instance.dragSlot = null; // dragSlot 을 null 로 비워줌.
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) // 내려놓을 인벤토리 슬롯이 있다면 
        {
            Slot tempSlot = DragSlot.instance.dragSlot; // tempSlot 이라는 Slot 변수를 만들어서 현재 드래그 중인 슬롯을 넣어줌. 

            if (tempSlot == this) // 원래 슬롯에 놓았을 경우.
            {
                Debug.Log("원래 슬롯");
                DragSlot.instance.SetColor(0); // 드래그 중인 슬롯의 투명도를 0으로 만들어 줌 .
                DragSlot.instance.dragSlot = null; // dragSlot 을 null로 해  비워줌.
                dice_Image.color = new Color(255, 255, 255, 1); // 아이템 이미지의 투명도를 원래대로 해준 후 
                return; // return 
            }
            else if(tempSlot.cur_dice.dice_Name != this.cur_dice.dice_Name 
                || tempSlot.cur_dice.level != this.cur_dice.level)
            {
                Debug.Log("이름 다름"); 
                DragSlot.instance.SetOrigin(1); // 드래그 중인 슬롯의 투명도를 0으로 만들어 줌 .
                DragSlot.instance.dragSlot = null; // dragSlot 을 null로 해  비워줌.
                dice_Image.color = new Color(255, 255, 255, 1); // 아이템 이미지의 투명도를 원래대로 해준 후 
                return; // return
            }
            else // 원래 슬롯이 아니라면 
            {
                Upgrade(this.cur_dice.level); // 슬롯을 바꿔준 후 
                DragSlot.instance.SetColor(0); // 드래그 슬롯을 투명도로 0으로 해줌.
                Debug.Log("업그레이드");
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

    // 아이템 이미지의 투명도 조절
    public void SetColor(float _alpha)
    {
        Color color1 = dice_Image.color;
        Color color2 = dice_LevelImage.color;
        color1.a = _alpha;
        color2.a = _alpha;
        dice_Image.color = color1;
        dice_LevelImage.color = color2;
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        cur_dice = null;
        dice_Image.sprite = null;
        Destroy(dice);
        //SetColor(0);
    }
}
