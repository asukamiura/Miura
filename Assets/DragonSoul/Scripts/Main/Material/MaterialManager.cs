using System.Collections;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] Material forceFieldMaterial;

    float currentVelocity = 0;

    const float DisabledSmoothTime = 0.3f;
    const float ActiveSmoothTime = 0;
    const float DefaultFresnalPower = 0;
    const float ActiveFresnalPower = 3;

    public static MaterialManager Instance { get; private set; }
    public bool IsActiveForceField { get; set; } = false;
    public bool IsDisabledForceField { get; set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        forceFieldMaterial.SetFloat("_FresnelPower", DefaultFresnalPower);
    }

    void Update()
    {
        if (IsActiveForceField)
        {
            forceFieldMaterial.SetFloat("_FresnelPower", Mathf.SmoothDamp(forceFieldMaterial.GetFloat("_FresnelPower"), ActiveFresnalPower, ref currentVelocity, ActiveSmoothTime));

            if (Mathf.Abs(ActiveFresnalPower - forceFieldMaterial.GetFloat("_FresnelPower")) < 0.1f)
            {
                forceFieldMaterial.SetFloat("_FresnelPower", ActiveFresnalPower);

                IsActiveForceField = false;
            }
        }

        if (IsDisabledForceField)
        {
            forceFieldMaterial.SetFloat("_FresnelPower", Mathf.SmoothDamp(forceFieldMaterial.GetFloat("_FresnelPower"), DefaultFresnalPower, ref currentVelocity, DisabledSmoothTime));

            if (Mathf.Abs(DefaultFresnalPower - forceFieldMaterial.GetFloat("_FresnelPower")) < 0.1f)
            {
                forceFieldMaterial.SetFloat("_FresnelPower", DefaultFresnalPower);

                IsDisabledForceField = false;
            }
        }
    }

    IEnumerator ChangeFresnelPower(float targetFresnelPower, float duration)
    {
        float startFresnelPower = forceFieldMaterial.GetFloat("_FresnelPower");
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            forceFieldMaterial.SetFloat("_FresnelPower", Mathf.Lerp(startFresnelPower, targetFresnelPower, time / duration));
            yield return null;
        }

        forceFieldMaterial.SetFloat("_FresnelPower", targetFresnelPower);
    }

    public void PlayChangeFresnelPower(float targetFresnelPower, float duration)
    {
        StopCoroutine("ChangeFresnelPower");

        StartCoroutine(ChangeFresnelPower(targetFresnelPower, duration));
    }

    void OnDisable()
    {
        forceFieldMaterial.SetFloat("_FresnelPower", DefaultFresnalPower);
    }
}
