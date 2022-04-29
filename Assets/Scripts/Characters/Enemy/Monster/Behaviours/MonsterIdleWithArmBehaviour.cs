using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleWithArmBehaviour : StateMachineBehaviour
{
    [SerializeField]
    float TimeToStay = 10;
    float _remainingTime = 0;
    bool _wait = true;

    Monster _monster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monster = animator.GetComponent<Monster>();
        _remainingTime = TimeToStay;
        _wait = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_wait)
            return;

        _remainingTime -= Time.deltaTime;
        if (_remainingTime < 0)
        {
            _monster.IdleWithArmToAttack();
            _wait = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}