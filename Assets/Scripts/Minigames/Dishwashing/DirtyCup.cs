using UnityEngine;
using UnityEngine.Events;

public class DirtyCup : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _targetCleanliness = 10;
    [SerializeField] private Vector2 _bubbleRange = new Vector2(1, 3);
    [SerializeField] private GameObject _bubble;
    [SerializeField] private Transform _bubbleSpawnPoint;

    [Header("Colors")]
    [SerializeField] private Color _dirtyColor;
    [SerializeField] private Color _cleanColor;

    [SerializeField] private VoidEventChannelSO _cupCleanEvent;
    [SerializeField] private UnityEvent _onClean;

    private DishwashingMinigame _minigame;

    private bool _isClean = false;
    private int _cleanliness = 0;

    private SpriteRenderer _renderer;

    void Start()
    {
        if(_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        _renderer.color = _dirtyColor;
    }

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x -= (_speed * Time.deltaTime);
        if(newPos.x < -12)
        {
            newPos.x = 12;

            if(_isClean)
            {
                _cupCleanEvent.RaiseEvent();
                if(_minigame.numDirtyCups >= 0)
                {
                    _isClean = false;
                    _renderer.color = _dirtyColor;
                }

                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
        transform.position = newPos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Water"))
        {
            Destroy(other.gameObject);
            SpawnBubbles();
            _cleanliness++;

            // Change cup color to be more clear
            _renderer.color = Color.Lerp(_dirtyColor, _cleanColor, (float)_cleanliness / _targetCleanliness);

            if(_cleanliness >= _targetCleanliness)
            {
                _onClean?.Invoke();
                _isClean = true;
            }
        }
    }

    public void SetMinigame(DishwashingMinigame dish)
    {
        _minigame = dish;
    }

    private void SpawnBubbles()
    {
        int numBubbles = Random.Range((int)_bubbleRange.x, (int)_bubbleRange.y + 1);
        
        for(int i = 0; i < numBubbles; i++)
        {
            Vector3 tmpSpawn = _bubbleSpawnPoint.position;
            tmpSpawn.x += Random.Range(-2.0f, 2.0f);
            tmpSpawn.y += Random.Range(-2.0f, 2.0f);

            GameObject bubble = Object.Instantiate(
                _bubble,
                tmpSpawn,
                Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
                transform // keep parent
            );

            // Convert local scale so that global scale = 1
            Vector3 parentScale = transform.lossyScale;
            bubble.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );
        }
    }
}
