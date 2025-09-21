using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private List<Selectable> _buttons;
    private int _curSelectionIdx = 0;
    private Selectable _curSelection;

    private bool _isActive = false;

    public void TogglePause()
    {
        _isActive = !_isActive;
        _menu.SetActive(_isActive);

        if(_isActive)
        {
            _curSelectionIdx = 0;
            _curSelection = _buttons[_curSelectionIdx];
            _curSelection.Select();
            InputManager.Instance.SetMinigameMode();
        }
        else
        {
            InputManager.Instance.SetMovementMode();
        }
    }

    public void OnTap()
    {
        _curSelectionIdx = (_curSelectionIdx + 1) % _buttons.Count;
        _curSelection = _buttons[_curSelectionIdx];
        _curSelection.Select();
    }

    public void OnHold()
    {
        _curSelection.GetComponent<ButtonBarFiller>().StartFill();
    }

    public void OnHoldRelease()
    {
        _curSelection.GetComponent<ButtonBarFiller>().StopFill();
    }
}
