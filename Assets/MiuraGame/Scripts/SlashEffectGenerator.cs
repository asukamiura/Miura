using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlashEffectGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> effectList = new List<GameObject>();

    public void GenerateEffect(string effectName)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        effect.SetActive(true);

        StartCoroutine(DestryEffect(effect));
    }

    IEnumerator DestryEffect(GameObject effect)
    {
        yield return new WaitForSeconds(0.45f);

        effect.SetActive(false);
    }
}
