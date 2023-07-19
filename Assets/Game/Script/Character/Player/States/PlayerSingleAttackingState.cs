using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSingleAttackingState : PlayerBaseState
{
    private Attack Attack;

    private float previousFrameTime;
    private Vector3 _movement;
    private bool alreadyAppliedForce;

    public PlayerSingleAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        Attack = StateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        StateMachine.WeaponDamage.SetAttack(Attack.Damage);
        StateMachine.Animator.CrossFadeInFixedTime(Attack.AnimationName, Attack.AnimationDampTime);
        alreadyAppliedForce = false;
    }

    public override void Tick(float deltaTime)
    {
        // CalculateMovement();
        Move(deltaTime);
        if (StateMachine.IsTargeted)
            Rotate(-(StateMachine.transform.position - StateMachine.Targeting.currentTarget.transform.position).normalized, deltaTime);

        float normalizedTime = GetNormalizeTime();

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= Attack.ForceTime)
            {
                TryApplyForce();
            }

            if (StateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (StateMachine.IsTargeted)
            {
                StateMachine.IsTargeted = false;
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
            else
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    private float GetNormalizeTime()
    {
        AnimatorStateInfo currentInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = StateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (StateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!StateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (Attack.ComboStateIndex == -1) return;

        if (normalizedTime < Attack.ComboAttackTime) return;

        StateMachine.SwitchState(
            new PlayerSingleAttackingState(
                StateMachine,
                Attack.ComboStateIndex
            )
        );
    }

    // private void CalculateMovement()
    // {
    //     _movement = StateMachine.transform.forward * Attack.AttackSpeed;
    // }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        StateMachine.ForceReceiver.AddImpact(StateMachine.transform.forward * Attack.Force);
        alreadyAppliedForce = true;
    }

    public override void Exit()
    {
    }
}