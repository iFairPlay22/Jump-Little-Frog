using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class InteractionObject : MonoBehaviour
{
    public virtual void Reset()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<Rigidbody2D>().freezeRotation = true; 
        GetComponent<Collider2D>().isTrigger = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.rigidbody == null) return;

        if (collision.rigidbody.tag == "Player")
        {
            OnCollisionWithPlayer(collision);
        }
    }

    protected virtual void OnCollisionWithPlayer(Collision2D collision)
    {
        
    }
}
