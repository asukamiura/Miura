using Player;
using System.Collections;
using UnityEngine;

public class BreathProjectile : MonoBehaviour
{
    [SerializeField] float speed = 10;      // 投射物の速度
    [SerializeField] float lifetime = 5;    // 投射物を自動的に非表示するまでの時間
    [SerializeField] float explosionEffectDuration = 3;    // 爆発エフェクト再生時間
    [SerializeField] GameObject explosionEffect;

    Coroutine lifeTimer;
    Rigidbody rb;

    public void Launch(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * speed;
        lifeTimer = StartCoroutine(AutoDestroy());
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Floor") || collider.CompareTag("Player"))
        {
            if (collider.CompareTag("Player"))
            {
                var player = collider.GetComponentInParent<PlayerCore>();
                if (player.IsInvincible)
                {
                    return;
                }
            }

            EffectManager.Instance.PlayEffect(explosionEffect, transform.position, transform.rotation, explosionEffectDuration);
            Release();
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifetime);

        Release();
    }

    void Release()
    {
        if (lifeTimer != null)
        {
            StopCoroutine(lifeTimer);
        }
        ObjectPool.Instance.ReleaseGameObject(gameObject);
    }

    void OnDisable()
    {
        rb.velocity = Vector3.zero;                
    }
}
