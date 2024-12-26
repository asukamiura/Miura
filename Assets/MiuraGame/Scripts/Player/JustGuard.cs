using Cinemachine;
using System.Collections;
using UnityEngine;

public class JustGuard : MonoBehaviour
{
    [SerializeField] private PlayerCameraController playerCameraController;
    private Animator enemyAnimator;
    private Animator playerAnimator;
    private bool isSpread = false;
    private bool isNarrow = false;

    private const float DefaultFOV = 70;
    private const float TargetFOV = 50;
    private const float SpreadSpeed = 2;
    private const float NarrowSpeed = 20;

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
        playerCameraController.StartNarrowFOV(TargetFOV, NarrowSpeed);
        playerCameraController.ApplyImpulse();

        enemyAnimator.speed = 0;
        playerAnimator.speed = 0f;
        Debug.Log("Start");

        yield return new WaitForSeconds(delay);

        playerCameraController.StartSpreadFOV(DefaultFOV, SpreadSpeed);

        enemyAnimator.speed = 1;
        playerAnimator.speed = 1;
        Debug.Log("End");
    }
}
