using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShootProjectile : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 100f)]
    float projectileSpeed = 1f;

    void Reset()
    {
        gameObject.SetActive(false);
        FindObjectOfType<BoxCollider2D>().isTrigger = true;
        FindObjectOfType<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(transform.right * transform.localScale.x * projectileSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("here");
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
