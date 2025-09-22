using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Renderer variables")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _highlightColor = Color.cyan;

    [Header("Interaction Variables")]
    [SerializeField] private GameObject _interactionGame;
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] public VoidEventChannelSO eventToRaise;

    private Color _originalColor;

    void Start()
    {
        if(_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        _originalColor = _renderer.color;
    }

    public void Interact()
    {
        if (_interactionGame != null)
        {
            _interactionGame.SetActive(true);
            InputManager.Instance.SetMinigameMode();
        }
        else
        {
            _onInteract?.Invoke();
            //eventToRaise.RaiseEvent();
        }
    }

    // Sets the highlight color
    public void SetHighlight(bool state)
    {
        if(state)
        {
            _renderer.color = _highlightColor;
        }

        else
        {
            _renderer.color = _originalColor;
        }
    }
}
