using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector3 _initialSpawnPoint;
    Vector3 _lastSpawnPoint;

    void _DebugAll(string fname)
    {
        Debug.Log(fname + "() init=" + _initialSpawnPoint + " last=" + _lastSpawnPoint);
    }

    private void Start()
    {
        _initialSpawnPoint = FindObjectOfType<StartCheckPoint>().GetComponent<Spawn>().GetSpawnPoint();
        _lastSpawnPoint = _initialSpawnPoint;
        _SpawnInInitialCheckPoint();
        _DebugAll("Start()");
    }

    public void CheckPointReached(Spawn currentSpawn)
    {
        _lastSpawnPoint = currentSpawn.GetSpawnPoint();
        _DebugAll("Start()");
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
        FindObjectOfType<Player>().gameObject.transform.position = _initialSpawnPoint;
        _DebugAll("_SpawnInInitialCheckPoint()");
    }

    void _SpawnInLastCheckPoint()
    {
        FindObjectOfType<Player>().gameObject.transform.position = _lastSpawnPoint;
        _DebugAll("_SpawnInLastCheckPoint()");
    }
}

    
        