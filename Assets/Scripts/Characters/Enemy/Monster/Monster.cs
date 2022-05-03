using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Monster : MonoBehaviour
{
    #region Variables
    [Header("General")]
    [SerializeField]
    [Range(1, 10)]
    int Life = 5;
    bool _vulnerable = false;

    [Header("Meteor rain")]
    [SerializeField]
    ParticleSystem MeteorRainPrefab;

    [SerializeField]
    Transform MeteorRainSummonPoint;

    Transform _target;
    Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
    #endregion

    #region Meteor Rain
    public GameObject SummonMeteorRain()
    {
        // Create projectile
        GameObject meteorRain = Instantiate(MeteorRainPrefab.gameObject);
        meteorRain.transform.SetParent(MeteorRainSummonPoint.transform);
        meteorRain.SetActive(true);

        return meteorRain;
    }

    #endregion

    #region Movement
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
    #endregion

    #region States
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
                _animator.SetTrigger("StartUpAttack");
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
    #endregion

    #region Life

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
    #endregion
}
