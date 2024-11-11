using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float hp;  // HP
    private float maxHP;  // HPè„å¿
    public bool isDead { get { return hp <= 0; } }  // éÄñSÉtÉâÉO

    private void Start()
    {
        maxHP = hp;
    }

    public float HP
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(hp, 0, maxHP);
        }
    }

    public void Heal(int healVal)
    {
        if (hp < maxHP )
        {
            hp += healVal;
        }
    }

    public void Damage(int damageVal)
    {
        if (hp > 0)
        {
            hp -= damageVal;
        }
    }

}
