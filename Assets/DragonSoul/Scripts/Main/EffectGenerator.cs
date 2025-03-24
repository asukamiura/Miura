using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    public static EffectGenerator Instance { get; private set; }
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

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
    }

    public void PlayEffect(string effectName, Vector3 effectPos, Quaternion effectRotation, float showingTime)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        GameObject obj = Instantiate(effect, effectPos, effectRotation);

        StartCoroutine(DestroyEffect(obj, showingTime));
    }

    IEnumerator DestroyEffect(GameObject effect, float showingTime)
    {
        yield return new WaitForSeconds(showingTime);

        Destroy(effect);
    }
}
