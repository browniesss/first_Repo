using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dice : MonoBehaviour
{
    public string dice_Name;
    public int level;
    public float attack_Delay;
    public float damage;

    public GameObject target;

    public Slot parent_Slot;

    private void Start()
    {
        StartCoroutine(attack_Coroutine());

        parent_Slot = transform.parent.GetComponent<Slot>();
    }

    void Set_Target()
    {
        if (GameManager.Instance.enemy_List.Count > 0) // 적이 1마리 이상일때
        {
            GameObject[] enemy_Arr = GameManager.Instance.enemy_List.ToArray();

            target = enemy_Arr[0]; // 첫번째로 생성된 적을 항상 타겟으로 지정.
        }
    }

    IEnumerator attack_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attack_Delay);

            if (target != null)
            {
                for (int i = 0; i < parent_Slot.Cur_Dice.level; i++)
                {
                    Attack();

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    void Attack()
    {
        GameObject bullet = GameManager.Resource.Instantiate("Bullet");

        bullet.GetComponent<Bullet>().parent_Dice = this;

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        bullet.transform.position = Camera.main.ScreenToWorldPoint(transform.position);

        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        bullet.transform.DOMove(target.transform.position, 0.05f).SetEase(Ease.Linear);

        bullet_Destroy_Coroutine(bullet);
    }

    IEnumerator bullet_Destroy_Coroutine(GameObject bullet)
    {
        yield return new WaitForSeconds(1f);

        GameManager.Resource.Destroy(bullet);
    }

    private void Update()
    {
        Set_Target();
    }
}
