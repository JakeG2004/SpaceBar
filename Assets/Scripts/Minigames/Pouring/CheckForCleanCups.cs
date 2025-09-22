using UnityEngine;
using UnityEngine.Events;

public class CheckForCups : MonoBehaviour
{
    [SerializeField] private UnityEvent _onSuccess;

    public void TryCups()
    {
        if(DrinkManager.Instance.cleanCups <= 0)
        {
            return;
        }

        _onSuccess?.Invoke();
    }
}
