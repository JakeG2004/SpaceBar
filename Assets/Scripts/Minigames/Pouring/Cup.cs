using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private Transform filling;
    [SerializeField] private SpriteRenderer fillSprite;
    [SerializeField] private int maxFilling = 100;
    private int fillAmount;
    
    void Awake()
    {
        fillSprite.color = Color.black;
    }

    public void Fill(DrinkColor color)
    {
        Color liquidColor;
        switch (color)
        {
            case DrinkColor.RED:
                liquidColor = Color.red;
                break;
            case DrinkColor.GREEN:
                liquidColor = Color.green;
                break;
            default:
                liquidColor = Color.blue;
                break;
        }
        
        fillSprite.color = fillSprite.color * ((float) fillAmount / (fillAmount + 1f))
                           + liquidColor / (fillAmount + 1f);
        Debug.Log("Denominator color: " + liquidColor / (fillAmount + 1f));
                           
        fillAmount++;
        
        filling.localScale = new(filling.localScale.x,
                                 (float) fillAmount / maxFilling,
                                 filling.localScale.z);
    }
    
    /*public void Fill(float amount, DrinkColor color)
    {
        Color liquidColor;
        switch (color)
        {
            case DrinkColor.RED:
                liquidColor = Color.red;
                break;
            case DrinkColor.GREEN:
                liquidColor = Color.green;
                break;
            default:
                liquidColor = Color.blue;
                break;
        }
        
    
        fillSprite.color = fillSprite.color * (fillAmount / fillAmount + amount)
                           + liquidColor * (amount / fillAmount + amount);
        
        fillAmount += amount;
        
        filling.localScale = new(filling.localScale.x,
                                 fillAmount / maxFilling,
                                 filling.localScale.z);
    }*/
}
