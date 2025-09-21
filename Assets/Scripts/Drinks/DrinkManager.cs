using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrinkManager : MonoBehaviour
{
    public static DrinkManager Instance { get; private set; }
    public List<Drink> drinks = new();
    private Drink _currentDrink = new();
    private Drink _targetDrink = new();

    public int cleanCups = 10;
    public int dirtyCups = 0;

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

    public void SetTopping(DrinkTopping topping)
    {
        _currentDrink.topping = topping;
    }

    public void SetDrinkBase(DrinkColor baseColor)
    {
        if(_currentDrink.drink1 == DrinkColor.NONE)
        {
            _currentDrink.drink1 = baseColor;
            return;
        }
        else if(_currentDrink.drink2 == DrinkColor.NONE)
        {
            _currentDrink.drink2 = baseColor;
            return;
        }
    }

    public void SetTargetDrink(Drink drink)
    {
        _targetDrink = drink;
    }

    public bool CurrentDrinkIsValid()
    {
        if(_targetDrink.drink1 != _currentDrink.drink1)
        {
            return false;
        }

        if(_targetDrink.drink2 != _currentDrink.drink2)
        {
            return false;
        }

        if(_targetDrink.topping != _currentDrink.topping)
        {
            return false;
        }

        return true;
    }

    public void ServeCustomer()
    {
        cleanCups--;
    }

    public void WashedCups()
    {
        dirtyCups--;
        cleanCups++;
    }
}