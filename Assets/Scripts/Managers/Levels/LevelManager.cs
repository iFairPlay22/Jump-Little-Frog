using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector3 _initialSpawnPoint;
    Vector3 _lastSpawnPoint;
    Vector3 _currentSpawnPoint;

    void Awake()
    {
        _initialSpawnPoint = FindObjectOfType<StartCheckPoint>().GetComponent<Spawn>().GetSpawnPoint();
        _lastSpawnPoint = _initialSpawnPoint;
        _SpawnInInitialCheckPoint();
    }

    public void CheckPointReached(Spawn currentSpawn)
    {
        _lastSpawnPoint = currentSpawn.GetSpawnPoint();
    }

    public void Victory()
    {
        _ReloadCurrentScene();
        _SpawnInInitialCheckPoint();
    }

    public void Defeat()
    {
        _ReloadCurrentScene();
        _SpawnInLastCheckPoint();
    }
    void _ReloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void _SpawnInInitialCheckPoint()
    {
        _currentSpawnPoint = _initialSpawnPoint;
    }

    void _SpawnInLastCheckPoint()
    {
        _currentSpawnPoint = _lastSpawnPoint;
    }

    public Vector3 GetCurrentSpawnPoint()
    {
        return _currentSpawnPoint;
    }
}

    
        