using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    Dictionary<string, EffectData> effectDict = new Dictionary<string, EffectData>();

    public struct EffectData
    {
        public GameObject prefab;
        public float duration;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 再生時間を登録
        RegisterEffectDuration();
    }

    // エフェクトの再生時間を登録する処理
    void RegisterEffectDuration()
    {
        foreach (var effect in effectList)
        {
            if (effect == null) continue;

            float maxDuration = 0;
            var particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                if (ps.main.duration > maxDuration)
                {
                    maxDuration = ps.main.duration;
                }
            }

            if (!effectDict.ContainsKey(effect.name))
            {
                effectDict.Add(effect.name, new EffectData
                {
                    prefab = effect,
                    duration = maxDuration
                });
            }
            else
            {
                Debug.LogWarning($"Effect {effect.name} が重複しています。");
            }
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
        if (!effectDict.TryGetValue(effectName, out var data)) return;

        GameObject gameObject = ObjectPool.Instance.GetGameObject(data.prefab, effectPos, effectRotation);

        StartCoroutine(ReleaseEffect(gameObject, data.duration));
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
