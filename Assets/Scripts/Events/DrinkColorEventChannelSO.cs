using UnityEngine;

/// <summary>
/// General event channel that broadcasts and carries DrinkColor payload.
/// </summary>
[CreateAssetMenu(menuName = "Events/DrinkColor Event Channel", fileName = "DrinkColorEventChannel")]
public class DrinkColorEventChannelSO : GenericEventChannelSO<DrinkColor>
{
}
