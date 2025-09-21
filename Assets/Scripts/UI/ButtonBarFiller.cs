using UnityEngine;
using UnityEngine.UI;

public class ButtonBarFiller : MonoBehaviour
{
    [SerializeField] private Transform _maskTransform;
    private float _fillAmt = 0f;
    private bool _filling = false;

    public void StartFill()
    {
        _filling = true;
    }

    public void StopFill()
    {
        _filling = false;
    }

    void Update()
    {
        if(_filling)
        {
            _fillAmt += Time.deltaTime;
        }
        else
        {
            _fillAmt -= Time.deltaTime;
        }

        _fillAmt = Mathf.Clamp01(_fillAmt);

        Vector3 newScale = _maskTransform.localScale;
        newScale.x = _fillAmt;
        _maskTransform.localScale = newScale;

        if(_fillAmt >= 0.99)
        {
            GetComponent<Button>().onClick?.Invoke();
        }
    }
}
