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
    }

    private void Update()
    {
        // InputReciverから現在の操作状態を取得
        if (InputReciver.Instance.IsGamepad)
        {
            ui.sprite = gamepadUI;
        }
        else
        {
            ui.sprite = keyboardMouseUI;
        }        
    }
}
