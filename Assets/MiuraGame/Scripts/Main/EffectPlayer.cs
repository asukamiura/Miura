using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    /// <summary>
    /// エフェクトを再生
    /// </summary>
    /// <param name="effectName">エフェクト名</param>
    /// <param name="showingTime">エフェクト表示する時間</param>
    public void PlayEffect(string effectName, float showingTime)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null || effect.activeSelf) { return; }

        effect.SetActive(true);

        StartCoroutine(DestroyEffect(effect, showingTime));
    }

    IEnumerator DestroyEffect(GameObject effect, float showingTime)
    {
        yield return new WaitForSeconds(showingTime);

        effect.SetActive(false);
    }
}
