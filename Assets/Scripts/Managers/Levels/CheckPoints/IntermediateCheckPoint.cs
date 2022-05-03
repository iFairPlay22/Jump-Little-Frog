using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawn))]
[RequireComponent(typeof(SfxManager))]
public class IntermediateCheckPoint : RaycastCheckPoint
{
    [Header("SFX")]

    [SerializeField]
    AudioClip reachedAudioClip;

    SfxManager _sfxManager;
    Spawn _spawn;

    public override void Awake()
    {
        base.Awake();
        _spawn = GetComponent<Spawn>();
        _sfxManager = GetComponent<SfxManager>();

    }
    protected override void OnCheckPointReached()
    {
        _sfxManager.Play(reachedAudioClip);
        FindObjectOfType<LevelManager>().CheckPointReached(_spawn);
    }
}
