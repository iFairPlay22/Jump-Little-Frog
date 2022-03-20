using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheckPoint : RaycastCheckPoint
{
    protected override void OnCheckPointReached()
    {
        FindObjectOfType<LevelManager>().Victory();
    }
}
