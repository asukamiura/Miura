using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JustGuard : MonoBehaviour
{
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;

    private const float DefaultFOV = 70;
    private const float TargetFOV = 50;
    private const float SpreadSpeed = 2;
    private const float NarrowSpeed = 20;
    private const float TargetDutch = 5;
    private const float DefaultDutch = 0;
    private const float ChangeDutchSpeed1 = 20;
    private const float ChangeDutchSpeed2 = 5;

    public bool isJustGuard = false;

    private void Start()
    {
        
    }

    public void ActionJustGuard()
    {
        StartCoroutine(ApplyJustGuard(1f));
    }

    private IEnumerator ApplyJustGuard(float delay)
    {
        isJustGuard = true;

        playerCameraController.RecenteringEnabled();
        playerCameraController.StartChangeDutch(TargetDutch, ChangeDutchSpeed1);
        playerCameraController.StartChangeFOV(TargetFOV, NarrowSpeed);
        playerCameraController.ApplyImpulse();

        animationController.ChangeAllAnimationSpeed(0);
        Debug.Log("Start");

        yield return new WaitForSeconds(delay);

        //playerCameraController.ChangeDutch(DefaultDutchAngle);
        playerCameraController.StartChangeFOV(DefaultFOV, SpreadSpeed);
        playerCameraController.StartChangeDutch(DefaultDutch, ChangeDutchSpeed2);
        playerCameraController.RecenteringDisabled();        
        animationController.ChangeAllAnimationSpeed(1);

        isJustGuard = false;
        Debug.Log("End");
    }
}
