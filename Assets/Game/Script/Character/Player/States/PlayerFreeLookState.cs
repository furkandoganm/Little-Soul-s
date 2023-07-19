using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerFreeLookState : PlayerBaseState
{
    private Vector3 _movement;
    private float _verticalVelocity;
    private bool HeightEnough;
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    // private readonly int RollHash = Animator.StringToHash("Roll");
    private readonly int IsRollHash = Animator.StringToHash("IsRoll");

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.InputReader.JumpEvent += Roll;

        StateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, StateMachine.CrossFadeDuration);
        // StateMachine.Animator.Play(FreeLookBlendTreeHash);
        // StateMachine.PlayerVFXManager.UpdateFootStep(true);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.InputReader.IsAttacking)
        {
            StateMachine.SwitchState(new PlayerSingleAttackingState(StateMachine, 0));
            return;
        }
        
        if (StateMachine.IsRoll)
        {
            CalculateMovement(StateMachine.transform.forward * StateMachine.RollingSpeed);
            Move(_movement, deltaTime);
            Rotate(_movement, deltaTime);
            return;
        }

        CalculateMovement();

        if (_movement != Vector3.zero)
            Rotate(_movement, deltaTime);

        UpdateAnimation(deltaTime);

        // FaceMovementDirection(deltaTime);

        Fall(false);
    }

    private void CalculateMovement(Vector3 specificMovement = default(Vector3))
    {
        // if (_verticalVelocity >= StateMachine.MaxVerticalVelocity)
        //     _verticalVelocity += StateMachine.Gravity * deltaTime;
        //
        // if (!StateMachine.CharacterController.isGrounded)
        // {
        //     // if (Physics.Raycast(StateMachine.GroundPoint.position, -Vector3.up, out var hit))
        //     // {
        //     //     _movement += _verticalVelocity * Vector3.up;
        //     //     
        //     //     if (hit.distance > 0.8f)
        //     //     {
        //     //         StateMachine.Animator.SetInteger(FallingStateHash, 1);
        //     //         if (hit.distance >= StateMachine.ImpactHeight)
        //     //             HeightEnough = true;
        //     //     }
        //     //     else
        //     //     {
        //     //         if (HeightEnough)
        //     //         {
        //     //             StateMachine.Animator.SetInteger(FallingStateHash, 4);
        //     //             HeightEnough = false;
        //     //         }
        //     //         else
        //     //         {
        //     //             StateMachine.Animator.SetInteger(FallingStateHash, 3);
        //     //         }
        //     //         
        //     //         StateMachine.InputReader.JumpEvent += Somersault;
        //     //     }
        //     // }
        //     // _movement = StateMachine.Gravity * Vector3.up;
        // }
        // else
        // {
        //     _movement = StateMachine.Gravity * 0.1f  * Vector3.up;
        // }

        if (specificMovement != Vector3.zero)
        {
            _movement = specificMovement;
            return;
        }

        _movement.x = StateMachine.InputReader.MovementValue.x;
        _movement.z = StateMachine.InputReader.MovementValue.y;
    }

    // private void FaceMovementDirection(float deltaTime)
    // {
    //     StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,
    //         Quaternion.LookRotation(_movement), deltaTime * StateMachine.RotationDamping);
    // }

    // private void Somersault()
    // {
    //     StateMachine.Animator.SetInteger(FallingStateHash, 2);
    // }

    private void OnTarget()
    {
        if (!StateMachine.Targeting.SelectTarget()) return;
        if (StateMachine.Targeting.targets.Count < 2)
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
        else
            StateMachine.SwitchState(new PlayerMultiTargetingState(StateMachine));
    }

    private void UpdateAnimation(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f, StateMachine.AnimationDampTime, deltaTime);
            return;
        }

        if (StateMachine.InputReader.IsSlow)
        {
            // StateMachine.CharacterController.Move(_movement / 2 * (StateMachine.FreeLookMovementSpeed * deltaTime));
            Move((_movement / StateMachine.FreeLookMovementCoefficient) * StateMachine.FreeLookMovementSpeed,
                deltaTime);
            StateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f / StateMachine.FreeLookMovementCoefficient,
                StateMachine.AnimationDampTime, deltaTime);
        }
        else
        {
            // StateMachine.CharacterController.Move(_movement * (StateMachine.FreeLookMovementSpeed * deltaTime));
            Move(_movement * StateMachine.FreeLookMovementSpeed, deltaTime);
            StateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, StateMachine.AnimationDampTime, deltaTime);
        }
    }

    private void Roll()
    {
        StateMachine.Animator.SetBool(IsRollHash, true);
        // StateMachine.Animator.Play(RollHash);
        // StateMachine.IsRoll = true;
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= OnTarget;
        StateMachine.InputReader.JumpEvent -= Roll;
        // StateMachine.PlayerVFXManager.UpdateFootStep(false);
    }
}