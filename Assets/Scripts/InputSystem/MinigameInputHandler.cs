using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameInputHandler
{
    // Subscribable Events
    public Action OnHold;
    public Action OnCancelHold;
    public Action OnTap;

    public bool _isHolding = false;

    private PlayerInputActions _pia;

    public MinigameInputHandler(PlayerInputActions pia)
    {
        _pia = pia;

        BindInputActions();
    }

    // Invokes the actions
    private void HandleHold(InputAction.CallbackContext ctx)
    {
        _isHolding = true;
        OnHold?.Invoke();
    }

    private void CancelHold(InputAction.CallbackContext ctx)
    {
        if(!_isHolding)
        {
            return;
        }

        _isHolding = false;
        OnCancelHold?.Invoke();
    }

    private void HandleTap(InputAction.CallbackContext ctx)
    {
        OnTap?.Invoke();
    }

    private void BindInputActions()
    {
        _pia.Minigames.Hold.performed += HandleHold;
        _pia.Minigames.Hold.canceled += CancelHold;
        _pia.Minigames.Tap.performed += HandleTap;
    }
}