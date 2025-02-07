using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float hp;  // HP

    public float HP => hp;
    public float MaxHP { get; set; }  // HP上限
    public float MinHP { get; set; } = 0;
    public bool IsDead => 0 >= hp;  // 死亡フラグ

    void Awake()
    {
        MaxHP = hp;
    }

    public void Heal(float healVal)
    {
        hp = Mathf.Clamp(hp + healVal, MinHP, MaxHP);
    }

    public void Damage(float damageVal)
    {
        hp = Mathf.Clamp(hp - damageVal, MinHP, MaxHP);
    }

}
