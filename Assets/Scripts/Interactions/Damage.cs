using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Damage : InteractionObject
{
    [SerializeField]
    int damages = 1;

    [SerializeField]
    bool kill = false;

    protected override void OnCollisionWithPlayer(Collision2D collision)
    {
        if (collision == null || collision.rigidbody == null) return;

        if (collision.rigidbody.tag == "Player")
        {
            if (kill)
            {
                FindObjectOfType<PlayerHealth>().DestroyHealth();
            } else
            {
                FindObjectOfType<PlayerHealth>().ReduceHealth(damages);
            }
        }
    }


}
