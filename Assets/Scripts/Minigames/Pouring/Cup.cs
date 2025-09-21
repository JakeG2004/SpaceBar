using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO cupFull;
    [SerializeField] private DrinkColorEventChannelSO sufficientColor;
    [SerializeField] private Transform filling;
    [SerializeField] private SpriteRenderer fillSprite;
    [SerializeField] private int maxFilling = 100;
    private int fillAmount;
    
    private int redFill;
    private int greenFill;
    private int blueFill;
    
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
                redFill++;
                if (redFill >= maxFilling * 0.4f)
                {
                    sufficientColor.RaiseEvent(color);
                }
                break;
            case DrinkColor.GREEN:
                liquidColor = Color.green;
                greenFill++;
                if (greenFill >= maxFilling * 0.4f)
                {
                    sufficientColor.RaiseEvent(color);
                }
                break;
            default:
                liquidColor = Color.blue;
                blueFill++;
                if (blueFill >= maxFilling * 0.4f)
                {
                    sufficientColor.RaiseEvent(color);
                }
                break;
        }
        
        
        
        fillSprite.color = fillSprite.color * ((float) fillAmount / (fillAmount + 1f))
                           + liquidColor / (fillAmount + 1f);
        Debug.Log("Denominator color: " + liquidColor / (fillAmount + 1f));
                           
        fillAmount++;
        
        if (fillAmount >= 100)
        {
            cupFull.RaiseEvent();
        }
        
        filling.localScale = new(filling.localScale.x,
                                 (float) fillAmount / maxFilling,
                                 filling.localScale.z);
    }
}
