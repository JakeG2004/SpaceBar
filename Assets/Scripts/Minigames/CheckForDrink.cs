using UnityEngine;
using UnityEngine.Events;

public class CheckForDrink : MonoBehaviour
{
    [SerializeField] private UnityEvent _onSuccess;

    public void TryCups()
    {
        if(DrinkManager.Instance.GetDrinkColor() == Color.white)
        {
            return;
        }

        _onSuccess?.Invoke();
    }
}
