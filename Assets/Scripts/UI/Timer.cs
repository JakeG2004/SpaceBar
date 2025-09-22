using UnityEngine;
using UnityEngine.UI; // Needed if you want to display it on a UI Text element
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO timerDone;
    [SerializeField] private int startMinutes = 2;     // starting minutes
    private TMP_Text timerText;           // assign a UI Text in the Inspector

    private float remainingTime;

    void Start()
    {
        remainingTime = startMinutes * 60f; // convert minutes to seconds
        timerText = GetComponent<TMP_Text>();
        if (timerText == null)
        {
            Debug.Log("failed to get timer text");
        }
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0) remainingTime = 0;
        }
        else
        {
            timerDone.RaiseEvent();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        // Format as 00:00
        string timeString = string.Format("{0}:{1:00}", minutes, seconds);

        // Show on UI Text (if assigned)
        timerText.text = timeString;
    }
}
