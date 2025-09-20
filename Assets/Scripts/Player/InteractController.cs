using UnityEngine;

public class InteractController : MonoBehaviour
{
    public Interactable _curInteractable = null;
    public void TryInteract()
    {
        if(_curInteractable == null)
        {
            return;
        }

        _curInteractable.Interact();
    }

    // Upon colliding with something else, add its interactable to the stack
    void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if(interactable == null)
        {
            return;
        }

        _curInteractable = interactable;
    }

    // Remove interactable if its the current one
    void OnTriggerExit2D(Collider2D other)
    {
        if(_curInteractable == other.gameObject.GetComponent<Interactable>())
        {
            _curInteractable = null;
        }
    }
}
