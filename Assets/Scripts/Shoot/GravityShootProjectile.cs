using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GravityShootProjectile : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 5f)]
    float projectileSpeed = 3f;

    [SerializeField]
    [Range(1f, 5f)]
    float RotationSpeed = 2f;

    Cannon cannon;

    public void SetCannon(Cannon c)
    {
        cannon = c;
    }

    float fTime = 0f;

    void Reset()
    {
        gameObject.SetActive(false);
        FindObjectOfType<Collider2D>().isTrigger = true;
        FindObjectOfType<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fTime += Time.fixedDeltaTime * projectileSpeed;
        transform.position = cannon.GetPosition(fTime);

        transform.Rotate(new Vector3(RotationSpeed * Time.fixedDeltaTime, 0, 0));
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
