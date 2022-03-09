using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class SpikeHead : MonoBehaviour
{
    #region Serializable Fields

    [Header("Detection")]

    [SerializeField]
    [Range(10f, 100f)]
    float DetectionRange = 5.0f;

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

    [Header("Debug")]

    [SerializeField]
    bool DisplayGizmos = true;

    #endregion

    #region Private Fields

    #region Movement & Collisions
    Vector3[] _directionsToLookAt = {};
    bool move = false;
    Vector3? moveTo;
    string _onCollisionAnimationParameter = "";
    #endregion

    #region Components
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    #endregion

    #endregion

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _animator.SetFloat("collisionAnimationSpeed", TimeForCollisionAnimation);
        ResetCollisionAnimations();
    }

    #region Collision Animations

    private void ResetCollisionAnimations()
    {
        _animator.SetBool("bottomHit", false);
        _animator.SetBool("topHit", false);
        _animator.SetBool("rightHit", false);
        _animator.SetBool("leftHit", false);
    }

    private IEnumerator PlayCollisionAnimation()
    {
        Debug.Log(_onCollisionAnimationParameter + " " + (1.0f / TimeForCollisionAnimation));
        _animator.SetBool(_onCollisionAnimationParameter, true);
        yield return new WaitForSeconds(2.0f);
        ResetCollisionAnimations();
        _onCollisionAnimationParameter = "";
    }

    #endregion

    #region Movement & Collisions
    private void Start()
    {
        _InitDirections();
    }

    private void _InitDirections()
    {
        _directionsToLookAt = new Vector3[4];
        _directionsToLookAt[0] = transform.TransformDirection(Vector3.down);
        _directionsToLookAt[1] = transform.TransformDirection(Vector3.up);
        _directionsToLookAt[2] = transform.TransformDirection(Vector3.right);
        _directionsToLookAt[3] = transform.TransformDirection(Vector3.left);
    }

    private void FixedUpdate()
    {
        if (moveTo == null && move == false)
        {
            // On n'a pas détecté de collision, on ne bouge pas
            _MakeDetections();
        }

        if (moveTo != null && move == true)
        {
            // On a précedemment détecté une collision, on bouge
            _Move();
        }
    }

    private void _Move()
    {
        if (moveTo == null || move == false)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, moveTo.Value, Time.fixedDeltaTime * Speed);
        
        // Si on se rapproche grandement du vecteur destination
        if (Mathf.Abs(Vector3.Distance(currentPosition, moveTo.Value)) <= 0.1f)
        {
            // On joue l'animation de collision
            nextPosition = moveTo.Value;
            moveTo = null;
            move = false;
            StartCoroutine(PlayCollisionAnimation());
        }

        transform.position = nextPosition;
    }

    private void _MakeDetections()
    {
        foreach (Vector3 directionToLookAt in _directionsToLookAt)
        {
            RaycastHit2D playerHit = Physics2D.Raycast(
                transform.position,
                transform.TransformDirection(directionToLookAt),
                DetectionRange,
                1 << LayerMask.NameToLayer("Player")
            );

            // Si on a detecté le player dans cette direction
            if (playerHit.collider != null)
            {
                RaycastHit2D groundHit = Physics2D.Raycast(
                    transform.position,
                    transform.TransformDirection(directionToLookAt),
                    DetectionRange,
                    1 << LayerMask.NameToLayer("Ground")
                );

                if (groundHit.collider != null)
                {
                    // Si on a détecté le ground dans cette direction
                    // On update le vecteur de destination, et le paramètre
                    // de l'animation correspondante
                    moveTo = groundHit.point;
                    if (directionToLookAt == Vector3.down || directionToLookAt == Vector3.up)
                    {
                        if (directionToLookAt == Vector3.down)
                            _onCollisionAnimationParameter = "bottomHit";
                        else
                            _onCollisionAnimationParameter = "topHit";

                        moveTo -= directionToLookAt * (_spriteRenderer.bounds.size.y / 2.0f);

                    } else {
                        if (directionToLookAt == Vector3.right)
                            _onCollisionAnimationParameter = "rightHit";
                        else
                            _onCollisionAnimationParameter = "leftHit";

                        moveTo -= directionToLookAt * (_spriteRenderer.bounds.size.y / 2.0f);
                    }

                    StartCoroutine(WaitBeforeMove());
                    
                    return;
                }
            }
        }
    }

    IEnumerator WaitBeforeMove()
    {
        move = false;
        yield return new WaitForSeconds(SecondsBeforeMove);
        move = true;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (!DisplayGizmos)
            return;

        if (_directionsToLookAt.Length == 0)
            _InitDirections();

        foreach (Vector3 directionToLookAt in _directionsToLookAt)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                transform.position,
                transform.position + directionToLookAt * DetectionRange
            );
        }
    }

    #endregion
}
