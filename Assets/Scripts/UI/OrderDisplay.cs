using System;
using TMPro;
using UnityEngine;

public class OrderDisplay : MonoBehaviour
{
    public GameObject Toppings;
    public GameObject FirstColor;
    public GameObject SecondColor;
    public GameObject DrinkName;
    public Sprite Lemon;
    public Sprite Pepper;
    public Sprite Umbrella;
    public Sprite Olive;

    private Vector3 initialPos;
    private Vector3 hiddenPos;

    void Start()
    {
        initialPos = transform.position;
        hiddenPos = initialPos + new Vector3(0.0f, 5.0f, 0.0f);
    }

    void Update()
    {
        // if (Input.GetKeyDown("r"))
        // {
        //     ShowDrink(DrinkManager.Instance.GetRandomDrink());
        // }

        // animates order position
        transform.position = new Vector3(
            transform.position.x,
            Math.Max(transform.position.y - 10f * Time.deltaTime, initialPos.y),
            transform.position.z
        );
    }

    public void OnDrinkOrdered()
    {
        ShowDrink(DrinkManager.Instance.GetTargetDrink());
    }

    /// Displays the drink order on screen.
    public void ShowDrink(Drink drink)
    {
        // animates order position
        transform.position = hiddenPos;

        // order items
        DrinkName.GetComponent<TextMeshProUGUI>().text = drink.drinkName;

        Toppings.GetComponent<SpriteRenderer>().sprite = ToppingTexture(drink.topping);
        Toppings.SetActive(drink.topping != DrinkTopping.NONE);

        FirstColor.GetComponent<SpriteRenderer>().color = DrinkColorToColor(drink.drink1);
        SecondColor.GetComponent<SpriteRenderer>().color = DrinkColorToColor(drink.drink2);
        SecondColor.SetActive(drink.drink2 != DrinkColor.NONE && drink.drink1 != drink.drink2);
    }

    private Color ToppingToColor(DrinkTopping topping)
    {
        switch (topping)
        {
            case DrinkTopping.LEMON: return Color.lemonChiffon;
            case DrinkTopping.PEPPER: return Color.gray5;
            case DrinkTopping.OLIVE: return Color.olive;
            case DrinkTopping.UMBRELLA: return Color.red;
        }
        return Color.black;
    }

    private Color DrinkColorToColor(DrinkColor color)
    {
        switch (color)
        {
            case DrinkColor.BLUE: return Color.blue;
            case DrinkColor.GREEN: return Color.green;
            case DrinkColor.RED: return Color.red;
        }
        return Color.black;
    }

    private Sprite ToppingTexture(DrinkTopping topping)
    {
        switch (topping)
        {
            case DrinkTopping.LEMON: return Lemon;
            case DrinkTopping.PEPPER: return Pepper;
            case DrinkTopping.OLIVE: return Olive;
            case DrinkTopping.UMBRELLA: return Umbrella;
        }
        return Umbrella;
    }
}
