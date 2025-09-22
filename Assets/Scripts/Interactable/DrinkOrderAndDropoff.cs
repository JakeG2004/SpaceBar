using UnityEngine;

public class DrinkOrderAndDropoff : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO orderDrink;
    [SerializeField] private VoidEventChannelSO serveDrink;
    private Interactable interactable;
    
    void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    public void HandleInteraction()
    {
        if(DrinkManager.Instance._hasOrder && DrinkManager.Instance.CurrentDrinkIsValid())
        {
            serveDrink.RaiseEvent();
        }

        else if (!DrinkManager.Instance._hasOrder)
        {
            orderDrink.RaiseEvent();
        }

        else
        {
            return;
        }
    }
}
