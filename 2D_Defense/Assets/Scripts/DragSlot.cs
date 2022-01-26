using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; // 싱글톤
    public Slot dragSlot; // 드래그 하는 슬롯을 받아옴.

    [SerializeField]
    public Image imageDice; // 드래그하는 슬롯의 아이템 이미지.
    public Image dice_LevelImage;

    Image MyImage;

    void Start()
    {
        instance = this; // 싱글톤 
        MyImage = GetComponent<Image>();
    }

    public void DragSetImage(Image _diceImage,Image _levelImage) // 드래그 하는 슬롯의 이미지를 바꿔주기 위한 함수.
    {
        imageDice = _diceImage;
        dice_LevelImage = _levelImage;
        MyImage.sprite = imageDice.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha) // 해당 슬롯의 Color 값을 바꿔주기 위한 함수.
    {
        Color color = new Color(255, 255, 255, _alpha);
        Color originColor = new Color(255, 255, 255, 0);

        imageDice.color = originColor;
        dice_LevelImage.color = originColor;

        MyImage.color = color;
    }

    public void SetOrigin(float _alpha) // 해당 슬롯의 Color 값을 바꿔주기 위한 함수.
    {
        Color color = new Color(255, 255, 255, _alpha);
        Color originColor = new Color(255, 255, 255, 0);

        imageDice.color = color;
        dice_LevelImage.color = color;

        MyImage.color = originColor;
    }
}
