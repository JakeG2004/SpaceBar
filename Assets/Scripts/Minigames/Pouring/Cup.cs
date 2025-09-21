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
    
    void OnEnable()
    {
        fillSprite.color = Color.black;
        fillAmount = 0;
        filling.localScale = new(filling.localScale.x, 0.001f, filling.localScale.z);

        redFill = 0;
        greenFill = 0;
        blueFill = 0;
    }

    public void Fill(Color color)
    {
        if (color.r > color.g && color.r > color.b)
        {
            redFill++;
            if (redFill >= maxFilling * 0.4f)
            {
                sufficientColor.RaiseEvent(DrinkColor.RED);
            }
        }
        else if (color.g > color.r && color.g > color.b)
        {
            greenFill++;
            if (greenFill >= maxFilling * 0.4f)
            {
                sufficientColor.RaiseEvent(DrinkColor.GREEN);
            }
        }
        else
        {
            blueFill++;
            if (blueFill >= maxFilling * 0.4f)
            {
                sufficientColor.RaiseEvent(DrinkColor.BLUE);
            }
        }    
        
        fillSprite.color = fillSprite.color * ((float) fillAmount / (fillAmount + 1f))
                           + color / (fillAmount + 1f);
                           
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
