using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class MinigameController : MonoBehaviour
{
    [Header("Input Events")]
    [SerializeField] private UnityEvent _onHold;
    [SerializeField] private UnityEvent _onHoldRelease;
    [SerializeField] private UnityEvent _onTap;
    
    [Header("Other")]
    [SerializeField] private GameObject _sceneObjects;

    private MinigameInputHandler _inputHandler;
    
    void OnEnable()
    {
        StartCoroutine(WaitAndBind());

        if(_sceneObjects != null)
            _sceneObjects.SetActive(false);
    }

    void OnDisable()
    {
        UnbindInputActions();
    }

    public void SetMinigameComplete()
    {
        InputManager.Instance.SetMovementMode();
        gameObject.SetActive(false);

        if(_sceneObjects != null)
            _sceneObjects.SetActive(true);
    }

    // Binds input events to the input handler
    private void BindInputActions()
    {
        _inputHandler = InputManager.Instance.minigameInput;
        _inputHandler.OnHold += HandleHold;
        _inputHandler.OnCancelHold += CancelHold;
        _inputHandler.OnTap += HandleTap;
    }

    private void UnbindInputActions()
    {
        _inputHandler.OnHold -= HandleHold;
        _inputHandler.OnCancelHold -= CancelHold;
        _inputHandler.OnTap -= HandleTap;
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

    private IEnumerator WaitAndBind()
    {
        while(InputManager.Instance == null)
        {
            yield return null;
        }

        BindInputActions();
    }
}
