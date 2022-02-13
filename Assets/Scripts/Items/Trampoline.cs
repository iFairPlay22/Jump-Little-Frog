using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Trampoline : InteractionObject
{
    [SerializeField]
    [Range(400f, 600f)]
    float _propulsionPower = 500f;

    [SerializeField]
    [Range(1f, 5f)]
    float _animationTime = 2f;

    [SerializeField]
    Player _player;

    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("On", false);
        _animator.SetFloat("AnimationTime", _animationTime);
    }

    protected override void OnCollisionWithPlayer(Collision2D collision)
    {
        StartCoroutine(TrampolineOn());
    }

    IEnumerator TrampolineOn()
    {
        _animator.SetBool("On", true);
        _player.Propulse(_propulsionPower);
        yield return new WaitForSeconds(1f / _animationTime);
        _animator.SetBool("On", false);
    }
}
