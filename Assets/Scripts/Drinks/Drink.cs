using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drink
{
    public DrinkColor drink1;
    public DrinkColor drink2;
    public DrinkTopping topping;

    public string drinkName;

    public Drink()
    {
        drink1 = DrinkColor.RED;
        drink2 = DrinkColor.RED;

        topping = DrinkTopping.PEPPER;
        drinkName = "Test Drink";
    }

    // Generates a list of num unique drinks. Max with current options is 24
    static public List<Drink> GenerateRandomDrinks(int numDrinks)
    {
        List<Drink> drinks = new();
        List<string> seenNames = new();

        for(int i = 0; i < numDrinks; i++)
        {
            Drink newDrink = new();
            bool isNewDrink;
            int numLoops = 0;

            // Ensure each drink recipe is unique
            do
            {
                numLoops++;
                isNewDrink = true;
                // 50% chance to make a drink with 2 pours
                if(Random.Range(0.1f, 1f) > 0.5f)
                {
                    newDrink.drink1 = (DrinkColor)Random.Range(0, 3);
                    newDrink.drink2 = (DrinkColor)Random.Range(0, 3);
                }

                // 1 pour drink
                else
                {
                    newDrink.drink1 = (DrinkColor)Random.Range(0, 3);
                    newDrink.drink2 = newDrink.drink1;
                }

                // Determine the topping
                newDrink.topping = (DrinkTopping)Random.Range(0, 5);

                // Check for drinks with the same composition and reject
                foreach (Drink drink in drinks)
                {
                    bool sameCombo =
                        (drink.drink1 == newDrink.drink1 && drink.drink2 == newDrink.drink2) ||
                        (drink.drink1 == newDrink.drink2 && drink.drink2 == newDrink.drink1);

                    bool sameTopping = drink.topping == newDrink.topping;

                    if (sameCombo && sameTopping)
                    {
                        isNewDrink = false;
                        break;
                    }
                }
            } while(!isNewDrink);

            // Ensure each drink name is unique
            do
            {
                // Determine the name
                string[] adjectives = {"Wet", "Slimy", "Zorp", "Nebula", "Thraxan", "Gleeby"};
                string[] nouns = {"Juice", "Elixr", "Hypergoo", "Stardust", "Nectar"};

                newDrink.drinkName = adjectives[Random.Range(0, adjectives.Length)] + " " + nouns[Random.Range(0, nouns.Length)];

            } while(seenNames.Contains(newDrink.drinkName));

            seenNames.Add(newDrink.drinkName);
            drinks.Add(newDrink);
        }

        return drinks;
    }
}

public enum DrinkColor
{
    RED,
    GREEN,
    BLUE,
    NONE
};

public enum DrinkTopping
{
    PEPPER,
    OLIVE,
    LEMON,
    UMBRELLA,
    NONE
};
