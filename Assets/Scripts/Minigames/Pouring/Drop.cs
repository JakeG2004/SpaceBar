using UnityEngine;

public class Drop : MonoBehaviour
{
    private float timer;
    private float despawnTime = 5;
    private bool hasBeenDisabled;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= despawnTime)
        {
            Destroy(gameObject);
        }
    }
    
    void OnDisable()
    {
        hasBeenDisabled = true;
    }
    
    void OnEnable()
    {
        if (hasBeenDisabled)
        {
            Destroy(gameObject);
        }
    }
}
