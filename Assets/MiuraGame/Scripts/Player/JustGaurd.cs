using System.Collections;
using UnityEngine;

public class JustGaurd : MonoBehaviour
{
    private Animator enemyAnimator;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }

    public void ActionJustGaurd()
    {
        StartCoroutine(ApplyJustGaurd(1));
    }

    private IEnumerator ApplyJustGaurd(float delay)
    {
        enemyAnimator.speed = 0;
        playerAnimator.speed = 0;
        Debug.Log("Start");

        yield return new WaitForSeconds(delay);

        enemyAnimator.speed = 1;
        playerAnimator.speed = 1;
        Debug.Log("End");
    }
}
