using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // Placement
    [SerializeField] private List<Transform> startPositions;
    private int _playerIndex = 0;

    [SerializeField] private Transform carSpawnStartPoint;
    private int _spawnedPlayerCount = 0;
    public Vector3 GetEmptyStartPosition()
    {
        Vector3 position = Vector3.zero;
        if (_playerIndex < startPositions.Count)
        {
            position = startPositions[_playerIndex].position;
        }

        _playerIndex++;

        return position;
    }

    public Vector3 GetEmptyCarSpawnPosition()
    {
        var pos = carSpawnStartPoint.position + (new Vector3(5,0,0) * _spawnedPlayerCount);
        _spawnedPlayerCount++;

        return pos;
    }
}
