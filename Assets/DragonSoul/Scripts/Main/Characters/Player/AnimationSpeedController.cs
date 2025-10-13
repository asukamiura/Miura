using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }    
}
