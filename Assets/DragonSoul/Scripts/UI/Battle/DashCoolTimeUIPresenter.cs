using UnityEngine;

public class DashCoolTimeUIPresenter : MonoBehaviour
{
    [SerializeField] DashCooldownManager dashCooldownManager;
    [SerializeField] DashCoolTimeUIView view;

    void OnEnable()
    {
        dashCooldownManager.OnCooldown += view.SetCoolTime;
    }

    void OnDestroy()
    {
        dashCooldownManager.OnCooldown -= view.SetCoolTime;
    }
}
