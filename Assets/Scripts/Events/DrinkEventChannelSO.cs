using UnityEngine;

/// <summary>
/// General event channel that broadcasts and carries DrinkColor payload.
/// </summary>
[CreateAssetMenu(menuName = "Events/Drink Event Channel", fileName = "DrinkEventChannel")]
public class DrinkEventChannelSO : GenericEventChannelSO<Drink>
{
}
