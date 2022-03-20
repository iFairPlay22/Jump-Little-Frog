using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawn))]
public class IntermediateCheckPoint : RaycastCheckPoint
{
    Spawn _spawn;

    public override void Awake()
    {
        base.Awake();
        _spawn = GetComponent<Spawn>();

    }
    protected override void OnCheckPointReached()
    {
        FindObjectOfType<LevelManager>().CheckPointReached(_spawn);
    }
}
