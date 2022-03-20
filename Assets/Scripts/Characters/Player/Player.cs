using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SfxManager))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    #region Values

    #region Serialize fields
    [Header("Speed")]
    [SerializeField]
    [Range(200, 300)]
    float speed = 250;

    [SerializeField]
    [Range(1f, 3f)]
    float runningSpeedAmplifier = 1.5f;

    [SerializeField]
    [Range(0f, 1f)]
    float crouchingSpeedAmplifier = 0.5f;

    [Header("Stand")]

    [SerializeField]
    Collider2D standingCheckCollider;

    [SerializeField]
    Transform groundCheckTransform;

    [SerializeField]
    [Range(0f, 0.2f)]
    float groundCheckRadius = 0.15f;

    [Header("Jump")]

    [SerializeField]
    [Range(0, 3)]
    int maxSuccessiveJumps = 2;

    [SerializeField]
    [Range(250, 350)]
    float jumpPower = 300f;

    [Header("Slice walls")]
    [SerializeField]
    [Range(0f, 5f)]
    float slidingFactor = 3;

    [SerializeField]
    Transform wallCheckTransform;

    [SerializeField]
    [Range(0f, 0.2f)]
    float wallCheckRadius = 0.15f;

    [Header("Crouch")]

    [SerializeField]
    Collider2D crouchingCheckCollider;

    [SerializeField]
    Transform overHeadLeftCheckTransform;

    [SerializeField]
    Transform overHeadRightCheckTransform;

    [SerializeField]
    [Range(0f, 0.2f)]
    float overHeadCheckRadius = 0.15f;

    [Header("Appear / Disappear")]
    [SerializeField]
    [Range(0f, 2f)]
    float appearSpeedAnimation = 0.8f;

    [SerializeField]
    [Range(0f, 2f)]
    float disappearSpeedAnimation = 0.8f;

    [Header("SFX")]
    [SerializeField]
    ParticleSystem movingParticleSystem;

    [Header("Layer")]

    [SerializeField]
    LayerMask groundLayerMask;

    [Header("SFX")]

    [SerializeField]
    AudioClip jumpAudioClip;

    [Header("Debug")]
    [SerializeField]
    bool displayGizmos = false;

    #endregion

    #region Private values

    #region Components
    Rigidbody2D _rigidbody;
    Animator _animator;
    SfxManager _sfxManager;
    #endregion

    #region Inputs
    float _moveInputValue = 0f;
    bool _runInputValue = false;
    bool _crouchInputValue = false;
    bool _jumpInputValue = false;
    #endregion

    #region Movement
    private enum States { APPEARING, READY, DISAPPERING };
    States _state =  States.APPEARING;
    [SerializeField]
    bool _facingRight = true;
    [SerializeField]
    bool _isGrounded = false;
    [SerializeField]
    bool _isRunning = false;
    [SerializeField]
    bool _isCrouching = false;
    [SerializeField]
    bool _isJumping = false;
    [SerializeField]
    bool _isSliding = false;
    int _successiveJumps = 0;
    #endregion

    #endregion

    #endregion

    #region Methods
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sfxManager = GetComponent<SfxManager>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("appearAnimationSpeed", appearSpeedAnimation);
        _animator.SetFloat("disappearAnimationSpeed", disappearSpeedAnimation);
        _animator.SetBool("updateAnims", true);

        StartCoroutine(Appear());
    }

    void Update()
    {
        // Left or right arrow input (to move)
        _moveInputValue = Input.GetAxisRaw("Horizontal");

        // Shift input (to run)
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _runInputValue = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _runInputValue = false;

        // Jump input (to jump)
        if (Input.GetButtonDown("Jump"))
            _jumpInputValue = true;

        // Crouch input (to crouch)
        if (Input.GetButtonDown("Crouch"))
            _crouchInputValue = true;
        if (Input.GetButtonUp("Crouch"))
            _crouchInputValue = false;

    }

    void OnDrawGizmos()
    {
        if (displayGizmos)
        {
            Gizmos.color = Color.red;

            // Draw the detection colliders
            Gizmos.DrawSphere(wallCheckTransform.position, wallCheckRadius);
            Gizmos.DrawSphere(overHeadRightCheckTransform.position, overHeadCheckRadius);
            Gizmos.DrawSphere(overHeadLeftCheckTransform.position, overHeadCheckRadius);
            Gizmos.DrawSphere(groundCheckTransform.position, groundCheckRadius);
        }
    }

    void FixedUpdate()
    {
        if (!_CanMoveOrInteract())
            return;

        _GroundCheck();
        _SliceWallsCheck();
        _Jump();
        _Crouch();
        _Move();
    }

    bool _CanMoveOrInteract()
    {
        if (_state == States.APPEARING)
            return false;

        if (_state == States.DISAPPERING)
            return false;

        return true;
    }

    void _GroundCheck()
    {
        _isGrounded = false;

        // Check if the GroundCheckObject is colliding with other 2D colliders that are in the "Ground" Layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
        if (colliders.Length > 0)
            _isGrounded = true;
    }
    
    void _SliceWallsCheck()
    {
        bool wallsCollision = Physics2D.OverlapCircle(wallCheckTransform.transform.position, wallCheckRadius, groundLayerMask);
        bool isFalling = _rigidbody.velocity.y < 0 && !_isGrounded;
        bool wantToMove = _moveInputValue != 0;
        bool isSliding = wallsCollision && wantToMove && isFalling;

        if (isSliding)
        {
            _successiveJumps = 1;
            _isSliding = true;
        } else
        {
            _isSliding = false;
        }
    }

    void _Jump()
    {
        // Jump
        if (_jumpInputValue) {
            if (_successiveJumps != maxSuccessiveJumps)
            {
                _isGrounded = false;
                _isJumping = true;
                _successiveJumps++;
                _rigidbody.velocity = Vector3.up * jumpPower * Time.fixedDeltaTime;
                _sfxManager.Play(jumpAudioClip);
            }
            _jumpInputValue = false;
        }
        
        if (_isGrounded) {
            _successiveJumps = 0;
            _isJumping = false;
        }

        _animator.SetBool("jump", _isJumping);
    }

    public void Propulse(float propulsionPower)
    {
        _rigidbody.velocity = Vector3.up * propulsionPower * Time.fixedDeltaTime;
        _successiveJumps = 0;
    }

    void _Crouch()
    {
        if (_isGrounded)
        {
            if (_isCrouching)
            {
                // If we are crouching and we want not to crouch again, we verify that we can
                if (!_crouchInputValue)
                {
                    bool rightOverlap = Physics2D.OverlapCircle(overHeadRightCheckTransform.position, overHeadCheckRadius, groundLayerMask);
                    bool leftOverlap = Physics2D.OverlapCircle(overHeadLeftCheckTransform.position, overHeadCheckRadius, groundLayerMask);
                    _isCrouching = rightOverlap || leftOverlap;
                }
            } else
            {
                _isCrouching =_crouchInputValue;
            }

            // Crouch (update the colliders)
            crouchingCheckCollider.enabled = _isCrouching;
            standingCheckCollider.enabled = !_isCrouching;

            _animator.SetBool("isCroutching", _isCrouching);
        } else if (_isCrouching)
        {
            _isCrouching = false;
            _animator.SetBool("isCroutching", _isCrouching);
        }

    }

    void _Move()
    {

        #region Move

        // Move x
        float xDirectionInput = _moveInputValue;
        float xVelocity = speed * xDirectionInput * Time.fixedDeltaTime;

        // Run x
        _isRunning = !_isCrouching && !_isJumping && _isGrounded && _runInputValue && xDirectionInput != 0;
        if (_isRunning) xVelocity *= runningSpeedAmplifier;

        // Crouch x
        if (_isCrouching) xVelocity *= crouchingSpeedAmplifier;

        // Fall y
        float yVelocity = _rigidbody.velocity.y;

        if (_isSliding)
        {
            yVelocity = - slidingFactor;
        }
        
        Vector3 targetVelocity = new Vector2(xVelocity, yVelocity);
        _rigidbody.velocity = targetVelocity;

        #endregion

        #region Particles
        if (_isGrounded && movingParticleSystem.isStopped)
            movingParticleSystem.Play();

        if (!_isGrounded && !movingParticleSystem.isStopped)
            movingParticleSystem.Stop();

        #endregion

        #region Rotate

        if (_facingRight && xVelocity < 0)
        {
            // If looking right and go to left, make a rotation
            transform.localScale = new Vector3(-1, 1, 1);
            _facingRight = false;
        } else if (!_facingRight && xVelocity > 0)
        {
            // If looking left and go to right, make a rotation
            transform.localScale = new Vector3(1, 1, 1);
            _facingRight = true;
        }

        #endregion

        #region Animator
        // xVelocityVal : 0 (idle) / 1 (walk) / 2 (run)
        float xVelocityVal = 0;
        if (xDirectionInput != 0)
            xVelocityVal = 1;
        if (_isRunning)
            xVelocityVal = 2;

        // yVelocityVal : 0 < (falling) / < 0 (jumping)
        float yVelocityVal = yVelocity;

        _animator.SetFloat("xVelocity", xVelocityVal);
        _animator.SetFloat("yVelocity", yVelocityVal);

        #endregion

    }

    void _FreezePosition()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _rigidbody.freezeRotation = true;
    }

    void _UnFreezePosition()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.freezeRotation = true;
    }

    IEnumerator Appear()
    {
        _FreezePosition();
        _state = States.APPEARING;
        yield return new WaitForSeconds(1f / appearSpeedAnimation);
        _state = States.READY;
        _UnFreezePosition();
    }
    IEnumerator Disappear()
    {
        _FreezePosition();
        _state = States.DISAPPERING;
        _animator.SetBool("updateAnims", false);
        _animator.Play("Fox_disappear");
        yield return new WaitForSeconds(1f / disappearSpeedAnimation);
        FindObjectOfType<LevelManager>().Defeat();
    }

    public void Die() 
    { 
        _state = States.DISAPPERING;

        StartCoroutine(Disappear());
    }

    #endregion
}

