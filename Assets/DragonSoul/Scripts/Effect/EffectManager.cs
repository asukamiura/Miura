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

    public void PlayEffect(string effectName, Vector3 effectPos, Quaternion effectRotation)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        GameObject obj = Instantiate(effect, effectPos, effectRotation);

        StartCoroutine(DestroyEffect(obj, effectDic[effectName]));
    }

    IEnumerator DestroyEffect(GameObject effect, float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(effect);
    }
}
