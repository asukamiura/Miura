using Player;
using UnityEngine;

public class BattleOperationUIPresenter : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] BattleOperationUIView view;

    void Start()
    {
        playerCore.StateMachine.OnStateChanged += HandleStateChanged;
        playerCore.OnHealed += view.ReactHealIcon;
    }

    void OnDisable()
    {
        playerCore.OnHealed -= view.ReactHealIcon;     
        playerCore.StateMachine.OnStateChanged -= HandleStateChanged;
    }

    void HandleStateChanged(PlayerStateID newState)
    {
        switch (newState)
        {
            case PlayerStateID.AttackNormal1:
                break;

            case PlayerStateID.Dash:
                view.ReactDashIcon();
                break;

            case PlayerStateID.Dodge:
                view.ReactDodgeIcon();
                break;

            case PlayerStateID.Guard:
                view.ReactGuardIcon();
                break;

            case PlayerStateID.Block:
                view.ReactBlockIcon();
                break;

            case PlayerStateID.PowerUp:
                view.ReactPowerUpIcon();
                break;

            case PlayerStateID.AttackUltimate:
                view.ReactUltimateIcon();
                break;
        }
    }
}
