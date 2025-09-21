using UnityEngine;
using UnityEngine.Events;

public class IntEventChannelSubscriber : MonoBehaviour
{
    [Tooltip("The signal to listen for.")]
    [SerializeField] private IntEventChannelSO eventChannel;
    [Space]
    [Tooltip("Response to receiving signal from Event Channel")]
    [SerializeField] private UnityEvent<int> response;

    public void OnEventRaised(int value)
    {
        response?.Invoke(value);
    }

    public void SetChannelAndResponse(IntEventChannelSO eventChannel, UnityEvent<int> response)
    {
        this.eventChannel = eventChannel;
        this.response = response;
    }

    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised += OnEventRaised;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised -= OnEventRaised;
        }
    }
}
