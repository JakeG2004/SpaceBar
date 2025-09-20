using UnityEngine;
using UnityEngine.Events;

public class TorchMinigame : MonoBehaviour
{
    [SerializeField] private UnityEvent _onComplete;

    [SerializeField] private int _requiredTaps = 15;
    [SerializeField] private int _curTaps = 0;

    void OnEnable()
    {
        _requiredTaps = 15 + Random.Range(-3, 3);
        _curTaps = 0;
    }

    public void OnTap()
    {
        _curTaps++;

        if(_curTaps >= _requiredTaps)
        {
            FinishMinigame();
        }
    }

    private void FinishMinigame()
    {
        _onComplete?.Invoke();
    }
}
