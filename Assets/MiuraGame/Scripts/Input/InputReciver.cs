using UnityEngine;
using UnityEngine.InputSystem;

public class InputReciver : MonoBehaviour
{
    public static InputReciver Instance { get; set; }
    public bool IsGamepad { get; set; }    // ゲームパッド使用時はtrue、キーボード&マウス使用時はfalse

    // それぞれのデバイスの全ての入力を取得する
    InputAction gamepadAny = new InputAction(type: InputActionType.PassThrough, binding: "<Gamepad>/*", interactions: "Press");
    InputAction keyboardAny = new InputAction(type: InputActionType.PassThrough, binding: "<Keyboard>/*", interactions: "Press");
    InputAction mouseAny = new InputAction(type: InputActionType.PassThrough, binding: "<Mouse>/*", interactions: "Press");

    GameInput gameInput;

    // プレイヤー操作用
    public Vector2 Look { get { return gameInput.Player.Look.ReadValue<Vector2>(); } }
    public Vector2 Move { get { return gameInput.Player.Move.ReadValue<Vector2>(); } }
    public bool Dash { get { return gameInput.Player.Dodge.WasPressedThisFrame(); } }
    public bool Guard { get { return gameInput.Player.Parry.WasPressedThisFrame(); } }
    public bool AttackNormal { get { return gameInput.Player.AttackNormal.WasPressedThisFrame(); } }
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

    void OnEnable()
    {
        gameInput.Enable();
        gamepadAny.Enable();
        keyboardAny.Enable();
        mouseAny.Enable();
    }

    void OnDisable()
    {
        gameInput.Disable();
        gamepadAny.Disable();
        keyboardAny.Disable();
        mouseAny.Disable();
    }

    void OnDestroy()
    {
        gameInput.Dispose();
    }

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
        if (gamepadAny.triggered)
        {
            IsGamepad = true;
        }

        if (keyboardAny.triggered || mouseAny.triggered)
        {
            IsGamepad = false;
        }        
    }
}
