// ダメージを与えることができることを示す
using System;

public interface IPlayerDamageable
{
    public void TakeDamage(float damage, EnemyAttackType attackType, Action onJustGuarded);
}
