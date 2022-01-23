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
    
    GameObject _detectedObject;

    [Header("Debug")]

    [SerializeField]
    bool displayGizmos = true;

    #endregion

    #region Methods

    void Update()
    {
        if (InteractInput() && DetectObject())
        {
            Item item = _detectedObject.GetComponent<Item>();
            if (item)
            {
                item.Interact();
            }
        }
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
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if (obj != null)
        {
            _detectedObject = obj.gameObject;
        }

        return obj;
    }

    #endregion
}
