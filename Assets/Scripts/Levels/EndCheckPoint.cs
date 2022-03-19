using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndCheckPoint : RaycastDetection
{
    [Header("Animation")]
    [SerializeField]
    float StartAnimateWhenTargetInRange = 5f;

    [SerializeField]
    Transform Target;

    bool _detectCollisions = true;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void Update()
    {
        // Enable checkpoint animations when the target is in the circle
        if (!_animator.enabled && Vector2.Distance(transform.position, Target.position) <= StartAnimateWhenTargetInRange)
        {
            _animator.enabled = true;
        }
    }

    protected override bool ShoudDetectRaycastCollisions()
    {
        return _detectCollisions;
    }

    protected override void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D hitPoint, Vector3 direction)
    {
        _detectCollisions = false;
        FindObjectOfType<LevelManager>().Victory();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!DisplayGizmos)
            return;

        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, StartAnimateWhenTargetInRange);
    }
}
