using UnityEngine;
using UnityEngine.UI;

public class EmissionController : MonoBehaviour
{
    [SerializeField] Color glowColor = new Color(1.0f, 1.0f, 0.5f);
    [SerializeField] float glowIntensity = 2.0f;

    void Start()
    {
        GetComponent<Image>().material.SetColor("_EmissionColor", glowColor * glowIntensity);
    }
}
