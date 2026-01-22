using UnityEngine;
using UnityEngine.UI;

public class ChangeInputUI : MonoBehaviour
{
    [Header("切り替えるUI画像をセットで登録")]
    [SerializeField] Sprite gamepadUI;
    [SerializeField] Sprite keyboardMouseUI;

    Image ui;

    void Start()
    {
        ui = GetComponent<Image>();    
        
        // 初期Sprite設定
        ui.sprite = InputReceiver.Instance.IsGamepad ? gamepadUI : keyboardMouseUI;

        InputReceiver.Instance.OnDeviceChanged += OnDeviceChanged;
    }

    void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.OnDeviceChanged -= OnDeviceChanged;
        }        
    }

    void OnDeviceChanged(bool isGamepad)
    {
        ui.sprite = isGamepad ? gamepadUI : keyboardMouseUI;
    }
}
