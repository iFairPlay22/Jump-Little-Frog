using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RaycastDetection : MonoBehaviour
{
    #region Serializable Fields

    [Header("Detection")]

    [SerializeField]
    [Range(10f, 100f)]
    float DetectionRange = 5.0f;

    [SerializeField]
    DetectionDirections DetectionDirection;

    enum DetectionDirections
    { RIGHT, LEFT, TOP, BOTTOM, HORIZONTAL, VERTICAL, ALL }

    [Header("Debug")]
    [SerializeField]
    protected bool DisplayGizmos = true;

    #endregion

    #region Private Fields

    Vector3[] _directionsToLookAt = { };

    #endregion

    #region Raycast Detection

    public virtual void Start()
    {
        _InitDirections();
    }

    private void _InitDirections()
    {
        switch (DetectionDirection)
        {
            case DetectionDirections.ALL:
                _directionsToLookAt = new Vector3[4];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.down);
                _directionsToLookAt[1] = transform.TransformDirection(Vector3.up);
                _directionsToLookAt[2] = transform.TransformDirection(Vector3.right);
                _directionsToLookAt[3] = transform.TransformDirection(Vector3.left);
                break;
            case DetectionDirections.HORIZONTAL:
                _directionsToLookAt = new Vector3[2];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.right);
                _directionsToLookAt[1] = transform.TransformDirection(Vector3.left);
                break;
            case DetectionDirections.VERTICAL:
                _directionsToLookAt = new Vector3[2];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.up);
                _directionsToLookAt[1] = transform.TransformDirection(Vector3.down);
                break;
            case DetectionDirections.TOP:
                _directionsToLookAt = new Vector3[1];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.up);
                break;
            case DetectionDirections.BOTTOM:
                _directionsToLookAt = new Vector3[1];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.down);
                break;
            case DetectionDirections.RIGHT:
                _directionsToLookAt = new Vector3[1];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.right);
                break;
            case DetectionDirections.LEFT:
                _directionsToLookAt = new Vector3[1];
                _directionsToLookAt[0] = transform.TransformDirection(Vector3.left);
                break;
            default: 
                Debug.LogError("Cas de figure non géré!");
                break;
        }

        
    }

    public virtual void FixedUpdate()
    {
        if (ShoudDetectRaycastCollisions())
            _MakeDetections();
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
                OnRaycastDetection(groundHit, playerHit, directionToLookAt);
            }
        }
    }

    protected abstract bool ShoudDetectRaycastCollisions();

    protected abstract void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D hitPoint, Vector3 direction);

    #endregion

    #region Debug

    public virtual void OnDrawGizmos()
    {
        if (!DisplayGizmos)
            return;

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
