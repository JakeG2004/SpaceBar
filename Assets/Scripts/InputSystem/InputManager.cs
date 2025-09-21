// InputManager.cs
// A script which manages input mappings, and sends out inputs as subscribable events
// Author:  Jake Gendreau
// Date:    9/19/25

using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private bool _startInMovementMode = true;

    private PlayerInputActions _pia;

    // The action maps which other scripts will reference
    [HideInInspector] public MovementInputHandler movementInput;
    [HideInInspector] public MinigameInputHandler minigameInput;

    void Awake()
    {
        // Singleton pattern
        if(Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Create a new Player Controls class
        if(_pia == null)
        {
            _pia = new PlayerInputActions();
        }

        GetComponentReferences();

        if(_startInMovementMode)
        {
            SetMovementMode();
        }
        else
        {
            SetMinigameMode();
        }
    }

    public void SetMinigameMode()
    {
        _pia.Minigames.Enable();
        _pia.Movement.Disable();
    }

    public void SetMovementMode()
    {
        _pia.Minigames.Disable();
        _pia.Movement.Enable();
    }

    private void GetComponentReferences()
    {
        movementInput = new MovementInputHandler(_pia);
        minigameInput = new MinigameInputHandler(_pia);
    }
}
