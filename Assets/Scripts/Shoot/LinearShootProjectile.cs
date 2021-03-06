using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LinearShootProjectile : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 100f)]
    float Speed = 3f;

    void Reset()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Collider2D>().isTrigger = true;
        FindObjectOfType<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(transform.right * transform.localScale.x * Speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ShootVictim victim = collision.gameObject.GetComponent<ShootVictim>();
        if (victim)
        {
            victim.OnAction();
        }

        Destroy(gameObject);
    }
}
