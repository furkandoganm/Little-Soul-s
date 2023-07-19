using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMultiTargetingState : PlayerBaseState
{
    private Vector3 _movement;

    private readonly int MultiTargetingBlendTreeHash = Animator.StringToHash("MultiTargetingBlendTree");
    private readonly int MultiTargetingSpeedHash = Animator.StringToHash("MultiTargetingSpeed");
    
    public PlayerMultiTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.TargetEvent += SunderTarget;
        StateMachine.Animator.CrossFadeInFixedTime(MultiTargetingBlendTreeHash, StateMachine.CrossFadeDuration);
        StateMachine.TimeManager.DoSlowMotion();
        VFXManaging(true);
    }

    public override void Tick(float deltaTime)
    {
        CalculateMovement();
        UpdateAnimation(deltaTime);
        Rotate(_movement, deltaTime);
    }

    private void SunderTarget()
    {
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }

    private void CalculateMovement()
    {
        _movement.x = StateMachine.InputReader.MovementValue.x;
        _movement.z = StateMachine.InputReader.MovementValue.y;
        _movement *= StateMachine.MultiTargetingSpeed;
    }

    private void UpdateAnimation(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(MultiTargetingSpeedHash, 0f, StateMachine.AnimationDampTime, deltaTime);
        }
        else
        {
            StateMachine.Animator.SetFloat(MultiTargetingSpeedHash, 1f, StateMachine.AnimationDampTime, deltaTime);
            Move(_movement, deltaTime);
        }
    }

    private void VFXManaging(bool isTargeted)
    {
        foreach (Target target in StateMachine.Targeting.targets)
        {
            target.GetComponent<EnemyVFXManager>().Target(isTargeted);
        }
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= SunderTarget;
        StateMachine.TimeManager.BreakSlowMotion();
        VFXManaging(false);
    }
}
