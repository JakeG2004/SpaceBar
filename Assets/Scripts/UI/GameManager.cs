using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int DrinksServed { get; private set; }
    private float _gameTimer = 60f * 4f;
    public VoidEventChannelSO TimerExpiredEvent;

    void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        /*var timer = _gameTimer;
        _gameTimer = Math.Max(_gameTimer - Time.deltaTime, 0f);
        if (_gameTimer == 0f && timer != _gameTimer)
        {
            TimerExpiredEvent.RaiseEvent();
        }*/
    }

    public float GetGameTimer() => _gameTimer;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void TimeUp()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
