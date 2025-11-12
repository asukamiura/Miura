using Player;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] DodgePerformance dodgePerformance;
    [SerializeField] BlockPerformance blockPerformance;
    [SerializeField] UltimatePerformance ultimatePerformance;

    void Start()
    {
        playerCore.StateMachine.OnStateChanged += StartPerformance;
    }

    void OnDisable()
    {
        playerCore.StateMachine.OnStateChanged -= StartPerformance;        
    }

    void StartPerformance(PlayerStateID stateID)
    {
        switch (stateID)
        {
            case PlayerStateID.Dodge:
                dodgePerformance.StartPerformance();
                break;
            case PlayerStateID.Block:
                blockPerformance.StartPerformance();
                break;
            case PlayerStateID.AttackUltimate:
                ultimatePerformance.StartPerformance();
                break;
        }
    }
}
