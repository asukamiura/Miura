using UnityEngine;

public class BreathController : MonoBehaviour
{
    [SerializeField] Transform breathPoint;
    [SerializeField] GameObject projectilePrefab;

    public void GenerateEnergyBall()
    {
        GameObject effect = ObjectPool.Instance.GetGameObject(projectilePrefab, breathPoint.transform.position, breathPoint.transform.rotation);

        var projectile = effect.GetComponent<BreathProjectile>();

        if (projectile != null)
        {
            projectile.Launch(transform.forward);
        }
    }
}
