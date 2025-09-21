using UnityEngine;

public class CupFilling : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO fillingCollision;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Drop"))
        {
            fillingCollision.RaiseEvent();
            Destroy(collision.gameObject);            
        }
    }
}
