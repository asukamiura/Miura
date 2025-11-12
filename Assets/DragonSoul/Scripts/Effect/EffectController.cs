using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    Dictionary<string, float> effectDic = new Dictionary<string, float>();

    void Awake()
    {
        RegisterEffectDuration();
    }

    // エフェクトの再生時間を登録
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
    /// エフェクトを再生(登録された時間で終了)
    /// </summary>
    /// <param name="effectName">エフェクト名</param>
    public void ShowEffect(string effectName)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        if (effect.activeSelf)
        {
            effect.SetActive(false);
        }

        effect.SetActive(true);

        StartCoroutine(HideEffect(effect, effectDic[effectName]));
    }

    /// <summary>
    /// エフェクトを再生(手動で時間指定)
    /// </summary>
    /// <param name="effectName">エフェクト名</param>
    /// <param name="duration">エフェクト表示する時間</param>
    public void ShowEffect(string effectName, float duration)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null || effect.activeSelf) { return; }

        effect.SetActive(true);

        StartCoroutine(HideEffect(effect, duration));
    }

    IEnumerator HideEffect(GameObject effect, float duration)
    {
        yield return new WaitForSeconds(duration);

        effect.SetActive(false);
    }
}
