using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RegularMovement))]
public class BrownPlant : RaycastDetection
{
    [SerializeField]
    [Range(1.0f, 5.0f)]
    float SecondsBetweenAttacks = 2.0f;

    [SerializeField]
    [Range(1.0f, 5.0f)]
    float AttackAnimationTime = 1.0f;

    bool _TimeToAttack = false;
    Animator _animator;
    Shooter _shooter;
    RegularMovement _regularMovement;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("AttackAnimationTime", AttackAnimationTime);

        _shooter = GetComponent<Shooter>();
        _regularMovement = GetComponent<RegularMovement>();
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(_WaitBeforeNewAttack());
    }

    protected override bool ShoudDetectRaycastCollisions()
    {
        return _TimeToAttack;
    }

    protected override void OnRaycastDetection(RaycastHit2D groundHit, RaycastHit2D hitPoint, Vector3 direction)
    {
        // Rotate the plant
        _regularMovement.Stop();
        transform.localScale = new Vector3(-direction.x, 1, 1);

        // Attack
        StartCoroutine(_Attack());
        StartCoroutine(_WaitBeforeNewAttack());
    }

    IEnumerator _Attack()
    {
        float demiAnimationTime = 0.5f * (1.0f / AttackAnimationTime);

        _TimeToAttack = false;
        _animator.Play("BrownPlant_attack");

        yield return new WaitForSeconds(demiAnimationTime);
        _shooter._Shoot();

        yield return new WaitForSeconds(demiAnimationTime);
        _regularMovement.UnStop();
    }    

    IEnumerator _WaitBeforeNewAttack()
    {
        yield return new WaitForSeconds(SecondsBetweenAttacks);
        _TimeToAttack = true;
    }
}
