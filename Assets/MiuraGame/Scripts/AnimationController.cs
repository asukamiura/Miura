using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>(); 

    public void ChangeAnimationSpeed(string gameObjectName, float animationSpeed)
    {
        var animator = gameObjects.FirstOrDefault(gameObject => gameObject.name == gameObjectName).GetComponent<Animator>();

        animator.speed  = animationSpeed;
    }

    public void ChangeAllAnimationSpeed(float animationSpeed)
    {
        foreach (var gameObject in gameObjects)
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.speed = animationSpeed;
        }
    }
}
