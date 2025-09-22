using UnityEngine;

public class CupFilling : MonoBehaviour
{
    [SerializeField] private ColorEventChannelSO fillingCollision;
    private bool isFull;
    
    void OnEnable()
    {
        isFull = false;
        Debug.Log("Cup not full");
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFull && collision.gameObject.name.Contains("Drop"))
        {
            fillingCollision.RaiseEvent(collision.gameObject.GetComponent<SpriteRenderer>().color);
            Destroy(collision.gameObject);            
        }
    }
    
    public void StopFilling()
    {
        isFull = true;
        Debug.Log("Cup full");
    }
}
