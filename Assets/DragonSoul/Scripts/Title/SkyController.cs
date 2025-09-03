using UnityEngine;

public class SkyController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    float currentTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", currentTime * rotationSpeed);
    }

    private void OnDisable()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}
