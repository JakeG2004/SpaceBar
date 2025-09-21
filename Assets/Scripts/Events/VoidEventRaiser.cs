using UnityEngine;

public class VoidEventRaiser : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _eventSO;

    public void RaiseEvent()
    {
        _eventSO.RaiseEvent();
    }
}
