using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JustGuard : MonoBehaviour
{
    [SerializeField] PlayerCameraController playerCameraController;
    [SerializeField] AnimationController animationController;
    [SerializeField] NavMeshAgent enemyNavMeshAgent;

    const float DefaultFOV = 70;
    const float TargetFOV = 50;
    const float SpreadSpeed = 2;
    const float NarrowSpeed = 20;
    const float TargetDutch = 5;
    const float DefaultDutch = 0;
    const float ChangeDutchSpeed1 = 20;
    const float ChangeDutchSpeed2 = 5;

    public bool isJustGuard = false;

    void Start()
    {

    }

    public void ActionJustGuard()
    {
        StartCoroutine(ApplyJustGuard(1f));
    }

    IEnumerator ApplyJustGuard(float delay)
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
