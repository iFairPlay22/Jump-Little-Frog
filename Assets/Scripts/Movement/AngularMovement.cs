using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AngularMovement : MonoBehaviour
{
    [SerializeField]
    [Range(25f, 175f)]
    float speed = 50;

    [SerializeField]
    bool circularMovement = false;

    [SerializeField]
    [Range(0, 360)]
    int startAngle = 90;

    [SerializeField]
    [Range(0, 360)]
    int endAngle = 270;
    [SerializeField]

    int _clockWise = 1;

    Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        transform.localRotation = Quaternion.Euler(0, 0, startAngle);
    }

    private float _Sigmoid(float x)
    {
        return Mathf.Clamp(.3f, 1.0f / (1.0f + 5.0f * Mathf.Exp(-1.0f / 7.0f * x)), 1.0f);
    }
    

    void FixedUpdate()
    {
        float currentZ = transform.localRotation.eulerAngles.z;
        float deltaZ = speed * Time.fixedDeltaTime;
        float nextZ;

        if (circularMovement)
        {
            nextZ = currentZ + deltaZ;
        } else
        {
            if (!(startAngle <= currentZ && currentZ <= endAngle))
                _clockWise *= -1;

            nextZ = currentZ + deltaZ * _clockWise;

            int minZDiff = (int) Mathf.Min(nextZ - startAngle, endAngle - nextZ);
            float lerpFactor = _Sigmoid(minZDiff);
            Debug.Log(lerpFactor);
            nextZ = Mathf.Lerp(currentZ, nextZ, lerpFactor);

        }

        transform.localRotation = Quaternion.Euler(0, 0, nextZ);
    }
}
