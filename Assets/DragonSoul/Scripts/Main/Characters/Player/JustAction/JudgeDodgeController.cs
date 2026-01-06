using Player;
using UnityEngine;

public class JudgeDodgeController : MonoBehaviour
{
    PlayerCore playerCore;
    float elapsedTime = 0;

    void Update()
    {
        if (gameObject.activeSelf)
        {
            elapsedTime += Time.deltaTime;        
        }
    }

    void OnDisable()
    {
        elapsedTime = 0;   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyAttackBox>(out var enemyAttack))
        {
            if (enemyAttack.attackType == EnemyAttackType.Dodgeable)
            {
                playerCore.HandleDodge(elapsedTime);
            }           
        }
    }
    
    public void SetPlayerCore(PlayerCore playerCore)
    {
        this.playerCore = playerCore;
    }
}
