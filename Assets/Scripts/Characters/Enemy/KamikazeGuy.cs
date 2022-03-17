using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class KamikazeGuy : RaycastDetection
{
    [Header("Target to follow")]
    [SerializeField]
    Transform Target;

    [Header("Walk")]
    [SerializeField]
    [Range(0.1f, 10f)]
    float WalkingSpeed = 3f;

    [SerializeField]
    [Range(0.1f, 5f)]
    float WalkingSeconds = 3f;

    [Header("Run")]
    [SerializeField]
    [Range(0.1f, 10f)]
    float RunningSpeed = 3f;

    [SerializeField]
    [Range(0.1f, 5f)]
    float RunningSeconds = 3f;

    [Header("Explode")]
    [SerializeField]
    [Range(0.1f, 10f)]
    float ExplodingSpeed = 3f;

    [SerializeField]
    [Range(0.1f, 5f)]
    float ExplodingSeconds = 3f;

    float _currentSpeed = 0f;
    Explosion _explosionSystem;
    Animator _animator;
    Rigidbody2D _rigidbody;

    enum States { WAITING, WALKING, RUNNING, EXPLODING }
    States _currentState = States.WAITING;

    void Awake()
    {
        _explosionSystem = GetComponent<Explosion>();

        _animator = GetComponent<Animator>();
        _animator.SetFloat("ExplodingAnimationTime", 1.0f / ExplodingSeconds);

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override bool ShoudDetectRaycastCollisions()
    {
        return _currentState == States.WAITING;
    }

    protected override void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D hitPoint, Vector3 direction)
    {
        StartCoroutine(_ChargeAndExplode());
    }

    IEnumerator _ChargeAndExplode()
    {
        // Walk
        _currentState = States.WALKING;
        _currentSpeed = WalkingSpeed;
        _animator.SetBool("isWalking", true);

        yield return new WaitForSeconds(WalkingSeconds);

        // Run
        _currentState = States.RUNNING;
        _currentSpeed = RunningSpeed;
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isRunning", true);

        yield return new WaitForSeconds(RunningSeconds);

        // Explode
        _currentSpeed = ExplodingSpeed;
        _currentState = States.EXPLODING;
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isExploding", true);

        yield return new WaitForSeconds(ExplodingSeconds);

        _animator.SetBool("isExploding", false);
        _explosionSystem.Explode();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (
            _currentState == States.WALKING || 
            _currentState == States.RUNNING || 
            _currentState == States.EXPLODING
        )
            _Move();
    }
    void _Move()
    {
        // Follow the Target
        Vector3 current = transform.parent.position;
        Vector3 destination = Vector3.MoveTowards(current, Target.position, _currentSpeed * Time.deltaTime);
        destination = new Vector3(destination.x, current.y, current.z);
        transform.parent.position = destination;

        // Follow in direction of the Target
        transform.localScale = new Vector3(Target.position.x > transform.parent.position.x ? -1 : 1, 1, 1);
    }

    public void ExplosionCallback()
    {
        Destroy(gameObject);
    }
}
