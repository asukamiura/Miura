using TMPro;
using UnityEngine;

public class DamageUIGenerator : MonoBehaviour
{
    [SerializeField] PlayerAttackBroadcaster attackBroadcaster;
    [SerializeField] GameObject damageUIPrefab;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject overlayUI;

    const float ShowingTime = 0.8f;

    void OnEnable()
    {
        attackBroadcaster.OnHitNotified += HandleHit;
    }

    void OnDisable()
    {
        attackBroadcaster.OnHitNotified -= HandleHit;
    }

    void HandleHit(HitInfo hitInfo)
    {
        GenerateDamageUI(hitInfo.damage, hitInfo.hitPoint, hitInfo.inPowerUp);
    }

    /// <summary>
    /// ダメージUIを生成
    /// </summary>
    /// <param name="damageValue">ダメージ量</param>
    /// <param name="targetPosition">生成する位置</param>
    /// <param name="inPowerUp">パワーアップ中か？</param>
    void GenerateDamageUI(float damageValue, Vector3 targetPosition, bool inPowerUp)
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
