using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RegularMovement))]
public class FlyingPlatform : InteractionObject
{

    [SerializeField]
    [Range(0, 5f)]
    float _fallAfterSeconds = 2f;

    [SerializeField]
    [Range(5, 20f)]
    float _fallingSpeed = 10f;
    bool _fall = false;

    [SerializeField]
    [Range(0, 5f)]
    float _destroyAfterSeconds = 3f;

    protected override void OnCollisionWithPlayer(Collision2D collision)
    {
        if (!_fall)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallAfterSeconds);

        _fall = true;
        GetComponent<RegularMovement>().Stop();
        
        yield return new WaitForSeconds(_destroyAfterSeconds);

        Destroy(gameObject.transform.parent.gameObject);
    }

    void FixedUpdate()
    {
        if (_fall)
        {
            transform.position -= Vector3.up * _fallingSpeed * Time.fixedDeltaTime;
        }
    }
}
