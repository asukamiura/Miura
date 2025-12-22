using System.Collections;
using TMPro;
using UnityEngine;

public class TimingUIView : MonoBehaviour
{
    [SerializeField] GameObject timingUI;
    [SerializeField] TextMeshProUGUI timingText;
    const float Duration = 1.0f;    // 表示時間

    public void HideTimingUI()
    {
        timingUI.SetActive(false);
    }

    public void ShowTimingUI(TimingType timing)
    {
        timingUI.SetActive(true);

        StartCoroutine(ChangeTimingUI(timing.ToString()));
    }

    // ガード、回避のタイミングUIの表示処理
    IEnumerator ChangeTimingUI(string timing)
    {
        timingText.text = timing;

        yield return new WaitForSeconds(Duration);

        timingUI.SetActive(false);
    }
}
