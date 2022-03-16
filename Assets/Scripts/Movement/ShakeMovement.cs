using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 2f)]
    float ShakeEverySeconds = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    float MaxMoveRange = 0.05f;

    [SerializeField]
    [Range(0f, 1f)]
    float ShakeRange = 0.1f;

    [SerializeField]
    bool DisplayGizmos = true;

    Vector3 _initialPosition;

    void Start()
    {
        _SaveCurrentPosition();
        StartCoroutine(_Shake());
    }

    void _SaveCurrentPosition()
    {
        _initialPosition = transform.localPosition;
    }

    IEnumerator _Shake()
    {
        transform.localPosition = _GenerateRandomCoordsAround();
        yield return new WaitForSeconds(ShakeEverySeconds);
        StartCoroutine(_Shake());
    }

    Vector3 _GenerateRandomCoordsAround()
    {
        Vector3 destination = RandomStaticMethods.GenerateRandomPointIn2dCircle(
            _initialPosition, ShakeRange
        );

        return Vector3.MoveTowards(transform.localPosition, destination, MaxMoveRange);
    }

    void OnDrawGizmos()
    {
        if (DisplayGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, ShakeRange);
        }
    }
}
