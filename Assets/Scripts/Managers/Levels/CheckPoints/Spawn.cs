using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    Transform SpawnPoint;

    public Vector3 GetSpawnPoint()
    {
        return SpawnPoint.position;
    }
}
