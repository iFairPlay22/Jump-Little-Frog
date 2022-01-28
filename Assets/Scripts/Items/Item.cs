using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum ItemTypes { Static, Consumable };

    [Header("Type")]

    [SerializeField]
    string ItemName;

    [SerializeField]
    string ItemDescription;

    [SerializeField]
    ItemTypes ItemType;

    [SerializeField]
    UnityEvent ConsumeEvent;

    public enum InteractionTypes { NONE, PickUp };

    [Header("Interactions")]

    [SerializeField]
    InteractionTypes InteractionType;

    [SerializeField]
    UnityEvent CustumInteractionEvent;

    //[SerializeField]
    private void Reset()
    {
        // Set the default values of the component
        GetComponent<Collider2D>().isTrigger = true; 
        gameObject.layer = LayerMask.NameToLayer("Item");
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemDescription()
    {
        return ItemDescription;
    }

    public void Interact()
    {
        switch (InteractionType)
        {
            case InteractionTypes.NONE:
                break;
            case InteractionTypes.PickUp:
                
                // Add to the inventory and desable it afterwards  
                FindObjectOfType<InventorySystem>().PickUpItem(this.gameObject);
                this.gameObject.SetActive(false);
                
                break;
        }

        CustumInteractionEvent.Invoke();
    }

    public void Consume()
    {
        if (ItemType == ItemTypes.Consumable)
        {
            ConsumeEvent.Invoke();
            FindObjectOfType<InventorySystem>().RemoveItem(gameObject);
        }
    }
}
