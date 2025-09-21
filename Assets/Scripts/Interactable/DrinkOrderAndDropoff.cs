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
    
    public void DrinkOrdered()
    {
        interactable.eventToRaise = serveDrink;
    }
    
    public void DrinkServed()
    {
        interactable.eventToRaise = orderDrink;
    }
}
