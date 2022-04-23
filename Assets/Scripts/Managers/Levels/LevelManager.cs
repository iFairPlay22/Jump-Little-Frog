using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector3 _initialSpawnPoint;
    Vector3 _lastSpawnPoint;
    Vector3 _currentSpawnPoint;

    [SerializeField]
    float TimeBeforeSpawn = 2;

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
        Debug.Log("Defeat");
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
        StartCoroutine(_Spawn());
    }

    void _SpawnInLastCheckPoint()
    {
        _currentSpawnPoint = _lastSpawnPoint;
        StartCoroutine(_Spawn());
    }

    IEnumerator _Spawn()
    {
        yield return new WaitForSeconds(TimeBeforeSpawn);
        FindObjectOfType<Player>().Spawn(_currentSpawnPoint);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }
}


