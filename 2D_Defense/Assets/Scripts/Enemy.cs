using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float hp;

    public float HP { get => hp; set => hp = value; }

    void Start()
    {
        transform.position = GameManager.Instance.Destination[0].position;
    }

    public void Enemy_Move()
    {
        transform.DOMove(GameManager.Instance.Destination[1].position, 3f)
            .SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(GameManager.Instance.Destination[2].position, 3f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOMove(GameManager.Instance.Destination[3].position, 3f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    GameManager.Instance.Cur_Player.cur_KillCount++;
                    GameManager.Instance.Stage_Clear_Check();
                });
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Bullet bullet = collision.GetComponent<Bullet>();

            if (bullet.parent_Dice.parent_Slot.Cur_Dice != null)
                isDamaged(bullet.parent_Dice.parent_Slot.Cur_Dice.damage);

            GameManager.Resource.Destroy(collision.gameObject);

        }
    }

    public void isDamaged(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            HP = 99;

            transform.position = GameManager.Instance.Destination[0].position;

            transform.DOKill();

            GameManager.Resource.Destroy(this.gameObject);

            GameManager.Instance.enemy_List.Dequeue();

            GameManager.Instance.Cur_Player.have_Money += 20;

            GameManager.Instance.Cur_Player.cur_KillCount += 1;

            GameManager.Instance.Stage_Clear_Check();
        }
    }

    void Update()
    {
    }
}
