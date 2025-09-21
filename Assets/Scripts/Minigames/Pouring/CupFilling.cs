using UnityEngine;

public class CupFilling : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO fillingCollision;
    private bool isFull;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFull && collision.gameObject.name.Contains("Drop"))
        {
            fillingCollision.RaiseEvent();
            Destroy(collision.gameObject);            
        }
    }
    
    public void StopFilling()
    {
        isFull = true;
    }
}
