using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private float speed;
    private float timeToLeave;
    private float timer = 0;
    
    public Vector3 targetPosition;
    
    void Awake()
    {
        targetPosition = Vector3.zero;
        
        timeToLeave = Random.Range(10f, 20f);
    }

    void Update()
    {
        Vector3 maxMovement = targetPosition - transform.localPosition;
        Vector3 moveDirection = maxMovement.normalized;
        Vector3 movement = Time.deltaTime * speed * moveDirection;
        
        // Prevents jittering when near targetPosition.
        if (movement.sqrMagnitude > maxMovement.sqrMagnitude)
        {
            transform.localPosition += maxMovement;
        }
        else
        {
            transform.localPosition += movement;
        }
                                            
        timer += Time.deltaTime;
        if (timer >= timeToLeave)
        {
            Leave();
        }
    }

    public void Leave()
    {
        targetPosition = targetPosition + Vector3.down * 10;
        transform.parent.GetComponent<QueueManager>().CustomerLeave(gameObject);
        Debug.Log("Leaving");
    }
}
