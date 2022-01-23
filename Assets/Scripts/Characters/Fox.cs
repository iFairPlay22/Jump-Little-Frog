using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    #region Values

    #region Serialize fields
    [Header("Speed")]
    [SerializeField]
    float speed = 50;

    [SerializeField]
    float runningSpeedAmplifier = 2f;

    [SerializeField]
    float crouchingSpeedAmplifier = 0.5f;

    [Header("Stand")]

    [SerializeField]
    Collider2D standingCheckCollider;

    [SerializeField]
    Transform groundCheckCollider;

    [SerializeField]
    float groundCheckRadius = 0.02f;

    [Header("Jump")]

    [Range(0, 3)]
    [SerializeField]
    int maxSuccessiveJumps = 2;

    [SerializeField]
    [Range(1, 3)]
    float jumpPower = 2.7f;

    [Header("Crouch")]

    [SerializeField]
    Collider2D crouchingCheckCollider;

    [SerializeField]
    Transform overHeadLeftCheckCollider;

    [SerializeField]
    Transform overHeadRightCheckCollider;

    [SerializeField]
    float overHeadCheckRadius = 0.02f;

    [Header("Layer")]

    [SerializeField]
    LayerMask groundLayerMask;

    [Header("Debug")]
    [SerializeField]
    bool displayGizmos = false;

    #endregion

    #region Private values

    #region Components
    Rigidbody2D _rigidbody;
    Animator _animator;
    #endregion

    #region Inputs
    float _moveInputValue = 0f;
    bool _runInputValue = false;
    bool _crouchInputValue = false;
    bool _jumpInputValue = false;
    #endregion

    #region Movement
    [SerializeField] bool _facingRight = true;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] bool _isRunning = false;
    [SerializeField] bool _isCrouching = false;
    [SerializeField] bool _isJumping = false;
    int _successiveJumps = 0;
    #endregion

    #endregion

    #endregion

    #region Methods
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
            Gizmos.DrawSphere(overHeadRightCheckCollider.position, overHeadCheckRadius);
            Gizmos.DrawSphere(overHeadLeftCheckCollider.position, overHeadCheckRadius);
            Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        }
    }

    void FixedUpdate()
    {
        _GroundCheck();
        _Jump();
        _Crouch();
        _Move();
    }

    void _GroundCheck()
    {
        _isGrounded = false;

        // Check if the GroundCheckObject is colliding with other 2D colliders that are in the "Ground" Layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayerMask);
        if (colliders.Length > 0)
            _isGrounded = true;
    }
    
    void _Jump()
    {
        // Jump
        if (_jumpInputValue && _successiveJumps != maxSuccessiveJumps)
        {
            _isGrounded = false;
            _isJumping = true;
            _successiveJumps++;
            _rigidbody.velocity = Vector3.up * jumpPower;
            _jumpInputValue = false;
        }
        
        if (_isGrounded) {
            _successiveJumps = 0;
            _isJumping = false;
        }

        _animator.SetBool("jump", _isJumping);
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
                    bool rightOverlap = Physics2D.OverlapCircle(overHeadRightCheckCollider.position, overHeadCheckRadius, groundLayerMask);
                    bool leftOverlap = Physics2D.OverlapCircle(overHeadLeftCheckCollider.position, overHeadCheckRadius, groundLayerMask);
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
        
        Vector3 targetVelocity = new Vector2(xVelocity, yVelocity);
        _rigidbody.velocity = targetVelocity;

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

    #endregion
}

