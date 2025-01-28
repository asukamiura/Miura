using UnityEngine;

public class InputReciver : MonoBehaviour
{
    public static InputReciver Instance { get; set; }
    GameInput gameInput;
    bool countStart = false;    // 通常攻撃ボタンの入力時間の計測開始フラグ   
    float countTime = 0;    // 通常攻撃ボタンの入力時間 

    // プレイヤー操作用
    public Vector2 Look { get { return gameInput.Player.Look.ReadValue<Vector2>(); } }
    public Vector2 Move { get { return gameInput.Player.Move.ReadValue<Vector2>(); } }
    public bool Dodge { get { return gameInput.Player.Dodge.WasPressedThisFrame(); } }
    public bool Guard { get { return gameInput.Player.Parry.WasPressedThisFrame(); } }
    public bool AttackNormal { get { return gameInput.Player.AttackNormal.WasReleasedThisFrame() && countTime < 2; } }
    public bool AttackCharge { get { return gameInput.Player.AttackNormal.WasReleasedThisFrame() && countTime >= 2; } }
    public bool AttackUltimate { get { return gameInput.Player.AttackUltimate.WasPressedThisFrame(); } }
    public bool Heal { get { return gameInput.Player.Heal.WasPressedThisFrame(); } }
    public bool PowerUp { get { return gameInput.Player.PowerUp.WasPressedThisFrame(); } }
    public bool Pause { get { return gameInput.Player.Pause.WasReleasedThisFrame(); } }

    // UI操作用
    public bool Decision { get { return gameInput.UI.Decision.WasPressedThisFrame(); } }
    public bool SelectMoveUp { get { return gameInput.UI.SelectMoveUp.WasPressedThisFrame(); } }
    public bool SelectMoveDown { get { return gameInput.UI.SelectMoveDown.WasPressedThisFrame(); } }
    public bool SelectMoveLeft { get { return gameInput.UI.SelectMoveLeft.WasPressedThisFrame(); } }
    public bool SelectMoveRight { get { return gameInput.UI.SelectMoveRight.WasPressedThisFrame(); } }

    void OnEnable() => gameInput.Enable();
    void OnDisable() => gameInput.Disable();
    void OnDestroy() => gameInput.Dispose();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameInput = new GameInput();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameInput.Player.AttackNormal.WasPressedThisFrame())
        {
            countStart = true;
        }
        if (gameInput.Player.AttackNormal.WasReleasedThisFrame())
        {
            countStart = false;
        }

        if (countStart)
        {
            countTime += Time.deltaTime;
        }
    }

    public void ResetInputCountTime()
    {
        countTime = 0;
    }
}
