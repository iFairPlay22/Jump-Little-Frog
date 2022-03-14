using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(Animator))]
public class FirePlant : MonoBehaviour
{
    [SerializeField]
    [Range(1.0f, 5.0f)]
    float SecondsBetweenAttacks = 2.0f;

    [SerializeField]
    [Range(1.0f, 5.0f)]
    float AttackAnimationTime = 1.0f;

    Animator _animator;
    Shooter _shooter;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("AttackAnimationTime", AttackAnimationTime);

        _shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        StartCoroutine(_Attack());
    }

    IEnumerator _Attack()
    {
        yield return new WaitForSeconds(SecondsBetweenAttacks);
        _animator.Play("FirePlant_attack");

        yield return new WaitForSeconds(1.0f / AttackAnimationTime);
        _shooter._Shoot();

        StartCoroutine(_Attack());
    }
}
