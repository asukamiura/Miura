using UnityEngine;

public class HPGaugePresenter : MonoBehaviour
{
    [SerializeField] HealthManager healthManager;
    [SerializeField] HPGaugeView view;

    void Start()
    {
        // 初期設定
        view.SetHP(healthManager.CurrentHP, healthManager.MaxHP);
    }

    void OnEnable()
    {
        healthManager.OnHPChanged += view.SetHP;        
    }

    void OnDisable()
    {
        healthManager.OnHPChanged -= view.SetHP;
    }
}
