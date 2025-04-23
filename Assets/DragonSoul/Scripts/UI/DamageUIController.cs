using UnityEngine;

public class DamageUIController : MonoBehaviour
{
    const float Radius = 0.5f;
    const float OffsetY = 0.8f;
    
    RectTransform rectTransform;
    Vector3 showPosition;

    public Vector3 TargetPosition { private get; set; }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Vector3 circlePosition = Radius * Random.insideUnitCircle;
        showPosition = new Vector3(circlePosition.x, circlePosition.y + OffsetY, circlePosition.z) + TargetPosition;
    }

    void Update()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, showPosition);
    }
}
