using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int StrollingSpeedHash = Animator.StringToHash("StrollingSpeed");

    private Vector3 _movement;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.SetFloat(StrollingSpeedHash, 1f, StateMachine.AnimationDampTime, Time.deltaTime);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChasingRange())
        {
            StateMachine.SwitchState(new EnemyStrollingState(StateMachine));
            return;
        }

        if (StateMachine.transform.localScale != StateMachine.MaxScale)
        {
            Vector3.Lerp(StateMachine.transform.localScale
                , StateMachine.MaxScale, deltaTime);   
        }

        StateMachine.Animator.SetFloat(StrollingSpeedHash, 1f, StateMachine.AnimationDampTime, Time.deltaTime);
        CalculateMovement();
        Move(_movement, deltaTime);
        Rotate(_movement.normalized, deltaTime);
    }

    private void CalculateMovement()
    {
        StateMachine.NavMeshAgent.destination = StateMachine.Player.transform.position;
        _movement = StateMachine.NavMeshAgent.desiredVelocity.normalized * StateMachine.MaxSpeed;
        StateMachine.NavMeshAgent.velocity = StateMachine.CharacterController.velocity;
    }

    public override void Exit()
    {
        StateMachine.NavMeshAgent.ResetPath();
        StateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
