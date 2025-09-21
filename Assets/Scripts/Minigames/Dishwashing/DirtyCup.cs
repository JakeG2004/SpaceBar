using UnityEngine;

public class DirtyCup : MonoBehaviour
{
    [SerializeField] private int _targetCleanliness = 10;
    [SerializeField] private Vector2 _bubbleRange = new Vector2(1, 3);
    [SerializeField] private GameObject _bubble;
    [SerializeField] private Transform _bubbleSpawnPoint;

    private bool _isClean = false;
    private int _cleanliness = 0;

    private SpriteRenderer _renderer;

    void Start()
    {
        if(_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Test");
        if(other.CompareTag("Water"))
        {
            SpawnBubbles();
            _cleanliness++;

            // Change cup color to be more clear
            _renderer.color = Color.Lerp(_renderer.color, Color.white, (float)_cleanliness / _targetCleanliness);

            if(_cleanliness >= _targetCleanliness)
            {
                _isClean = true;
            }
        }
    }

    private void SpawnBubbles()
    {
        int numBubbles = Random.Range((int)_bubbleRange.x, (int)_bubbleRange.y + 1);
        
        for(int i = 0; i < numBubbles; i++)
        {
            Vector3 tmpSpawn = _bubbleSpawnPoint.position;
            tmpSpawn.x += Random.Range(-2.0f, 2.0f);
            tmpSpawn.y += Random.Range(-2.0f, 2.0f);

            Object.Instantiate(_bubble, tmpSpawn, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
    }

    private void ResetPos()
    {
        if(_isClean)
        {
            Destroy(this.gameObject);
        }
    }
}
