using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine };

    [SerializeField]
    InteractionType interactionType;

    //[SerializeField]
    private void Reset()
    {
        // Set the default values of the component
        GetComponent<Collider2D>().isTrigger = true; 
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    public void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.NONE:
                break;
            case InteractionType.PickUp:
                Debug.Log("PickUp");
                break;
            case InteractionType.Examine:
                Debug.Log("Examine");
                break;
        }
    }
}
