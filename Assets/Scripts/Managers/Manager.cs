using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Only one intance for each managers
 **/
public class Manager : MonoBehaviour
{
    private static bool _alreadyCreated = false;

    private void Awake()
    {
        if (_alreadyCreated)
        {
            Destroy(gameObject);
            return;
        }

        _alreadyCreated = true;
        DontDestroyOnLoad(gameObject);
    }
}
