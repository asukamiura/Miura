using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReciver : MonoBehaviour
{
    static InputReciver instance;
    GameInput gameInput;
    public static InputReciver Instance => instance;

    public Vector2 Move { get { return gameInput.Player.Move.ReadValue<Vector2>(); } }
    public bool AttackNormal { get { return gameInput.Player.AttackNormal.triggered; } }


    void Awake()
    {
        gameInput = new GameInput();

        if (instance != null )
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void OnEnable() => gameInput.Enable();
    void OnDisable() => gameInput.Disable();
    void OnDestroy() => gameInput.Dispose();
}
