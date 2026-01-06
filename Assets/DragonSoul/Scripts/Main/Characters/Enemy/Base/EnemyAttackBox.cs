using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    public EnemyAttackType attackType;  // ジャストガード可能攻撃か、ジャスト回避可能攻撃か
    public int damageVal = 0;   // 与えるダメージ量
    public Action OnJustGuarded;

    HashSet<GameObject> hitObjs = new HashSet<GameObject>();    // 攻撃が当たったオブジェクトを一時的に保持(多段ヒットを防ぐため)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hitObjs.Contains(other.gameObject))
            {
                // ダメージを与える
                other.GetComponentInParent<IPlayerDamageable>().TakeDamage(damageVal, attackType, OnJustGuarded);
                hitObjs.Add(other.gameObject);
            }          
        }        
    }

    void OnDisable()
    {
        hitObjs.Clear();
    }

    public void ShowAttackBox()
    {
        gameObject.SetActive(true);
    }

    public void HideAttackBox()
    {
        gameObject.SetActive(false);
    }
}
