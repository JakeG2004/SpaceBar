using UnityEngine;
using UnityEngine.UI;

public class CupDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _umbrella;
    [SerializeField] private GameObject _olive;
    [SerializeField] private GameObject _pepper;
    [SerializeField] private GameObject _lemon;
    [SerializeField] private Image _drink;

    void Update()
    {
        SetTopping();
        SetDrinkColor();
    }

    private void SetDrinkColor()
    {
        _drink.color = DrinkManager.Instance.GetDrinkColor();
        _drink.gameObject.SetActive(_drink.color != Color.white);
    }

    private void SetTopping()
    {
        _umbrella.SetActive(false);
        _olive.SetActive(false);
        _pepper.SetActive(false);
        _lemon.SetActive(false);

        switch(DrinkManager.Instance.GetDrinkTopping())
        {
            case DrinkTopping.UMBRELLA:
                _umbrella.SetActive(true);
                break;
            case DrinkTopping.OLIVE:
                _olive.SetActive(true);
                break;
            case DrinkTopping.PEPPER:
                _pepper.SetActive(true);
                break;
            case DrinkTopping.LEMON:
                _lemon.SetActive(true);
                break;
        }
    }
}
