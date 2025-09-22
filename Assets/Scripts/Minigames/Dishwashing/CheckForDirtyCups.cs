using UnityEngine;
using UnityEngine.Events;

public class CheckForDirtyCups : MonoBehaviour
{
    [SerializeField] private UnityEvent _onSuccess;

    public void TryCups()
    {
        if(DrinkManager.Instance.dirtyCups <= 0)
        {
            return;
        }

        _onSuccess?.Invoke();
    }
}
