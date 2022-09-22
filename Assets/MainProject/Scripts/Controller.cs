using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private IControllable target = null;
    private IControllable mouse = null;

    private PlayerMainControls actions = null;

    private void Awake()
    {
        actions = new PlayerMainControls();
    }

    public void RegisterInputAction()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += OnMoveInput;

        actions.Player.Attack.performed += OnAttackInput;

        actions.Player.MouseDelta.performed += OnMouseDelta;

        actions.Player.Rolling.performed += OnRollingInput;

        actions.Player.Jump.performed += OnJumpInput;

        actions.Player.Inventory.performed += OnInventoryInput;

        actions.Player.Pause.performed += OnPauseInput;

        SetTarget(GameManager.Inst.MainPlayer as IControllable);
        SetMouse(GameManager.Inst.MainCamera as IControllable);
    }

    public void UnRegisterInputAction()
    {
        actions.Player.Move.performed -= OnMoveInput;

        actions.Player.Attack.performed -= OnAttackInput;

        actions.Player.MouseDelta.performed -= OnMouseDelta;

        actions.Player.Rolling.performed -= OnRollingInput;

        actions.Player.Jump.performed -= OnJumpInput;

        actions.Player.Inventory.performed -= OnInventoryInput;

        actions.Player.Pause.performed -= OnPauseInput;

        actions.Player.Disable();
    }

    public void SetTarget(IControllable controllTarget)
    {
        target = controllTarget;
    }

    public void SetMouse(IControllable controllMouse)
    {
        mouse = controllMouse;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        target.KeyboardInputDir = context.ReadValue<Vector2>();
        //Debug.Log(context.ReadValue<Vector2>());
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouse.MouseDelta = context.ReadValue<Vector2>();
        //Debug.Log(context.ReadValue<Vector2>());
    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {
        target.OnAttack();
    }

    private void OnRollingInput(InputAction.CallbackContext context)
    {
        target.OnRolling();
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        target.OnJump();
    }

    private void OnInventoryInput(InputAction.CallbackContext context)
    {
        target.OnInventory();
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        target.OnPause();
    }
}
