using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(RegularMovement))]
public class FlyingPlatform : MonoBehaviour
{
    [Header("Movement on fall")]
    [SerializeField]
    [Range(0, 3f)]
    float _fallingSpeed = 2.0f;

    [SerializeField]
    [Range(0, 5f)]
    float _fallingTimeBeforeDestroy = 3f;

    [Header("Propeller on fall")]

    [SerializeField]
    [Range(0, 0.5f)]
    float _propellerSpeedDiff = 0.1f;

    [SerializeField]
    [Range(0, 0.5f)]
    float _propellerTimeDiff = 0.2f;

    [SerializeField]
    [Range(0, 0.5f)]
    float _propellerMinSpeed = 0.3f;

    float _propellerSpeed = 1f;
    bool _on = true;
    Animator _animator;

    private void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("PropellerSpeed", _propellerSpeed);
    }

    void FixedUpdate()
    {
        if (!_on)
        {
            transform.position -= Vector3.up * _fallingSpeed * 0.1f * Time.fixedTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_on && collision.rigidbody.tag == "Player")
        {
            StartCoroutine(ReducePropellerSpeed());
        }
    }

    IEnumerator ReducePropellerSpeed()
    {
        _propellerSpeed = Mathf.Max(0.1f, _propellerSpeed - _propellerSpeedDiff);
        _animator.SetFloat("PropellerSpeed", _propellerSpeed);

        if (_propellerSpeed <= _propellerMinSpeed)
        {
            StartCoroutine(Fall());
        } else
        {
            yield return new WaitForSeconds(_propellerTimeDiff);
            StartCoroutine(ReducePropellerSpeed());
        }

    }

    IEnumerator Fall()
    {
        
        _on = false;
         GetComponent<RegularMovement>().Stop();
        
        yield return new WaitForSeconds(_fallingTimeBeforeDestroy);

        Destroy(gameObject.transform.parent.gameObject);
    }
}
