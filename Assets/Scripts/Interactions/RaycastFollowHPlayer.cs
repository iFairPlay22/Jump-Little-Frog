using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SfxManager))]
public class RaycastFollowHPlayer : RaycastDetection
{
    [Header("Run")]
    [SerializeField]
    [Range(0.1f, 10f)]
    float RunningSpeed = 3f;

    Vector3 _target;
    Animator _animator;
    enum States { WAITING, RUNNING }

    [SerializeField]
    States _currentState = States.WAITING;

    [Header("SFX")]
    [SerializeField]
    AudioClip runningAudioClip;
    bool _canPlaySfx = true;
    SfxManager _sfxManager;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sfxManager = GetComponent<SfxManager>();
    }

    protected override bool ShoudDetectRaycastCollisions()
    {
        return true;
    }

    protected override void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D playerHit, Vector3 direction)
    {
        _currentState = States.RUNNING;
        _target = playerHit.point;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_currentState == States.RUNNING)
            _Move();

        _animator.SetBool("Run", _currentState == States.RUNNING);
    }

    void _Move()
    {
        if (runningAudioClip && _canPlaySfx)
        {
            _sfxManager.Play(runningAudioClip);
            _canPlaySfx = false;
        }
            

        // Follow the Target
        Vector3 current = transform.position;
        Vector3 next = Vector3.MoveTowards(current, _target, RunningSpeed * Time.deltaTime);
        next = new Vector3(next.x, current.y, current.z);
        transform.position = next;

        if (Vector3.Distance(next, _target) <= 0.3f)
        {
            _currentState = States.WAITING;
            _canPlaySfx = true;
        }

        // Flip in direction of the Target
        transform.localScale = new Vector3(_target.x > next.x ? -1 : 1, 1, 1);
    }
}
