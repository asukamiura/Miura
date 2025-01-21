using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>(); 

    public void ChangeAnimationSpeed(string tagName, float animationSpeed)
    {
        var gameObject = gameObjects.FirstOrDefault(gameObject => gameObject.tag == tagName);

        var animator = gameObject.GetComponent<Animator>();
        animator.speed  = animationSpeed;

        if (gameObject.CompareTag("Enemy"))
        {
            NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            navMeshAgent.speed *= animationSpeed;
            navMeshAgent.acceleration *= animationSpeed;
        }
    }

    public void ChangeAllAnimationSpeed(float animationSpeed)
    {
        foreach (var gameObject in gameObjects)
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.speed = animationSpeed;

            if (gameObject.CompareTag("Enemy"))
            {
                NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
                navMeshAgent.speed = animationSpeed;
                //navMeshAgent.acceleration *= animationSpeed;
            }
        }
    }
}
