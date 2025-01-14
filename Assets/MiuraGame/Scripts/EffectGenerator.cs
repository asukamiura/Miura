using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> effectList = new List<GameObject>();

    public void PlayEffect(string effectName, Vector3 playPos, Quaternion playRotation)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        GameObject obj = Instantiate(effect, playPos, playRotation);

        StartCoroutine(DestroyEffect(obj));
    }

    IEnumerator DestroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(effect);
    }
}
