using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectEventHandler : MonoBehaviour
{
    protected GameObject[] effects;
    protected Dictionary<int, float> effectDurationDic = new Dictionary<int, float>();
    protected Coroutine[] hideCroutines;

    void Awake()
    {
        effects = GetEffectArray();
        hideCroutines = new Coroutine[effects.Length];
        RegisterEffectDuration();
    }

    protected abstract GameObject[] GetEffectArray();

    // エフェクトの再生時間を登録
    void RegisterEffectDuration()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            ParticleSystem[] particleSystems = effects[i].GetComponentsInChildren<ParticleSystem>();

            // 一番長い再生時間を取得
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems)
            {
                float duration = ps.main.duration;

                if (duration > maxDuration)
                {
                    maxDuration = duration;
                }
            }

            effectDurationDic.Add(i, maxDuration);
        }
    }

    /// <summary>
    /// 通常攻撃のエフェクトを表示
    /// </summary>
    /// <param name="comboIndex">通常攻撃の段数</param>
    protected void ShowEffect(int comboIndex)
    {
        if (comboIndex < 1 || comboIndex > effects.Length) { return; }

        int index = comboIndex - 1;

        if (hideCroutines[index] != null)
        {
            StopCoroutine(hideCroutines[index]);
            hideCroutines[index] = null;
            effects[index].SetActive(false);
        }

        effects[index].SetActive(true);

        hideCroutines[index] = StartCoroutine(WaitHide(effects[index], effectDurationDic[index]));
    }

    /// <summary>
    ///保持しているエフェクトすべて再生
    /// </summary>
    protected void ShowEffect()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (hideCroutines[i] != null)
            {
                StopCoroutine(hideCroutines[0]);
                hideCroutines[i] = null;
                effects[i].SetActive(false);
            }

            effects[i].SetActive(true);
            hideCroutines[i] = StartCoroutine(WaitHide(effects[i], effectDurationDic[i]));
        }
    }

    /// <summary>
    /// エフェクトを再生時間を待って非表示にする
    /// </summary>
    /// <param name="effect">非表示にするエフェクト</param>
    /// <param name="duration">待つ時間</param>
    /// <returns></returns>
    protected IEnumerator WaitHide(GameObject effect, float duration)
    {
        yield return new WaitForSeconds(duration);

        effect.SetActive(false);
    }
}
