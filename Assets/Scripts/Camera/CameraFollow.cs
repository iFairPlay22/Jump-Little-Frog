using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    [Range(1, 10)]
    float smootFactor = 1;

    [SerializeField]
    Vector2 minValues, maxValues;

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        Vector3 currentPosition = transform.position;
        
        Vector3 targetPosition = target.position + offset;
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x), 
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y), 
            targetPosition.z
        );

        Vector3 smoothedPosition = Vector3.Lerp(currentPosition, targetPosition, smootFactor * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }

}
