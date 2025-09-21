using UnityEngine;

/// <summary>
/// General event channel that broadcasts and carries GameObject payload.
/// </summary>
[CreateAssetMenu(menuName = "Events/Int Event Channel", fileName = "GameObjectEventChannel")]
public class IntEventChannelSO : GenericEventChannelSO<int>
{
}