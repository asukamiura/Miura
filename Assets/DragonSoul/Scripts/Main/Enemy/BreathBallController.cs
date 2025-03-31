using UnityEngine;

public class BreathBallController : MonoBehaviour
{
    Quaternion effectRotation = Quaternion.identity;

    const float ShowingTime = 1.0f;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Floor"))
        {
            EffectManager.Instance.PlayEffect("FrostExplosionMega", transform.position, effectRotation);
            Destroy(gameObject);
        }
    }
}
