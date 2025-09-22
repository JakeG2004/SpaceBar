using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrinkManager : MonoBehaviour
{
    public static DrinkManager Instance { get; private set; }
    public List<Drink> drinks = new();
    private Drink _currentDrink = new();
    private Drink _targetDrink = new();
    public DrinkEventChannelSO _orderPlacedEvent;

    public int cleanCups = 10;
    public int dirtyCups = 0;

    private bool _canGetNewOrder = true;
    public bool _hasOrder = false;
    private Color _drinkColor = Color.white;

    void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        drinks = Drink.GenerateRandomDrinks(24);
    }

    public Drink GetRandomDrink()
    {
        return drinks[Random.Range(0, drinks.Count)];
    }

    public void SaveDrinkColor(Color color)
    {
        _drinkColor = color;
    }

    public Color GetDrinkColor()
    {
        return _drinkColor;
    }

    public DrinkTopping GetDrinkTopping()
    {
        return _currentDrink.topping;
    }

    public void CreateOrder()
    {
        if(!_canGetNewOrder)
        {
            return;
        }

        _hasOrder = true;
        _canGetNewOrder = false;

        SetTargetDrink(GetRandomDrink());
        _orderPlacedEvent.RaiseEvent(_targetDrink);
    }

    public void SetTopping(DrinkTopping topping)
    {
        _currentDrink.topping = topping;
    }

    public void SetDrinkBase(DrinkColor baseColor)
    {
        if(_currentDrink.drink1 == DrinkColor.NONE)
        {
            _currentDrink.drink1 = baseColor;
        }
        else if(_currentDrink.drink2 == DrinkColor.NONE)
        {
            _currentDrink.drink2 = baseColor;
        }

    }

    public void SetTargetDrink(Drink drink)
    {
        _targetDrink = drink;
    }

    public Drink GetTargetDrink() => _targetDrink;

    public bool CurrentDrinkIsValid()
    {
        bool isCorrectDrink = (((_targetDrink.drink1 == _currentDrink.drink1) && (_targetDrink.drink2 == _currentDrink.drink2)) ||
                                ((_targetDrink.drink1 == _currentDrink.drink2) && (_targetDrink.drink2 == _currentDrink.drink1)));
        if(!isCorrectDrink)
        {
            return false;
        }

        if (_targetDrink.topping != _currentDrink.topping)
        {
            return false;
        }

        return true;
    }

    public void ServeCustomer()
    {
        _canGetNewOrder = true;
        _hasOrder = false;

        cleanCups--;
        ResetDrink();
    }

    public void ResetDrink()
    {
        _currentDrink = new();
        _drinkColor = Color.white;
    }

    public void WashedCups()
    {
        dirtyCups--;
        cleanCups++;
    }

    public void CustomerReturnsDrink()
    {
        dirtyCups++;
    }
}