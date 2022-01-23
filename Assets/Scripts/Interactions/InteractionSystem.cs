using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    #region Variables

    [Header("Detection")]

    [SerializeField]
    Transform detectionPoint;

    [SerializeField]
    private float detectionRadius = 0.02f;

    [SerializeField]
    LayerMask detectionLayer;

    [Header("Debug")]

    [SerializeField]
    bool displayGizmos = true;

    #endregion

    #region Methods

    void Update()
    {
        if (InteractInput() && DetectObject())
                Debug.Log("Interaction!");
    }

    private void OnDrawGizmos()
    {
        if (displayGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
    }

    #endregion
}
