using TMPro;
using UnityEngine;

public class DamageUIGenerator : MonoBehaviour
{
    [SerializeField] GameObject damageUIPrefab;
    [SerializeField] Canvas uiCanvas;

    const float ShowingTime = 0.8f;

    public void GenerateDamageUI(float damageValue, Vector3 targetPosition)
    {
        if (damageValue == 0) { return; }
        // ダメージUIを生成
        GameObject damageUI = Instantiate(damageUIPrefab);

        // ダメージ量をUIに反映
        damageUI.GetComponent<TextMeshProUGUI>().text = damageValue.ToString();

        // Canvasの子オブジェクトに設定
        damageUI.transform.SetParent(uiCanvas.transform);

        // 生成位置をDamaUIに渡す
        damageUI.GetComponent<DamageUIController>().targetPosition = targetPosition;

        Destroy(damageUI, ShowingTime);
    }
}
