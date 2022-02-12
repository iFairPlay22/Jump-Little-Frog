using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractionObject
{
    protected override void OnCollisionWithPlayer(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
