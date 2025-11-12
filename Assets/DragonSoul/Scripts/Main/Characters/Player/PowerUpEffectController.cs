using UnityEngine;

public class PowerUpEffectController : MonoBehaviour
{
    [SerializeField] PowerManager powerManager;
    [SerializeField] GameObject lightningAura;

    void Awake()
    {
        powerManager.OnPowerUpEnd += HidePowerUpEffect;
    }

    void OnDisable()
    {
        powerManager.OnPowerUpEnd -= HidePowerUpEffect;        
    }

    public void ShowPowerUpEffect()
    {
        EffectManager.Instance.PlayEffect("VFX_Zap_02_Blue", transform.position);
        EffectManager.Instance.PlayEffect("NovaLightningBlue", transform.position);
        lightningAura.SetActive(true);
        MaterialManager.Instance.IsActiveForceField = true;
    }

    void HidePowerUpEffect()
    {
        lightningAura.SetActive(false);
        MaterialManager.Instance.IsDisabledForceField = true;
    }
}
