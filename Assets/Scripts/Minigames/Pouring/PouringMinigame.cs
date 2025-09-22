using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PouringMinigame : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO minigameComplete;
    [SerializeField] private VoidEventChannelSO minigameStarted;
    [SerializeField] private VoidEventChannelSO redFlash;
    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private Bottle[] bottles;
    [SerializeField] private Cup cup;
    private DrinkColor drinkColor1 = DrinkColor.NONE;
    private DrinkColor drinkColor2 = DrinkColor.NONE;
    private int currentBottleIndex = 0;
      
    void Start()
    {
        bottles[currentBottleIndex].Highlight();
    }

    void OnEnable()
    {
        minigameStarted.RaiseEvent();
    }
    
    public void FillCup(Color color)
    {
        cup.Fill(color);
    }
    
    public void CycleBottles()
    {
        if (!bottles[currentBottleIndex].IsPouring)
        {
            bottles[currentBottleIndex].Unhighlight();
            currentBottleIndex = (currentBottleIndex + 1) % bottles.Length;
            bottles[currentBottleIndex].Highlight();
        }
    }
    
    public void StartPour()
    {
        bottles[currentBottleIndex].StartPour();
    }
    
    public void StopPour()
    {
        bottles[currentBottleIndex].StopPour();
    }
    
    public void CupIsFull()
    {
        StartCoroutine(PlayParticles());
    }
    
    public void NewDrinkColor(DrinkColor color)
    {
        if (drinkColor1 == DrinkColor.NONE)
        {
            drinkColor1 = color;
        }
        else if (drinkColor2 == DrinkColor.NONE)
        {
            drinkColor2 = color;
        }
    }
    
    private bool IsDrinkCorrect()
    {
        Drink targetDrink = DrinkManager.Instance.GetTargetDrink();
        if ((drinkColor1 == targetDrink.drink1 && drinkColor2 == targetDrink.drink2)
         || (drinkColor1 == targetDrink.drink2 && drinkColor2 == targetDrink.drink1))
        {
            return true;
        }
        return false;
    }
    
    private IEnumerator PlayParticles()
    {
        if (IsDrinkCorrect())
        {
            winParticles.Play();
            
            while (winParticles.isPlaying)
            {
                yield return null;
            }
            
            minigameComplete.RaiseEvent();
        }
        else
        {
            redFlash.RaiseEvent();
        
            drinkColor1 = DrinkColor.NONE;
            drinkColor2 = DrinkColor.NONE;
            
            minigameComplete.RaiseEvent();
        }
    }
}
