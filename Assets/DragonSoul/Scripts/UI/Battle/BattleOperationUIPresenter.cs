using Player;
using UnityEngine;

public class BattleOperationUIPresenter : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] BattleOperationUIView view;

    void Start()
    {
        playerCore.stateMachine.OnStateChanged = HandleStateChanged;
        playerCore.OnHeal += view.ReactHealIcon;
    }

    void OnDisable()
    {
        playerCore.OnHeal -= view.ReactHealIcon;        
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
