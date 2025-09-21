using UnityEngine;

/// <summary>
/// General event channel that broadcasts and carries Color payload.
/// </summary>
[CreateAssetMenu(menuName = "Events/Color Event Channel", fileName = "ColorEventChannel")]
public class ColorEventChannelSO : GenericEventChannelSO<Color>
{
}
