using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    [SerializeField]
    [Range(10f, 100f)]
    float Speed = 50.0f;

    [SerializeField]
    [Range(10f, 100f)]
    float DetectionRange = 5.0f;

    Vector3[] _directionsToLookAt = {};
    Vector3? moveTo;

    private void Awake()
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
        if (moveTo == null)
        {
            // On n'a pas détecté de collision, on ne bouge pas
            _UpdateMoveTo();
        }

        if (moveTo != null)
        {
            // On a précedamment détecté une collision, on bouge
            _Move();
        }
    }

    private void _Move()
    {
        if (moveTo == null)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, moveTo.Value, Time.fixedDeltaTime * Speed);

        if (Mathf.Abs(Vector3.Distance(currentPosition, nextPosition)) <= 0.3f)
        {
            nextPosition = moveTo.Value;
            moveTo = null;
        }

        transform.position = nextPosition;
    }

    private void _UpdateMoveTo()
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
                    moveTo = groundHit.point;
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
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
}
