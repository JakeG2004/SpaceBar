using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ToppingMinigame : MonoBehaviour
{
    [SerializeField] private UnityEvent _onComplete;

    [Header("Tap variables")]
    [SerializeField] private int _requiredTaps = 15;
    [SerializeField] private int _curTaps = 0;

    [Header("Selection variables")]
    [SerializeField] private GameObject _curSelection;
    [SerializeField] private GameObject[] _selectables;
    [SerializeField] private GameObject _liquid;
    [SerializeField] private Transform _finalPos;
    [SerializeField] private Transform _initialPos;
    
    private int _curSelectionIdx;
    private Coroutine _decreaseTapsCoroutine;

    void OnEnable()
    {
        _curSelectionIdx = 0;
        _curSelection = _selectables[0];

        SetSelection();
        _requiredTaps = 30;
        _curTaps = 0;

        _decreaseTapsCoroutine = StartCoroutine(TapDecreaser());
        SetToppingPos();

        // if (DrinkManager.Instance.GetDrinkColor() == Color.white)
        // {
        //     _liquid.SetActive(false);
        // }
        // _liquid.GetComponent<SpriteRenderer>().color = DrinkManager.Instance.GetDrinkColor();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnTap()
    {
        _curTaps++;
        SetToppingPos();

        if(_curTaps > _requiredTaps)
        {
            FinishMinigame();
        }
    }

    private void FinishMinigame()
    {
        SoundManager.Instance?.PlayOneShot(SoundType.WIN);
        StopCoroutine(_decreaseTapsCoroutine);
        StartCoroutine(MinigameEnd());
    }

    public void OnHold()
    {
        StartCoroutine(SelectorChanger());
        _curTaps = 0;
    }

    public void OnHoldRelease()
    {
        StopAllCoroutines();
    }

    private void SetSelection()
    {
        foreach(GameObject selectable in _selectables)
        {
            if(selectable == _curSelection)
            {
                selectable.SetActive(true);
                continue;
            }

            selectable.SetActive(false);
        }
    }

    private void SetToppingPos()
    {
        _curSelection.transform.position = Vector2.Lerp(new Vector2(_initialPos.position.x, _initialPos.position.y), new Vector2(_finalPos.position.x, _finalPos.position.y), (float)_curTaps / _requiredTaps);
    }

    private IEnumerator SelectorChanger()
    {
        while(true)
        {
            _curSelectionIdx = (_curSelectionIdx + 1) % _selectables.Length;
            _curSelection = _selectables[_curSelectionIdx];

            SetSelection();

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator TapDecreaser()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.35f);
            _curTaps--;
            if(_curTaps < 0)
            {
                _curTaps = 0;
            }

            SetToppingPos();
        }
    }

    private IEnumerator MinigameEnd()
    {
        DrinkTopping topping = DrinkTopping.PEPPER;

        switch(_curSelection.name)
        {
            case "Lemon":
                topping = DrinkTopping.LEMON;
                break;

            case "Olive":
                topping = DrinkTopping.OLIVE;
                break;

            case "Umbrella":
                topping = DrinkTopping.UMBRELLA;
                break;
            
            case "Pepper":
                topping = DrinkTopping.PEPPER;
                break;

            default:
                break;
        }

        DrinkManager.Instance.SetTopping(topping);
        _onComplete?.Invoke();
        
        yield return new WaitForSeconds(2f);
        GetComponent<MinigameController>().SetMinigameComplete();
    }
}
