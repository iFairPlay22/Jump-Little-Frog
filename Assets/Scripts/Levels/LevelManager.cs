using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Victory()
    {
        _ReloadCurrentScene();
    }

    public void Defeat()
    {
        _ReloadCurrentScene();
    }

    void _ReloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
