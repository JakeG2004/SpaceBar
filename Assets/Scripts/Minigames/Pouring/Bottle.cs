using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private Color color;
    [SerializeField] GameObject drop;
    [SerializeField] float dropAngleVariance;
    [SerializeField] float pressure;
    [SerializeField] private float tiltSpeed = 5; // degrees/s
    [SerializeField] private float pourSpeed = 1;
    private bool isPouring;
    public bool IsPouring => isPouring;
    private Coroutine startPour;
    private Coroutine stopPour;
    
    [SerializeField] private float transitionTime = 1;
    
    [SerializeField] private float highlightedScale = 1.5f;
    private float normalScale;
    private float targetScale;
    private float floatVelocity;
    
    public Vector3 highlightedPosition;
    private Vector3 normalPosition;
    private Vector3 targetPosition;
    private Vector3 vectorVelocity;
    
    private Quaternion normalRotation;

    void Awake()
    {
        color = GetComponent<SpriteRenderer>().color;
        
        normalScale = transform.localScale.x;
        normalPosition = transform.localPosition;
        normalRotation = transform.rotation;
        
        targetScale = normalScale;
        targetPosition = normalPosition;
    }
    
    void OnEnable()
    {
        transform.rotation = normalRotation;
        StartCoroutine(DropSpawner());
    }

    void Update()
    {
        float scale = Mathf.SmoothDamp(transform.localScale.x, targetScale,
                                       ref floatVelocity, transitionTime);
        transform.localScale = Vector3.one * scale;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition,
                                                     ref vectorVelocity, transitionTime);
    }

    public void Highlight()
    {
        targetScale = highlightedScale;
        targetPosition = highlightedPosition;
    }
    
    public void Unhighlight()
    {
        targetScale = normalScale;
        targetPosition = normalPosition;
    }

    public void StartPour()
    {
        if (stopPour is not null)
        {
            StopCoroutine(stopPour);
        }
        startPour = StartCoroutine(Pour());
    }
    
    public void StopPour()
    {
        if (startPour is not null)
        {
            StopCoroutine(startPour);
        }
        stopPour = StartCoroutine(ReturnToUpright());
    }

    private void SpawnDrop()
    {
        // Instantiate drop
        GameObject newDrop = Instantiate(drop, tip.position, Quaternion.identity, transform.parent);
        
        // Change color
        newDrop.GetComponent<SpriteRenderer>().color = color * Random.Range(0.75f, 1.25f);
        
        // Vary size
        newDrop.transform.localScale = newDrop.transform.localScale * Random.Range(0.75f, 1.25f);
        
        // Base direction (forward from spawner)
        Vector3 direction = transform.up;

        // Apply random rotation within angleVariance
        float angleX = Random.Range(-dropAngleVariance, dropAngleVariance);
        float angleY = Random.Range(-dropAngleVariance, dropAngleVariance);
        float angleZ = Random.Range(-dropAngleVariance, dropAngleVariance);

        Quaternion randomRot = Quaternion.Euler(angleX, angleY, angleZ);
        Vector3 finalDirection = randomRot * direction;

        // Apply force
        Rigidbody2D rb = newDrop.GetComponent<Rigidbody2D>();
        rb.AddForce(finalDirection.normalized * pressure, ForceMode2D.Impulse);
    }

    private IEnumerator DropSpawner()
    {
        while (true)
        {
            while (isPouring)
            {
                float dropsPerSecond = (transform.eulerAngles.z - 30) * pourSpeed;
                if (dropsPerSecond < 2 || transform.eulerAngles.z > 135)
                {
                    break;
                }
                
                SpawnDrop();
                
                while (dropsPerSecond > 30)
                {
                    SpawnDrop();
                    dropsPerSecond /= 2;
                }
                
                yield return new WaitForSeconds(1 / dropsPerSecond);
            }
            yield return null;
        }
    }    
    
    private IEnumerator MoveBottle()
    {
        yield return null;
    }

    private IEnumerator Pour()
    {
        isPouring = true;
        
        while (true)
        {
            float zAngle = Mathf.Clamp(transform.eulerAngles.z +
                                       tiltSpeed * Time.deltaTime, 0, 135);
            
            transform.eulerAngles = new(transform.eulerAngles.x,
                                        transform.eulerAngles.y,
                                        zAngle);
            yield return null;
        }
    }

    private IEnumerator ReturnToUpright()
    {
        float startAngle = transform.eulerAngles.z;
    
        while (transform.eulerAngles.z < 180 && transform.eulerAngles.z != 0)
        {
            transform.eulerAngles += Vector3.back * tiltSpeed * Time.deltaTime;
            
            yield return null;
        }
        
        transform.eulerAngles = new(transform.eulerAngles.x,
                                    transform.eulerAngles.y, 0);
        isPouring = false;
    }
}
