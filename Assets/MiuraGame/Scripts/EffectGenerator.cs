using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> effectList = new List<GameObject>();

    public void PlayEffect(string effectName, Vector3 playPos, Quaternion playRotation)
    {
        var effect = effectList.FirstOrDefault(effect => effect.name == effectName);

        if (effect == null) { return; }

        Instantiate(effect, playPos, playRotation);
    }
}
