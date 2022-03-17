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

    [Header("Jump")]
    [SerializeField]
    Transform groundCheckTransform;

    [SerializeField]
    [Range(0f, 0.2f)]
    float groundCheckRadius = 0.15f;

    [SerializeField]
    LayerMask groundLayerMask;

    [SerializeField]
    [Range(200f, 300f)]
    float JumpSpeed = 200f;

    [SerializeField]
    Vector2 TargetHeightRangeToJump = new Vector2(1f, 3f);

    [SerializeField]
    Vector2 TargetWidthRangeToJump = new Vector2(0f, 4f);

    float _currentSpeed = 0f;
    bool _isGrounded = false;

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

        _GroundCheck();

        if (
            _currentState == States.WALKING || 
            _currentState == States.RUNNING || 
            _currentState == States.EXPLODING
        )
            _Move();
    }

    void _GroundCheck()
    {
        _isGrounded = false;

        // Check if the GroundCheckObject is colliding with other 2D colliders that are in the "Ground" Layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
        if (colliders.Length > 0)
            _isGrounded = true;
    }

    void _Move()
    {
        // Follow the Target
        Vector3 current = transform.parent.position;
        Vector3 now = Vector3.MoveTowards(current, Target.position, _currentSpeed * Time.deltaTime);
        now = new Vector3(now.x, current.y, current.z);
        transform.parent.position = now;

        // Jump if player is reachable
        if (_isGrounded)
        {   
            // If it is close enough
            float xDistance = Mathf.Abs(Target.position.x - now.x);
            float yDistance = Target.position.y - now.y;
            bool xReachable = TargetHeightRangeToJump.x <= xDistance && xDistance <= TargetHeightRangeToJump.y;
            bool yReachable = TargetWidthRangeToJump.x <= yDistance && yDistance <= TargetWidthRangeToJump.y;
            if (xReachable && yReachable)
            {
                // Jump
                _rigidbody.velocity = Vector3.up * JumpSpeed * Time.fixedDeltaTime;
            }
        }

        // Follow in direction of the Target
        transform.localScale = new Vector3(Target.position.x > now.x ? -1 : 1, 1, 1);
    }

    public void ExplosionCallback()
    {
        Destroy(gameObject);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!DisplayGizmos)
            return;

        float xMin = TargetWidthRangeToJump.x;
        float xMax = TargetWidthRangeToJump.y;
        float yMin = TargetHeightRangeToJump.x;
        float yMax = TargetHeightRangeToJump.y;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
           transform.position + new Vector3(-xMax, yMax, 0),
           transform.position + new Vector3(xMin, yMax, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(-xMax, yMax, 0),
           transform.position + new Vector3(-xMax, yMin, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(xMin, yMax, 0),
           transform.position + new Vector3(xMin, yMin, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(-xMax, yMin, 0),
           transform.position + new Vector3(xMin, yMin, 0)
        );

        Gizmos.DrawLine(
           transform.position + new Vector3(xMax, yMax, 0),
           transform.position + new Vector3(-xMin, yMax, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(xMax, yMax, 0),
           transform.position + new Vector3(xMax, yMin, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(xMin, yMax, 0),
           transform.position + new Vector3(-xMin, yMin, 0)
        );
        Gizmos.DrawLine(
           transform.position + new Vector3(xMax, yMin, 0),
           transform.position + new Vector3(-xMin, yMin, 0)
        );

        Gizmos.DrawSphere(groundCheckTransform.position, groundCheckRadius);
    }

}
