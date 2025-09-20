using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputHandler
{
    // Subscribable Events
    public Action OnHold;
    public Action OnCancelHold;
    public Action OnTap;
    public Action OnDoubleTap;

    public bool _isHolding = false;

    private PlayerInputActions _pia;
    private Coroutine _tapCoroutine = null;
    private int _taps = 0;
    private bool _tapReleased = false;

    public MovementInputHandler(PlayerInputActions pia)
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
        _taps++;

        if(_tapCoroutine == null)
        {
            _tapCoroutine = CoroutineRunner.Instance.StartCoroutine(TapCoroutine());
        }
    }

    private void CancelTap(InputAction.CallbackContext ctx)
    {
        _tapReleased = true;
    }

    private void BindInputActions()
    {
        _pia.Movement.Hold.performed += HandleHold;
        _pia.Movement.Hold.canceled += CancelHold;
        _pia.Movement.Tap.performed += HandleTap;
        _pia.Movement.Tap.canceled += CancelTap;
    }

    private IEnumerator TapCoroutine()
    {
        yield return new WaitForSeconds(0.05f);

        if (_taps == 1 && _tapReleased)
        {
            OnTap?.Invoke();
        }
        else if (_taps == 1 && !_tapReleased)
        {
            yield return new WaitForSeconds(.1f);

            if (_taps == 1)
            {
                OnTap?.Invoke();
            }
        }
        
        if (_taps > 1)
        {
            OnDoubleTap?.Invoke();
        }

        _taps = 0;
        _tapReleased = false; 
        _tapCoroutine = null;
    }
}