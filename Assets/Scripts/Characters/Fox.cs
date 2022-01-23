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

    [SerializeField]
    float jumpPower = 150;

    [Header("Stand")]

    [SerializeField]
    Collider2D standingCheckCollider;

    [SerializeField]
    Transform groundCheckCollider;

    [SerializeField]
    float groundCheckRadius = 0.02f;

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
    bool displayCollidersRange = false;

    #endregion

    #region Private values

    #region Components
    Rigidbody2D _rigidbody;
    Animator _animator;
    #endregion

    #region Inputs
    float _horizontalInputValue = 0f;
    bool _jumpInputValue = false;
    bool _crouchInputValue = false;
    #endregion

    #region Movement
    [SerializeField] bool _isRunning = false;
    [SerializeField] bool _facingRight = true;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] bool _isCrouching = false;
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
        _horizontalInputValue = Input.GetAxisRaw("Horizontal");

        // Shift input (to run)
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _isRunning = false;

        // Jump input (to jump)
        if (Input.GetButtonDown("Jump"))
            _jumpInputValue = true;
        if (Input.GetButtonUp("Jump"))
            _jumpInputValue = false;

        // Crouch input (to crouch)
        if (Input.GetButtonDown("Crouch"))
            _crouchInputValue = true;
        if (Input.GetButtonUp("Crouch"))
            _crouchInputValue = false;

    }

    void FixedUpdate()
    {
        GroundCheck();
        _Move(_horizontalInputValue, _jumpInputValue, _crouchInputValue);
    }

    void GroundCheck()
    {
        _isGrounded = false;

        // Check if the GroundCheckObject is colliding with other 2D colliders that are in the "Ground" Layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayerMask);
        if (colliders.Length > 0)
            _isGrounded = true;
    }

    private void OnDrawGizmos()
    {
        if (displayCollidersRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(overHeadRightCheckCollider.position, overHeadCheckRadius);
            Gizmos.DrawSphere(overHeadLeftCheckCollider.position, overHeadCheckRadius);
            Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        }
    }

    void _Move(float xDirectionInput, bool jump, bool crouch)
    {
        #region Jump and crouch

        if (jump && crouch)
            crouch = false;
            
        if (_isGrounded)
        {
            // If we are crouching and we want not to crouch again, we verify that we can
            if (_isCrouching && !crouch)
            {
                bool rightOverlap = Physics2D.OverlapCircle(overHeadRightCheckCollider.position, overHeadCheckRadius, groundLayerMask);
                bool leftOverlap = Physics2D.OverlapCircle(overHeadLeftCheckCollider.position, overHeadCheckRadius, groundLayerMask);
                if (rightOverlap || leftOverlap)
                    crouch = true;
            }

            _isCrouching = crouch;

            // Crouch (update the colliders)
            crouchingCheckCollider.enabled = crouch;
            standingCheckCollider.enabled = !crouch;

            // Jump
            if (jump)
            {
                _isGrounded = false;
                _rigidbody.AddForce(new Vector2(0f, jumpPower)); 
                _animator.SetBool("jump", true);
            } else
            {
                _animator.SetBool("jump", false);
            }
        }

        _animator.SetBool("isCroutching", _isCrouching);

        #endregion

        #region Move and run
        // Move x
        float xVelocity = speed * xDirectionInput * Time.fixedDeltaTime;

        // Run x
        if (_isRunning) xVelocity *= runningSpeedAmplifier;

        // Crouch x
        if (crouch) xVelocity *= crouchingSpeedAmplifier;

        // Fall y
        float yVelocity = _rigidbody.velocity.y;
        
        Vector3 targetVelocity = new Vector2(xVelocity, yVelocity);
        _rigidbody.velocity = targetVelocity;

        // Rotate
        // If looking right and go to left, make a rotation
        if (_facingRight && xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _facingRight = false;
        } else if (!_facingRight && xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _facingRight = true;
        }

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

