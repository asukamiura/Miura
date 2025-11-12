using TMPro;
using UnityEngine;

public class LockOnUIView : MonoBehaviour
{
    [SerializeField] GameObject lockOnUI;
    [SerializeField] TextMeshProUGUI conditionText;
    Color onColor = Color.green;
    Color offColor = Color.black;

    public void ChangeLockonUI(bool condition)
    {
        lockOnUI.SetActive(!condition);

        if (condition)
        {
            conditionText.text = "ON";
            conditionText.color = onColor;
        }
        else
        {
            conditionText.text = "OFF";
            conditionText.color = offColor;
        }
    }
}
