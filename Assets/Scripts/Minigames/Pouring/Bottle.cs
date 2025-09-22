using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private Color color;
    [SerializeField] GameObject drop;
    [SerializeField] float dropAngleVariance;
    [SerializeField] float pressure;
    [SerializeField] private float tiltSpeed = 90f; // degrees per second
    [SerializeField] private float pourSpeed = 1f;

    private bool isPouring;
    public bool IsPouring => isPouring;

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
    private Coroutine tiltRoutine;

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
        // Smooth scaling / positioning for highlight
        float scale = Mathf.SmoothDamp(transform.localScale.x, targetScale,
                                       ref floatVelocity, transitionTime);
        transform.localScale = Vector3.one * scale;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition,
                                                     ref vectorVelocity, transitionTime);

        // Auto-set pouring state based on angle
        float zAngle = NormalizeAngle(transform.eulerAngles.z);
        isPouring = zAngle > 30f && zAngle < 135f; 
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
        if (tiltRoutine != null) StopCoroutine(tiltRoutine);
        tiltRoutine = StartCoroutine(TiltBottle(135f)); // Tilt forward
    }
    
    public void StopPour()
    {
        if (tiltRoutine != null) StopCoroutine(tiltRoutine);
        tiltRoutine = StartCoroutine(TiltBottle(0f)); // Tilt back upright
    }

    private void SpawnDrop()
    {
        GameObject newDrop = Instantiate(drop, tip.position, Quaternion.identity, transform.parent);
        newDrop.GetComponent<SpriteRenderer>().color = color * Random.Range(0.75f, 1.25f);
        newDrop.transform.localScale *= Random.Range(0.75f, 1.25f);
        
        Vector3 direction = transform.up;
        Quaternion randomRot = Quaternion.Euler(
            Random.Range(-dropAngleVariance, dropAngleVariance),
            Random.Range(-dropAngleVariance, dropAngleVariance),
            Random.Range(-dropAngleVariance, dropAngleVariance));
        
        Vector3 finalDirection = randomRot * direction;
        newDrop.GetComponent<Rigidbody2D>().AddForce(finalDirection.normalized * pressure, ForceMode2D.Impulse);
    }

    private IEnumerator DropSpawner()
    {
        while (true)
        {
            while (isPouring)
            {
                float zAngle = NormalizeAngle(transform.eulerAngles.z);
                float pourFactor = Mathf.InverseLerp(30f, 135f, zAngle); // 0 at 30°, 1 at 135°
                float dropsPerSecond = Mathf.Max(2f, pourFactor * 30f * pourSpeed);

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

    private IEnumerator TiltBottle(float targetZ)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetZ)) > 0.1f)
        {
            float newZ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetZ, tiltSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZ);
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetZ);
        tiltRoutine = null;
    }

    /// <summary>
    /// Converts angle from 0-360 to -180..180 for easier reasoning
    /// </summary>
    private float NormalizeAngle(float zAngle)
    {
        return (zAngle > 180) ? zAngle - 360 : zAngle;
    }
}
