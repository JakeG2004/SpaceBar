using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private TextMeshProUGUI label;

    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        var timer = GameManager.Instance.GetGameTimer();
        label.text = $"{(int)Math.Floor(timer / 60f)}:{(int)Math.Floor(timer) % 60:D2}";
    }
}
