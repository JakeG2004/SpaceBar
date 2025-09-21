using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PouringMinigame : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO minigameComplete;
    [SerializeField] private ParticleSystem winParticles;
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
        StartCoroutine(PlayParticles());
    }
    
    private IEnumerator PlayParticles()
    {
        winParticles.Play();
        
        while (winParticles.isPlaying)
        {
            yield return null;
        }
        
        minigameComplete.RaiseEvent();
    }
}
