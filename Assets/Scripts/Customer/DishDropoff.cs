using UnityEngine;

public class DishDropoff : MonoBehaviour
{
    public static Vector3 position;
    
    void Awake()
    {
        position = transform.position + Vector3.right * 1.2f;
    }
}
