using TMPro;
using UnityEngine;

public class DamageUIGenerator : MonoBehaviour
{
    [SerializeField] GameObject damageUIPrefab;
    [SerializeField] Canvas uiCanvas;

    public void GenerateDamageUI(float damageValue, Vector3 targetPosition)
    {
        // ダメージUIを生成
        GameObject damageUI = Instantiate(damageUIPrefab);

        // ダメージ量をUIに反映
        damageUI.GetComponent<TextMeshProUGUI>().text = damageValue.ToString();

        // Canvasの子オブジェクトに設定
        damageUI.transform.SetParent(uiCanvas.transform);

        damageUI.GetComponent<DamageUIController>().targetPosition = targetPosition;

        Destroy(damageUI, 1f);
    }
}
