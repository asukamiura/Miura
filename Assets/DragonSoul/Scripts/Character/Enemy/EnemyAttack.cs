using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAttackType
    {
        Guardable,
        Dodgeable,
    }
    public EnemyAttackType attackType;
    public int damageVal = 0;
    public bool IsHit { get; private set; } = false;

    HashSet<GameObject> hitObjs = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hitObjs.Contains(other.gameObject))
            {
                hitObjs.Add(other.gameObject);
            }
            else
            {
                IsHit = true;
            }
        }        
    }

    void OnDisable()
    {
        hitObjs.Clear();
        IsHit = false;
    }
}
