using UnityEngine;
using UnityEngine.Events;

public class MinigameController : MonoBehaviour
{
    [Header("Input Events")]
    [SerializeField] private UnityEvent _onHold;
    [SerializeField] private UnityEvent _onHoldRelease;
    [SerializeField] private UnityEvent _onTap;

    private MinigameInputHandler _inputHandler;

    void Start()
    {
        BindInputActions();
    }

    public void SetMinigameComplete()
    {
        InputManager.Instance.SetMovementMode();
        this.gameObject.SetActive(false);
    }

    // Binds input events to the input handler
    private void BindInputActions()
    {
        _inputHandler = InputManager.Instance.minigameInput;
        _inputHandler.OnHold += HandleHold;
        _inputHandler.OnCancelHold += CancelHold;
        _inputHandler.OnTap += HandleTap;
    }

    private void HandleHold()
    {
        _onHold?.Invoke();
    }

    private void CancelHold()
    {
        _onHoldRelease?.Invoke();
    }

    private void HandleTap()
    {
        _onTap?.Invoke();
    }
}
