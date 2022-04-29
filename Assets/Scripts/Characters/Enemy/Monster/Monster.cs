using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Monster : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    int Life = 5;
    bool _vulnerable = false;

    [SerializeField]
    [Range(0f, 5f)]
    public float IdleTime;

    Transform _target;
    Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    public void Move(Vector3 destination, float speed)
    {
        // Follow the Target
        Vector3 current = transform.position;
        Vector3 next = Vector3.MoveTowards(current, destination, speed * Time.fixedDeltaTime);
        next = new Vector3(next.x, current.y, current.z);
        transform.position = next;

        _LookAtTarget();
    }

    void _LookAtTarget()
    {
        transform.localScale = new Vector3(_target.position.x > transform.position.x ? -1 : 1, 1, 1);
    }

    public void IdleToRandomAttack()
    {
        _ResetTriggers();

        // Trigger action
        float randomState = Random.Range(0, 4);
        switch (randomState)
        {
            case 0:
                _animator.SetTrigger("TakeArm");
                break;
            case 1:
                _animator.SetTrigger("UpAttack");
                break;
            case 2:
                _animator.SetTrigger("RightAttack");
                break;
            case 3:
                _animator.SetTrigger("MiddleAttack");
                break;
            default:
                Debug.LogError("Attaque non gérée !");
                break;
        }
    }

    public void IdleWithArmToAttack()
    {
        _ResetTriggers();

        _animator.SetTrigger("BottomArmAttack");
    }

    void _ResetTriggers()
    {
        _animator.ResetTrigger("TakeArm");
        _animator.ResetTrigger("BottomArmAttack");
        _animator.ResetTrigger("BottomArmAttack");
        _animator.ResetTrigger("UpAttack");
        _animator.ResetTrigger("RightAttack");
        _animator.ResetTrigger("MiddleAttack");
    }

    public void SetVulnerable(bool v)
    {
        _vulnerable = v;
    }

    private void LooseLife()
    {
        if (_vulnerable || Life == 0)
            return;

        Life = Mathf.Max(Life - 1, 0);

        if (Life != 0)
            _Hit();
        else
            _Die();
    }

    void _Hit()
    {
        _animator.SetTrigger("Hit");
    }

    void _Die()
    {
        _animator.SetTrigger("Die");
    }
}
