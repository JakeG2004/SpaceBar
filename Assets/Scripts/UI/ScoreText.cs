using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    void Start()
    {
        string score = "Score: " + DrinkManager.Instance.customersServed; 
        _scoreText.text = score;
        DrinkManager.Instance.customersServed = 0;
    }
}
