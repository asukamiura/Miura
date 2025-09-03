using UnityEngine;

public class UltimateGaugePresenter : MonoBehaviour
{
    [SerializeField] UltimateManager manager;
    [SerializeField] UltimateGaugeView view;

    void Start()
    {
        view.SetGaugeValue(manager.UltVal, manager.MaxUltVal);        
    }

    void OnEnable()
    {
        manager.OnGaugeValueChanged += view.SetGaugeValue;
    }

    void OnDisable()
    {
        manager.OnGaugeValueChanged -= view.SetGaugeValue;
    }
}
