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

    LineRenderer lr;

    private void Start()
    {
        StartCoroutine(attack_Coroutine());

        parent_Slot = transform.parent.GetComponent<Slot>();

        if (GetComponent<LineRenderer>() != null)
        {
            lr = GetComponent<LineRenderer>();
            lr.startColor = new Color(255, 255, 0);
            lr.endColor = new Color(255, 255, 0);
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
        }
    }

    void Set_Target()
    {
        if (!GameManager.Instance.isStage_Play)
        {
            target = null;
            return;
        }

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
                    Attack_Type(dice_Name);

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

        if (target != null)
            bullet.transform.DOMove(target.transform.position, 0.05f).SetEase(Ease.Linear);
    }

    void Attack_Type(string name)
    {
        switch (name)
        {
            case "Fire":
                {
                    Attack();
                    GameObject[] enemy_Arr = GameManager.Instance.enemy_List.ToArray();
                    int index = 0;

                    for (int i = 0; i < enemy_Arr.Length; i++)
                    {
                        if (enemy_Arr[i] == target)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (enemy_Arr.Length >= 2 && enemy_Arr[index + 1] != null)
                        enemy_Arr[index + 1].GetComponent<Enemy>().isDamaged(parent_Slot.Cur_Dice.damage);

                    GameObject eff_Obj = GameManager.Resource.Instantiate("Fire_Effect");
                    eff_Obj.transform.position = target.transform.position;
                    eff_Obj.GetComponent<Animation>().Play();
                }
                break;
            case "Wind":
                {
                    Attack();
                }
                break;
            case "Thunder":
                {
                    Attack();

                    GameObject[] enemy_Arr = GameManager.Instance.enemy_List.ToArray();
                    int index = 0;

                    for (int i = 0; i < enemy_Arr.Length; i++)
                    {
                        if (enemy_Arr[i] == target)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (enemy_Arr.Length >= 2 && enemy_Arr[index + 1] != null)
                        enemy_Arr[index + 1].GetComponent<Enemy>().isDamaged(parent_Slot.Cur_Dice.damage);
                    if (enemy_Arr.Length >= 3 && enemy_Arr[index + 2] != null)
                        enemy_Arr[index + 2].GetComponent<Enemy>().isDamaged(parent_Slot.Cur_Dice.damage);


                    lr.enabled = true;
                    if (enemy_Arr.Length >= 2 && enemy_Arr[index + 1] != null && target != null)
                    {
                        lr.SetPosition(0, target.transform.position);
                        lr.SetPosition(1, enemy_Arr[index + 1].transform.position);
                    }
                    if (enemy_Arr.Length >= 3 && enemy_Arr[index + 2] != null && target != null)
                    {
                        if (lr.positionCount == 2)
                            lr.positionCount += 1;

                        lr.SetPosition(2, enemy_Arr[index + 2].transform.position);
                    }

                    StartCoroutine(thunder_Dice_Coroutine());
                }
                break;
        }
    }

    IEnumerator thunder_Dice_Coroutine()
    {
        yield return new WaitForSeconds(0.2f);

        lr.enabled = false;
    }

    private void Update()
    {
        Set_Target();
    }
}
