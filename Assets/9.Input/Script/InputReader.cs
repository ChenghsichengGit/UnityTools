using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls; // 輸入控制器

    public Vector2 MovementValue { get; private set; } // 表示角色的移動輸入值
    public bool isAim { get; private set; }
    public bool isAttack { get; private set; }
    public event Action AimEvent;
    public event Action AttackEvent;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        // 啟用輸入控制
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        // 禁用輸入控制
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.ReadValue<float>() == 0)
            isAim = false;
        if (context.ReadValue<float>() == 1)
            isAim = true;

        AimEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.ReadValue<float>() == 0)
            isAttack = false;
        if (context.ReadValue<float>() == 1)
            isAttack = true;

        AttackEvent?.Invoke();
    }
}
