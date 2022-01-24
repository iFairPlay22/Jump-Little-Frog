using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp };

    [SerializeField]
    InteractionType interactionType;
    
    [SerializeField]
    UnityEvent custumEvent;

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
                
                // Add to the inventory and desable it afterwards  
                FindObjectOfType<InventorySystem>().PickUpItem(this.gameObject);
                this.gameObject.SetActive(false);
                
                break;
        }

        custumEvent.Invoke();
    }
}
