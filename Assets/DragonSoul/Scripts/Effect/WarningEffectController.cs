using System.Collections;
using UnityEngine;

public class WarningEffectController : MonoBehaviour
{
    [SerializeField] GameObject dodgeableEffect;
    [SerializeField] GameObject guardableEffect;
    const float Duration = 1f;

    public void ShowWarningEffect(EnemyAttackType attackType)
    {
        switch (attackType)
        {
            case EnemyAttackType.Dodgeable:
                dodgeableEffect.SetActive(true);
                StartCoroutine(WaitHide(dodgeableEffect));
                break;
            case EnemyAttackType.Guardable:
                guardableEffect.SetActive(true);
                StartCoroutine(WaitHide(guardableEffect));
                break;
        }
    }

    IEnumerator WaitHide(GameObject effect)
    {
        yield return new WaitForSeconds(Duration);

        effect.SetActive(false);
    }
}
