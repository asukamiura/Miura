using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Animator Animator => animator;
    public AnimatorStateInfo StateInfo { get; private set; }

    void Update()
    {
        StateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    public void PlayAniamtion(string animName, float transitionDuration = 0, int layer = 0, float timeOffset = 0)
    {
        animator.CrossFade(animName, transitionDuration, layer, timeOffset);
    }

    public void ChangeAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    public bool IsTimeElapsed(string currentAnimName, float transtionTime)
    {
        if (StateInfo.IsName(currentAnimName) && StateInfo.normalizedTime >= transtionTime)
        {
            return true;
        }

        return false;
    }

    public void SetRootMotion(bool enabled)
    {
        animator.applyRootMotion = enabled;
    }

    public void SetFloat(string parameterName, float value, float dampTime)
    {
        animator.SetFloat(parameterName, value, dampTime, Time.deltaTime);
    }
}
