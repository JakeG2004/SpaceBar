using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DishwashingMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _waterPrefab;
    [SerializeField] private Transform _waterSpawnPoint;
    [SerializeField] private GameObject _cupPrefab;
    [SerializeField] private Transform _cupSpawnPoint;

    [Header("Settings")]
    [SerializeField] private float _maxPressure = 1f;        // Max faucet pressure
    [SerializeField] private float _pressureRechargeRate = 0.2f; // Pressure restored per second
    [SerializeField] private float _pressureDrainPerWater = 0.05f; // Pressure lost per water unit
    [SerializeField] private int _maxWaterPerDispense = 10;  // Max number of water prefabs per use
    [SerializeField] private float _cupSpawnTime = 3f;

    [Header("Cup Variables")]
    public int numDirtyCups = 0;
    public int numCleanCups = 0;
    [SerializeField] private int _numCupsToClean = 0;

    [Header("End minigame variables")]
    [SerializeField] private GameObject _winText;
    [SerializeField] private UnityEvent _winEvent;

    private float _waterPressure;
    private bool _isDispensing = false;

    void OnEnable()
    {
        _winText.SetActive(false);
        numDirtyCups = DrinkManager.Instance.dirtyCups;
        if(numDirtyCups == 0)
        {
            StartCoroutine(WinGame());
            return;
        }

        _numCupsToClean = numDirtyCups;
        _waterPressure = _maxPressure; // start full
        StartCoroutine(RegeneratePressure());
        StartCoroutine(SpawnCups());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void DoWater()
    {
        if (_isDispensing) return; // prevent overlapping dispenses

        // Calculate how many water drops we can spawn based on current pressure
        int numWater = Mathf.RoundToInt(_maxWaterPerDispense * _waterPressure);
        if (numWater > 0)
        {
            StartCoroutine(DispenseWater(numWater));
        }
    }

    public void CupCleaned()
    {
        numDirtyCups--;
        numCleanCups++;

        if(numCleanCups >= _numCupsToClean)
        {
            StartCoroutine(WinGame());
        }
    }

    private IEnumerator RegeneratePressure()
    {
        while (true)
        {
            if (!_isDispensing)
            {
                // Slowly refill pressure if not dispensing
                _waterPressure = Mathf.Clamp01(_waterPressure + (_pressureRechargeRate * Time.deltaTime));
            }
            yield return null;
        }
    }

    private IEnumerator DispenseWater(int amount)
    {
        _isDispensing = true;
        SoundManager.Instance.PlayOneShot(SoundType.FAUCET, 0.1f);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(_waterPrefab, _waterSpawnPoint.position, Quaternion.identity);
            _waterPressure = Mathf.Clamp01(_waterPressure - _pressureDrainPerWater);
            yield return new WaitForSeconds(0.05f);
        }

        SoundManager.Instance.StopSound(SoundType.FAUCET, 0.2f);
        _isDispensing = false;
    }

    private IEnumerator SpawnCups()
    {
        int cupsToSpawn = numDirtyCups;
        if(numDirtyCups > 4)
        {
            cupsToSpawn = 4;
        }

        for(int i = 0; i < cupsToSpawn; i++)
        {
            Instantiate(_cupPrefab, _cupSpawnPoint).GetComponent<DirtyCup>().SetMinigame(this);
            numDirtyCups--;

            yield return new WaitForSeconds(_cupSpawnTime);
        }
    }

    private IEnumerator WinGame()
    {
        _winText.SetActive(true);
        _winEvent?.Invoke();
        SoundManager.Instance.PlayOneShot(SoundType.WIN);

        yield return new WaitForSeconds(2f);
        GetComponent<MinigameController>().SetMinigameComplete();
    }
}
