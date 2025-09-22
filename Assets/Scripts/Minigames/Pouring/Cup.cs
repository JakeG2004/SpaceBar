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

    // Track milestones per color
    private bool red40, red80;
    private bool green40, green80;
    private bool blue40, blue80;

    void OnEnable()
    {
        fillSprite.color = Color.black;
        fillAmount = 0;
        filling.localScale = new(filling.localScale.x, 0.001f, filling.localScale.z);

        redFill = greenFill = blueFill = 0;
        red40 = red80 = green40 = green80 = blue40 = blue80 = false;
    }

    public void Fill(Color color)
    {
        // Detect dominant color and increment
        if (color.r > color.g && color.r > color.b)
        {
            redFill++;
            CheckMilestones(DrinkColor.RED, ref redFill, ref red40, ref red80);
        }
        else if (color.g > color.r && color.g > color.b)
        {
            greenFill++;
            CheckMilestones(DrinkColor.GREEN, ref greenFill, ref green40, ref green80);
        }
        else
        {
            blueFill++;
            CheckMilestones(DrinkColor.BLUE, ref blueFill, ref blue40, ref blue80);
        }

        // Average color fill
        fillSprite.color = fillSprite.color * ((float) fillAmount / (fillAmount + 1f))
                           + color / (fillAmount + 1f);

        fillAmount++;
        if (fillAmount >= maxFilling)
        {
            DrinkManager.Instance.SaveDrinkColor(fillSprite.color);
            cupFull.RaiseEvent();
        }

        filling.localScale = new(filling.localScale.x,
                                 (float) fillAmount / maxFilling,
                                 filling.localScale.z);
    }

    private void CheckMilestones(DrinkColor color, ref int fill, ref bool hit40, ref bool hit80)
    {
        float pct = (float)fill / maxFilling;
        if (!hit40 && pct >= 0.4f)
        {
            hit40 = true;
            sufficientColor.RaiseEvent(color);
        }
        else if (!hit80 && pct >= 0.8f)
        {
            hit80 = true;
            sufficientColor.RaiseEvent(color);
        }
    }
}
