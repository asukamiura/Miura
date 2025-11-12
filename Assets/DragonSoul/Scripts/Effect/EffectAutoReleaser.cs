using System.Collections;
using UnityEngine;

public class EffectAutoReleaser : MonoBehaviour
{
    float maxDuration = 0;

    void Awake()
    {
        RegisterEffectDuration();
    }

    void OnEnable()
    {        
        StartCoroutine(WaitRelease());
    }

    // エフェクトの再生時間を登録
    void RegisterEffectDuration()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            float duration = ps.main.duration;

            if (duration > maxDuration)
            {
                maxDuration = duration;
            }
        }
    }

    // 登録された再生時間になったら、削除
    IEnumerator WaitRelease()
    {
        yield return new WaitForSeconds(maxDuration);

        ObjectPool.Instance.ReleaseGameObject(gameObject);
    }
}
