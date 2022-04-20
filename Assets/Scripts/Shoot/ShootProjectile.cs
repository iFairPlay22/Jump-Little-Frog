using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    bool LinearMovement = true;

    [SerializeField]
    [Range(0f, 100f)]
    float LinearSpeed = 50f;

    void Reset()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Collider2D>().isTrigger = true;
        FindObjectOfType<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LinearMovement)
            transform.Translate(transform.right * transform.localScale.x * LinearSpeed * Time.fixedDeltaTime);
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
