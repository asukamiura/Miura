// ダメージを与えることができることを示す
public interface IPlayerDamageable
{
    public void TakeDamage(float damage, EnemyAttackType attackType, EnemyAttack enemyAttack);
}
