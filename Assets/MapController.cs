using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapController : NetworkBehaviour
{
    // Placement
    [SerializeField] private List<Transform> startPositions;
    private NetworkVariable<int> _playerIndex = new NetworkVariable<int>(0);

    [SerializeField] private Transform carSpawnStartPoint;
    private NetworkVariable<int> _spawnedPlayerCount = new NetworkVariable<int>(0);
    
    public Vector3 GetEmptyStartPosition()
    {
        Vector3 position = Vector3.zero;
        if (_playerIndex.Value < startPositions.Count)
        {
            position = startPositions[_playerIndex.Value].position;
        }

        IncreasePlayerIndex_ServerRpc();

        return position;
    }

    public Vector3 GetEmptyCarSpawnPosition()
    {
        var pos = carSpawnStartPoint.position + (new Vector3(5,0,0) * _spawnedPlayerCount.Value);
        IncreaseSpawnedPlayerCount_ServerRpc();

        return pos;
    }

    [ServerRpc(RequireOwnership = false)]
    private void IncreasePlayerIndex_ServerRpc()
    {
        _playerIndex.Value++;
    }

    [ServerRpc(RequireOwnership = false)]
    private void IncreaseSpawnedPlayerCount_ServerRpc()
    {
        _spawnedPlayerCount.Value++;
    }
    
}
