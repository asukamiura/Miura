using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    Dictionary<string, float> effectDic = new Dictionary<string, float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        RegisterEffectDuration();
    }

    // エフェクトの再生時間を登録する処理
    void RegisterEffectDuration()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            ParticleSystem[] particleSystems = effectList[i].GetComponentsInChildren<ParticleSystem>();

            float maxDuration = 0;

            foreach (ParticleSystem ps in particleSystems)
            {
                float duration = ps.main.duration;

                if (duration > maxDuration)
                {
                    maxDuration = duration;
                }
            }

            effectDic.Add(effectList[i].name, maxDuration);
        }
    }

    /// <summary>
    /// エフェクトを再生
    /// </summary>
    /// <param name="effectName">エフェクトの名前</param>
    /// <param name="effectPos">再生開始位置</param>
    /// <param name="effectRotation">再生開始回転</param>
    public void PlayEffect(string effectName, Vector3 effectPos, Quaternion effectRotation)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        GameObject gameObject = ObjectPool.Instance.GetGameObject(effect,effectPos,effectRotation);

        StartCoroutine(ReleaseEffect(gameObject, effectDic[effectName]));
    }

    public void PlayEffect(GameObject effectPrefab, Vector3 effectPos, Quaternion effectRotation, float duration)
    {
        GameObject gameObject = ObjectPool.Instance.GetGameObject(effectPrefab, effectPos, effectRotation);

        StartCoroutine(ReleaseEffect(gameObject, duration));

    }

    IEnumerator ReleaseEffect(GameObject effect, float duration)
    {
        yield return new WaitForSeconds(duration);

        ObjectPool.Instance.ReleaseGameObject(effect);
    }
}
