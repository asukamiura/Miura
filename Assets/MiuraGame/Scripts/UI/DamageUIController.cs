using UnityEngine;

public class DamageUIController : MonoBehaviour
{
    RectTransform rectTransform;

    public Vector3 targetPosition;

    const float OffsetY = 1;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(targetPosition.x, targetPosition.y + OffsetY, targetPosition.z));
    }
}
