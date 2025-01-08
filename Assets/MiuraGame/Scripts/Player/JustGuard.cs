using Cinemachine;
using System.Collections;
using UnityEngine;

public class JustGuard : MonoBehaviour
{
    [SerializeField] private PlayerCameraController playerCameraController;
    private Animator enemyAnimator;
    private Animator playerAnimator;

    private const float DefaultFOV = 70;
    private const float TargetFOV = 50;
    private const float SpreadSpeed = 2;
    private const float NarrowSpeed = 20;
    private const float TargetDutch = 5;
    private const float DefaultDutch = 0;
    private const float ChangeDutchSpeed1 = 20;
    private const float ChangeDutchSpeed2 = 5;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }

    public void ActionJustGuard()
    {
        StartCoroutine(ApplyJustGuard(1f));
    }

    private IEnumerator ApplyJustGuard(float delay)
    {
        playerCameraController.RecenteringEnabled();
        playerCameraController.StartChangeDutch(TargetDutch, ChangeDutchSpeed1);
        playerCameraController.StartChangeFOV(TargetFOV, NarrowSpeed);
        playerCameraController.ApplyImpulse();

        enemyAnimator.speed = 0;
        playerAnimator.speed = 0f;
        Debug.Log("Start");

        yield return new WaitForSeconds(delay);

        //playerCameraController.ChangeDutch(DefaultDutchAngle);
        playerCameraController.StartChangeFOV(DefaultFOV, SpreadSpeed);
        playerCameraController.StartChangeDutch(DefaultDutch, ChangeDutchSpeed2);
        playerCameraController.RecenteringDisabled();
        enemyAnimator.speed = 1;
        playerAnimator.speed = 1;
        Debug.Log("End");
    }
}
