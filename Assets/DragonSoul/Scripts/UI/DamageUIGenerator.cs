using TMPro;
using UnityEngine;

public class DamageUIGenerator : MonoBehaviour
{
    [SerializeField] GameObject damageUIPrefab;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject overlayUI;

    const float ShowingTime = 0.8f;

    public void GenerateDamageUI(float damageValue, Vector3 targetPosition, bool inPowerUp)
    {
        if (damageValue == 0) { return; }

        // ダメージUIを生成
        GameObject damageUI = Instantiate(damageUIPrefab);

        // TextMeshProUGUIを取得
        TextMeshProUGUI damageUITextMesh = damageUI.GetComponent<TextMeshProUGUI>();

        // ダメージ量をUIに反映
        damageUITextMesh.text = damageValue.ToString();

        // パワーアップ中はシアンに、それ以外はホワイトに色を設定
        if (inPowerUp)
        {
            damageUITextMesh.color = Color.cyan;
        }
        else
        {
            damageUITextMesh.color = Color.white;
        }

        // Canvasの子オブジェクトに設定
        damageUI.transform.SetParent(overlayUI.transform);

        // 生成位置をDamaUIに渡す
        damageUI.GetComponent<DamageUIController>().TargetPosition = targetPosition;

        Destroy(damageUI, ShowingTime);
    }
}
