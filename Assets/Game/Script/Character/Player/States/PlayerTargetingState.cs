using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Rendering.UI;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    private readonly int IsRollHash = Animator.StringToHash("IsRoll");

    private Target target;

    private Vector3 _movement;
    private Vector3 _direction;

    // private bool isPassing;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, StateMachine.CrossFadeDuration);
        // StateMachine.Animator.Play(TargetingBlendTreeHash);
        StateMachine.InputReader.TargetEvent += SunderTarget;
        StateMachine.InputReader.JumpEvent += Roll;
        // if (StateMachine.Targeting.targets.Count > 1)
        // {
        //     StateMachine.SwitchState(new PlayerMultiTargetingState(StateMachine));
        //     isPassing = true;
        // }

        target = StateMachine.Targeting.currentTarget;
        StateMachine.Targeting.cineTargetingGroup.AddMember(target.transform, 1f, 2f);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.InputReader.IsAttacking)
        {
            StateMachine.IsTargeted = true;
            StateMachine.SwitchState(new PlayerSingleAttackingState(StateMachine, 3));
            return;
        }

        if (StateMachine.Targeting.currentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }

        Fall(false);

        CalculateRotation();
        Rotate(_direction, deltaTime);

        if (StateMachine.IsRoll)
        {
            CalculateMovement(_movement);
            if (StateMachine.InputReader.MovementValue == -Vector2.up)
                _movement = -(StateMachine.transform.forward + StateMachine.transform.right);
            Move(
                (_movement.normalized + _direction.normalized) *
                StateMachine.RollingSpeed, deltaTime);
            return;
        }

        CalculateMovement();
        UpdateAnimations(deltaTime);
    }

    private void SunderTarget()
    {
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }

    private void CalculateMovement(Vector3 specificMovement = default(Vector3))
    {
        // Vector3 forward = StateMachine.CloseCamera.transform.forward;
        // Vector3 right = StateMachine.CloseCamera.transform.right;
        //
        // forward.y = 0f;
        // right.y = 0f;
        //
        // forward.Normalize();
        // right.Normalize();
        // _movement = forward * StateMachine.InputReader.MovementValue.y +
        //             right * StateMachine.InputReader.MovementValue.x;
        // _movement *= StateMachine.TargetingMovementSpeed;

        // if (specificMovement != Vector3.zero)
        // {
        //     _movement = specificMovement;
        //     if (StateMachine.InputReader.MovementValue.x != 0f)
        //         _movement.x += (StateMachine.InputReader.MovementValue.x * 2f);
        //     else
        //         _movement.x += 5f;
        //     return;
        // }

        if (specificMovement != Vector3.zero)
            _movement = specificMovement;

        _movement += StateMachine.transform.right * StateMachine.InputReader.MovementValue.x;
        _movement += StateMachine.transform.forward * StateMachine.InputReader.MovementValue.y;
        _movement *= StateMachine.TargetingMovementSpeed;
    }

    private void CalculateRotation(Vector3 specificMovement = default(Vector3))
    {
        // if (specificMovement != Vector3.zero)
        // {
        //     _direction = _movement;
        //     if (StateMachine.InputReader.MovementValue.x != 0f)
        //         _direction.x += (StateMachine.InputReader.MovementValue.x * 2f);
        //     else
        //         _direction.x += 2f;
        //     return;
        // }

        _direction = target.transform.position - StateMachine.transform.position;
        _direction.y = 0f;
    }

    private void UpdateAnimations(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(TargetingForwardHash, 0f, StateMachine.AnimationDampTime, deltaTime);
            StateMachine.Animator.SetFloat(TargetingRightHash, 0f, StateMachine.AnimationDampTime, deltaTime);
            return;
        }

        if (StateMachine.InputReader.IsSlow)
        {
            // StateMachine.CharacterController.Move(_movement / 2 * (StateMachine.FreeLookMovementSpeed * deltaTime));
            Move((_movement / StateMachine.TargetingMovementCoefficient) * StateMachine.TargetingMovementSpeed,
                deltaTime);
            StateMachine.Animator.SetFloat(TargetingForwardHash,
                StateMachine.InputReader.MovementValue.x / StateMachine.TargetingMovementCoefficient,
                StateMachine.AnimationDampTime, deltaTime);
            StateMachine.Animator.SetFloat(TargetingRightHash,
                StateMachine.InputReader.MovementValue.y / StateMachine.TargetingMovementCoefficient,
                StateMachine.AnimationDampTime, deltaTime);
        }
        else
        {
            // StateMachine.CharacterController.Move(_movement * (StateMachine.FreeLookMovementSpeed * deltaTime));
            Move(_movement * StateMachine.TargetingMovementSpeed, deltaTime);
            StateMachine.Animator.SetFloat(TargetingForwardHash, StateMachine.InputReader.MovementValue.x,
                StateMachine.AnimationDampTime, deltaTime);
            StateMachine.Animator.SetFloat(TargetingRightHash, StateMachine.InputReader.MovementValue.y,
                StateMachine.AnimationDampTime, deltaTime);
        }

        /*
         * if (StateMachine.InputReader.MovementValue.y == 0)
         * {
         *  StateMachine.Animator.SetFloat(TargetingForwardHash, 0)
         * }
         * else
         * {
         *  float value = StateMachine.InputReader.MovementValue.y > 0? 1f: -1f;
         *  StateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
         * }
         * 
         * if (StateMachine.InputReader.MovementValue.x == 0)
         * {
         *  StateMachine.Animator.SetFloat(TargetingRightHash, 0)
         * }
         * else
         * {
         *  float value = StateMachine.InputReader.MovementValue.x > 0? 1f: -1f;
         *  StateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
         * }
         */
    }

    public void Roll()
    {
        StateMachine.Animator.SetBool(IsRollHash, true);
    }

    public override void Exit()
    {
        StateMachine.Targeting.cineTargetingGroup.RemoveMember(target.transform);
        StateMachine.InputReader.TargetEvent -= SunderTarget;
        StateMachine.InputReader.JumpEvent += Roll;
    }
}