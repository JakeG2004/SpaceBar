using UnityEngine;

public class FallingWater : MonoBehaviour
{
    [SerializeField] private float _despawnTime = 5f;
    [SerializeField] private Vector2 _initialXForceRange;
    [SerializeField] private Vector2 _initialYForceRange;

    private float _elapsedTime = 0f;

    void Start()
    {
        float xForce = Random.Range(_initialXForceRange.x, _initialXForceRange.y);
        float yForce = Random.Range(_initialYForceRange.x, _initialYForceRange.y);

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xForce, yForce);
        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color * Random.Range(0.75f, 1.25f);
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= _despawnTime)
        {
            Destroy(this.gameObject);
        }
    }
}
