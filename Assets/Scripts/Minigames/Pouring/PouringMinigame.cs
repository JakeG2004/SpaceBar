using UnityEngine;
using UnityEngine.Events;

public class PouringMinigame : MonoBehaviour
{
    [SerializeField] private UnityEvent _onComplete;
    [SerializeField] private Bottle[] bottles;
    [SerializeField] private Cup cup;
    private int currentBottleIndex = 0;
     
    void Start()
    {
        bottles[currentBottleIndex].Highlight();
    }
    
    public void FillCup()
    {
        cup.Fill(bottles[currentBottleIndex].color);
        Debug.Log("current bottle: " + currentBottleIndex);
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
}
