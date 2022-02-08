using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Dice parent_Dice; // 발사한 주사위 정보

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(bullet_Destroy_Coroutine());
    }

    public IEnumerator bullet_Destroy_Coroutine()
    {
        yield return new WaitForSeconds(1f);

        GameManager.Resource.Destroy(this.gameObject);
    }
}
