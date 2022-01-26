using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; // �̱���
    public Slot dragSlot; // �巡�� �ϴ� ������ �޾ƿ�.

    [SerializeField]
    public Image imageDice; // �巡���ϴ� ������ ������ �̹���.
    public Image dice_LevelImage;

    Image MyImage;

    void Start()
    {
        instance = this; // �̱��� 
        MyImage = GetComponent<Image>();
    }

    public void DragSetImage(Image _diceImage,Image _levelImage) // �巡�� �ϴ� ������ �̹����� �ٲ��ֱ� ���� �Լ�.
    {
        imageDice = _diceImage;
        dice_LevelImage = _levelImage;
        MyImage.sprite = imageDice.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha) // �ش� ������ Color ���� �ٲ��ֱ� ���� �Լ�.
    {
        Color color = new Color(255, 255, 255, _alpha);
        Color originColor = new Color(255, 255, 255, 0);

        imageDice.color = originColor;
        dice_LevelImage.color = originColor;

        MyImage.color = color;
    }

    public void SetOrigin(float _alpha) // �ش� ������ Color ���� �ٲ��ֱ� ���� �Լ�.
    {
        Color color = new Color(255, 255, 255, _alpha);
        Color originColor = new Color(255, 255, 255, 0);

        imageDice.color = color;
        dice_LevelImage.color = color;

        MyImage.color = originColor;
    }
}
