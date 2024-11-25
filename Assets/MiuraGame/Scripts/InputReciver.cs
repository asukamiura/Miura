using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReciver : MonoBehaviour
{
    public static InputReciver Instance { get; private set; }
    GameInput gameInput;
    float val;
    private bool countStart = false;   
    public float countTime = 0;    

    public Vector2 Move { get { return gameInput.Player.Move.ReadValue<Vector2>(); } }
    public bool Dodge { get { return gameInput.Player.Dodge.triggered; } }
    public bool Guard { get { return gameInput.Player.Parry.triggered; } }
    public bool AttackNormal { get { return gameInput.Player.AttackNormal.WasReleasedThisFrame() && countTime < 2; } }
    public bool AttackCharge { get { return gameInput.Player.AttackNormal.WasReleasedThisFrame() && countTime >= 2; } }
    public bool Heal { get { return gameInput.Player.Heal.WasPressedThisFrame(); } }
    public bool PowerUp { get { return gameInput.Player.PowerUp.WasPressedThisFrame(); } }


    private void Awake()
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

    private void Update()
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

    void OnEnable() => gameInput.Enable();
    void OnDisable() => gameInput.Disable();
    void OnDestroy() => gameInput.Dispose();
}
