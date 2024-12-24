using System.Collections;
using UnityEngine;

public class JustGuard : MonoBehaviour
{
    [SerializeField] PlayerCameraController playerCameraController;
    private Animator enemyAnimator;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }

    public void ActionJustGuard()
    {
        StartCoroutine(ApplyJustGuard(0.5f));
    }

    private IEnumerator ApplyJustGuard(float delay)
    {
        playerCameraController.ChangeCameraPriority();
        enemyAnimator.speed = 0;
        playerAnimator.speed = 0f;
        Debug.Log("Start");

        yield return new WaitForSeconds(delay);

        playerCameraController.ResetCameraPriority();
        enemyAnimator.speed = 1;
        playerAnimator.speed = 1;
        Debug.Log("End");
    }
}
