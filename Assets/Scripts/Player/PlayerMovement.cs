using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Direction Indicator Variables")]
    [SerializeField] private Transform _movementDirectionIndicator;
    [SerializeField] private float _movementRadius = 5f;
    [SerializeField] private float _indicatorSpeed = 1f;

    [Header("Movement Variables")]
    [SerializeField] private float _movementSpeed = 5f;
    private MovementInputHandler _movementInputHandler;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent _onTap;
    [SerializeField] private UnityEvent _onDoubleTap;

    private float _angle = 0f;
    private bool _holding = false;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        BindInputActions();
    }

    void Update()
    {
        UpdatePlayerVelocity();
        UpdateMovementIndicatorPos();
    }

    // Binds input events to the input handler
    private void BindInputActions()
    {
        _movementInputHandler = InputManager.Instance.movementInput;
        _movementInputHandler.OnHold += HandleHold;
        _movementInputHandler.OnCancelHold += CancelHold;
        _movementInputHandler.OnTap += HandleTap;
        _movementInputHandler.OnDoubleTap += HandleDoubleTap;
    }

    private void HandleHold()
    {
        _holding = true;
    }

    private void CancelHold()
    {
        _holding = false;
    }

    private void HandleTap()
    {
        _onTap?.Invoke();
    }

    private void HandleDoubleTap()
    {
        _onDoubleTap?.Invoke();
    }

    // Update the player velocity based on angle, move speed, and hold state
    private void UpdatePlayerVelocity()
    {
        if(_holding)
        {
            float velX = Mathf.Cos(_angle) * _movementSpeed;
            float velY = Mathf.Sin(_angle) * _movementSpeed;

            _rb.linearVelocity = new Vector2(velX, velY);

            return;
        }

        _rb.linearVelocity = Vector2.zero;
    }

    // Update the position of the movement indicator
    private void UpdateMovementIndicatorPos()
    {
        if(_holding)
        {
            return;
        }
        
        // Get the new angle according to time in radians
        _angle = (_angle + (Time.deltaTime * _indicatorSpeed)) % 6.28f;

        // Calculate the positions based on the angle and radius
        float posX = _movementRadius * Mathf.Cos(_angle);
        float posY = _movementRadius * Mathf.Sin(_angle);

        _movementDirectionIndicator.localPosition = new Vector2(posX, posY);
    }
}