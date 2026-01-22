using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    Dictionary<string, GameObject> effectDict = new Dictionary<string, GameObject>();

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

        // エフェクトを登録
        Register();
    }

    // エフェクトの再生時間を登録する処理
    void Register()
    {
        foreach (var effect in effectList)
        {
            if (effect == null) continue;      

            if (!effectDict.ContainsKey(effect.name))
            {
                effectDict.Add(effect.name, effect);
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
    /// <param name="effectName">エフェクト名</param>
    /// <param name="effectPos">再生開始位置</param>
    public void PlayEffect(string effectName, Vector3 effectPos)
    {
        if (!effectDict.TryGetValue(effectName, out var effect)) return;

        ObjectPool.Instance.GetGameObject(effect, effectPos, effect.transform.rotation);
    }

    /// <summary>
    /// エフェクトを再生
    /// </summary>
    /// <param name="effectName">エフェクトの名前</param>
    /// <param name="effectPos">再生開始位置</param>
    /// <param name="effectRotation">再生開始回転</param>
    public void PlayEffect(string effectName, Vector3 effectPos, Quaternion effectRotation)
    {
        if (!effectDict.TryGetValue(effectName, out var effect)) return;

        ObjectPool.Instance.GetGameObject(effect, effectPos, effectRotation);
    }

    /// <summary>
    /// エフェクトを再生
    /// </summary>
    /// <param name="effectPrefab">エフェクトプレハブ</param>
    /// <param name="effectPos">再生開始位置</param>
    /// <param name="effectRotation">>再生開始回転</param>
    /// <param name="duration">継続時間</param>
    public void PlayEffect(GameObject effectPrefab, Vector3 effectPos, Quaternion effectRotation, float duration)
    {
        ObjectPool.Instance.GetGameObject(effectPrefab, effectPos, effectRotation);
    }
}
