using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootVictim : MonoBehaviour
{
    [SerializeField]
    UnityEvent Action;

    public void OnAction()
    {
        Action?.Invoke();
    }
}
