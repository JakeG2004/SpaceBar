using UnityEngine;
using System.Collections;

public class DishwashingMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _waterPrefab;
    [SerializeField] private Transform _spawnPoint;

    [Header("Settings")]
    [SerializeField] private float _maxPressure = 1f;        // Max faucet pressure
    [SerializeField] private float _pressureRechargeRate = 0.2f; // Pressure restored per second
    [SerializeField] private float _pressureDrainPerWater = 0.05f; // Pressure lost per water unit
    [SerializeField] private int _maxWaterPerDispense = 10;  // Max number of water prefabs per use

    private float _waterPressure;
    private bool _isDispensing = false;

    void OnEnable()
    {
        _waterPressure = _maxPressure; // start full
        StartCoroutine(RegeneratePressure());
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
            StartCoroutine(DispenseWater(numWater));
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
        for (int i = 0; i < amount; i++)
        {
            Instantiate(_waterPrefab, _spawnPoint.position, Quaternion.identity);
            _waterPressure = Mathf.Clamp01(_waterPressure - _pressureDrainPerWater);
            yield return new WaitForSeconds(0.05f);
        }
        _isDispensing = false;
    }
}
