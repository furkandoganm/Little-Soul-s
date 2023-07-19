using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EnemyStrollingState : EnemyBaseState
{
    private readonly int StrollingBlendTreeHash = Animator.StringToHash("StrollingBlendTree");
    private readonly int StrollingSpeedHash = Animator.StringToHash("StrollingSpeed");
    // private readonly int SuspicionHash = Animator.StringToHash("Suspicion");

    private Vector3 _movement;
    // private Vector3 isRotate;

    public EnemyStrollingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.SetFloat(StrollingSpeedHash, 0.5f);
        StateMachine.Animator.CrossFadeInFixedTime(StrollingBlendTreeHash, StateMachine.CrossFadeDuration);
        // StateMachine.Animator.SetFloat(StrollingSpeedHash, 0.5f, StateMachine.AnimationDampTime, Time.deltaTime);
    }

    public override void Tick(float deltaTime)
    {
        CalculateMovement();

        if (IsInChasingRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        }

        // if (IsInSuspicionRange())
        // {
        //     StateMachine.Animator.CrossFadeInFixedTime(SuspicionHash, StateMachine.CrossFadeDuration);
        //     Rotate(CalculateRotation(), deltaTime);
        //     return;
        // }

        Move(_movement, deltaTime);
        Rotate(_movement.normalized, deltaTime);
    }

    private void CalculateMovement()
    {
        _movement = (StateMachine.TargetLocation - StateMachine.transform.position).normalized *
                    StateMachine.StrollingSpeed;
    }

    // private Vector3 CalculateRotation()
    // {
    //     return (StateMachine.Player.transform.position - StateMachine.transform.position).normalized;
    // }

    public override void Exit()
    {
        StateMachine.Animator.CrossFadeInFixedTime(StrollingSpeedHash, StateMachine.CrossFadeDuration);
        StateMachine.Animator.SetFloat(StrollingSpeedHash, 1f, StateMachine.AnimationDampTime, Time.deltaTime);
    }
}