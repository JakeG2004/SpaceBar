using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Customer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO returnCup;
    [SerializeField] private GameObjectEventChannelSO leave;
    [SerializeField] private float speed = 1;
    [SerializeField] private float avgLeaveTime = 15;
    [SerializeField] private float avgReturnCupTime = 10;
    [SerializeField] private float deviation = 5;

    [SerializeField] private GameObject _blueGuy;
    [SerializeField] private GameObject _purpleGuy;
    [SerializeField] private GameObject _greenGuy;
    
    private float leaveTime;
    private float returnCupTime;
    
    public Transform cupReturn;
    public Vector3 targetPosition;
    public bool hasBeenServed = false;
    
    void Awake()
    {
        targetPosition = Vector3.zero;
        
        leaveTime = Random.Range(avgLeaveTime - deviation,
                                   avgLeaveTime + deviation);
        returnCupTime = Random.Range(avgReturnCupTime - deviation,
                                     avgReturnCupTime + deviation);
                                     
        // StartCoroutine(TimedLeave());
    }

    void Start()
    {
        switch(Random.Range(0, 3))
        {
            case 0:
                _blueGuy.SetActive(true);
                break;
            case 1:
                _purpleGuy.SetActive(true);
                break;
            case 2:
                _greenGuy.SetActive(true);
                break;
            default:
                break;
        }
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
    }

    public void Leave()
    {
        targetPosition = targetPosition + Vector3.down * 10;
        leave.RaiseEvent(gameObject);
        
        if (hasBeenServed)
        {
            StartCoroutine(ReturnCup());
        }
    }
    
    private IEnumerator TimedLeave()
    {
        yield return new WaitForSeconds(leaveTime);
        
        if (!hasBeenServed)
        {
            Leave();
        }
    }
    
    private IEnumerator ReturnCup()
    {
        yield return new WaitForSeconds(returnCupTime);
        
        Vector3 oldPosition = transform.localPosition;
        
        targetPosition = transform.parent.InverseTransformPoint(DishDropoff.position);

        while ((transform.localPosition - targetPosition).sqrMagnitude > 0.01f)
        {
            yield return null;
        }
        
        returnCup.RaiseEvent();
        
        targetPosition = oldPosition;
    }
}
