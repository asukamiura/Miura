using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float hp;  // HP

    public float HP => hp;
    public float maxHP { get; private set; }  // HP上限
    public float minHP { get; private set; } = 0;
    public bool isDead => 0 >= hp;  // 死亡フラグ

    private void Awake()
    {
        maxHP = hp;
    }

    public void Heal(float healVal)
    {
        hp = Mathf.Clamp(hp + healVal, minHP, maxHP);
    }

    public void Damage(float damageVal)
    {
        hp = Mathf.Clamp(hp - damageVal, minHP, maxHP);
    }

}
