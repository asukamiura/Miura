using System.Collections;
using TMPro;
using UnityEngine;

public class TimingUIView : MonoBehaviour
{
    [SerializeField] GameObject timingUI;
    [SerializeField] TextMeshProUGUI timingText;

    public void HideTimingUI()
    {
        timingUI.SetActive(false);
    }

    public void ShowTimingUI(string timing)
    {
        timingUI.SetActive(true);

        StartCoroutine(ChangeTimingUI(timing));
    }

    // ガード、回避のタイミングUIの表示処理
    IEnumerator ChangeTimingUI(string timing)
    {
        timingText.text = timing;

        yield return new WaitForSeconds(1);

        timingUI.SetActive(false);
    }
}
