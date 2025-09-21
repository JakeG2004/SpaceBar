using UnityEngine;
using UnityEngine.Events;

public class PouringMinigame : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO minigameComplete;
    [SerializeField] private Bottle[] bottles;
    [SerializeField] private Cup cup;
    private int currentBottleIndex = 0;
      
    void Start()
    {
        bottles[currentBottleIndex].Highlight();
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
        // Insert feedback so user knows if it's good or not.    
        
        minigameComplete.RaiseEvent();
    }
}
