using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Renderer variables")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _highlightColor = Color.cyan;

    [Header("Interaction Variables")]
    [SerializeField] private GameObject _interactionGame;

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
        _interactionGame.SetActive(true);
        InputManager.Instance.SetMinigameMode();
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
