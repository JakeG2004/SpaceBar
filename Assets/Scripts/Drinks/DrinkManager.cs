using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrinkManager : MonoBehaviour
{
    public DrinkManager Instance { get; private set; }
    public List<Drink> drinks = new();

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
        drinks = Drink.GenerateRandomDrinks(10);
        foreach(Drink drink in drinks)
        {
            string debugString = drink.drinkName + "\n" + 
                                drink.drink1 + "\n" + 
                                drink.drink2 + "\n" + 
                                drink.topping; 
            Debug.Log(debugString);
        }
    }
}
