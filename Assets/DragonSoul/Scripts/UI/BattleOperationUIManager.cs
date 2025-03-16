using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleOperationUIManager : MonoBehaviour
{
    [SerializeField] PlayerEventManager playerEventManager;
    [SerializeField] Image attackIcon;
    [SerializeField] Image dashIcon;
    [SerializeField] Image guardIcon;
    [SerializeField] Image ultimateIcon;
    [SerializeField] Image healIcon;
    [SerializeField] Image powerUpIcon;

    Color defaultColor;
    Color pushColor = Color.red;

    const float ChangeColorDuration = 0.15f;    // UIの色変更継続時間

    void Awake()
    {
        playerEventManager.OnAttack += ShowAttackUIEffect;
        playerEventManager.OnDash += ShowDashUIEffect;
        playerEventManager.OnGuard += ShowGuardUIEffect;
        playerEventManager.OnUltimate += ShowUltimateUIEffect;
        playerEventManager.OnHeal += ShowHealUIEffect;
        playerEventManager.OnPowerUp += ShowPowerUpUIEffect;
    }

    private void OnDestroy()
    {
        playerEventManager.OnAttack -= ShowAttackUIEffect;
        playerEventManager.OnDash -= ShowDashUIEffect;
        playerEventManager.OnGuard -= ShowGuardUIEffect;
        playerEventManager.OnUltimate -= ShowUltimateUIEffect;
        playerEventManager.OnHeal -= ShowHealUIEffect;
        playerEventManager.OnPowerUp -= ShowPowerUpUIEffect;
    }

    void Start()
    {
        defaultColor = attackIcon.color;
    }

    IEnumerator ChangeColor(Image icon)
    {
        icon.color = pushColor;

        yield return new WaitForSeconds(ChangeColorDuration);

        icon.color = defaultColor;
    }

    void ShowAttackUIEffect()
    {
        StartCoroutine(ChangeColor(attackIcon));
    }

    void ShowDashUIEffect()
    {
        StartCoroutine(ChangeColor(dashIcon));
    }

    void ShowGuardUIEffect()
    {
        StartCoroutine(ChangeColor(guardIcon));
    }

    void ShowUltimateUIEffect()
    {
        StartCoroutine(ChangeColor(ultimateIcon));
    }

    void ShowHealUIEffect()
    {
        StartCoroutine(ChangeColor(healIcon));
    }

    void ShowPowerUpUIEffect()
    {
        StartCoroutine(ChangeColor(powerUpIcon));
    }

}
