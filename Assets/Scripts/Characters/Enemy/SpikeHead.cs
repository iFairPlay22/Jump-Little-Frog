using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class SpikeHead : RaycastDetection
{
    #region Serializable Fields

    [Header("Time")]

    [SerializeField]
    [Range(1f, 5f)]
    float SecondsBeforeMove = 3.0f;

    [SerializeField]
    [Range(0f, 3f)]
    float TimeForCollisionAnimation = 2.0f;

    [SerializeField]
    [Range(1f, 10f)]
    float Speed = 5.0f;

    #endregion

    #region Private Fields

    #region Movement & Collisions
    bool _ignoreRaycastDetection = false;
    bool _isMoving = false;
    Vector3? _moveTo;
    string _onCollisionAnimationName = "";
    #endregion

    #region Components
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    #endregion

    #endregion

    #region Movement & Collisions

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("collisionAnimationSpeed", TimeForCollisionAnimation);
    }

    protected override bool ShoudDetectRaycastCollisions()
    {
        return !_ignoreRaycastDetection;
    }

    protected override void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D playerHit, Vector3 direction)
    {
        if (groundHit.collider != null && playerHit.distance < groundHit.distance)
            
        _moveTo = groundHit.point;

        if (direction == Vector3.down || direction == Vector3.up)
        {
            if (direction == Vector3.down)
                _onCollisionAnimationName = "bottomHit";
            else
                _onCollisionAnimationName = "topHit";

            _moveTo -= direction * (_spriteRenderer.bounds.size.y / 2.0f);

        }
        else
        {
            if (direction == Vector3.right)
                _onCollisionAnimationName = "rightHit";
            else
                _onCollisionAnimationName = "leftHit";

            _moveTo -= direction * (_spriteRenderer.bounds.size.y / 2.0f);
        }

        _ignoreRaycastDetection = true;
        StartCoroutine(WaitBeforeMove());
    }

    IEnumerator WaitBeforeMove()
    {
        yield return new WaitForSeconds(SecondsBeforeMove);
        _isMoving = true;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isMoving)
        {
            _Move();
        }
    }

    void _Move()
    {
        if (_moveTo == null)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, _moveTo.Value, Time.fixedDeltaTime * Speed);
        
        // Si on se rapproche grandement du vecteur destination
        if (Mathf.Abs(Vector3.Distance(currentPosition, _moveTo.Value)) <= 0.1f)
        {
            // On joue l'animation de collision
            _animator.Play("SpikeHead_" + _onCollisionAnimationName);
            _onCollisionAnimationName = "";

            nextPosition = _moveTo.Value;

            _moveTo = null;
            _isMoving = false;
            _ignoreRaycastDetection = false;
        }

        transform.position = nextPosition;
    }

    #endregion
}
