using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallingStateHash = Animator.StringToHash("FallingState");

    private Vector3 _movement;
    private float fallingHeight;

    private bool isBusy;

    public static PlayerFallingState Instance;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.JumpEvent += Roll;
        StateMachine.Animator.SetInteger(FallingStateHash, 1);
        // StateMachine.ForceReceiver.currentGravity = StateMachine.ForceReceiver.fallingGravity;
        CalculateMovement();

        isBusy = false;

        Instance = this;
    }

    public override void Tick(float deltaTime)
    {
        Fall(true);
        if (!isBusy)
        {
            Move(_movement, deltaTime);
            UpdateAnimator(deltaTime);
        }

        Move(StateMachine._movement, deltaTime);

        if (fallingHeight < raycastHit.distance)
            fallingHeight = raycastHit.distance;

        StateMachine.ForceReceiver.currentGravity = Mathf.Lerp(StateMachine.ForceReceiver.fallingGravity,
            StateMachine.ForceReceiver.maxGravity, (fallingHeight - raycastHit.distance) / fallingHeight);
    }

    private void CalculateMovement()
    {
        _movement = StateMachine.transform.forward;
    }

    private void UpdateAnimator(float deltaTime)
    {
        // Debug.Log(raycastHit.distance);
        if (raycastHit.distance <= StateMachine.SlowingDistance)
        {
            StateMachine._movement = Vector3.zero;
            if (fallingHeight >= StateMachine.DamageDistance)
            {
                StateMachine.Animator.SetInteger(FallingStateHash, 3);
            }
            else
            {
                StateMachine.Animator.SetInteger(FallingStateHash, 2);
            }

            isBusy = true;
        }
    }

    private void Roll()
    {
        if (raycastHit.distance <= StateMachine.SlowingDistance * 1.5f)
        {
            StateMachine.Animator.SetInteger(FallingStateHash, 4);
            if (!isBusy)
                StateMachine._movement = StateMachine.transform.forward;
            isBusy = true;
            // StateMachine.StoreCalculator();
            // _movement = StateMachine.transform.forward * StateMachine.RollingSpeed;
        }
    }

    public override void Exit()
    {
        StateMachine.InputReader.JumpEvent -= Roll;
        StateMachine.Animator.SetInteger(FallingStateHash, 0);
        // StateMachine.ForceReceiver.currentGravity = StateMachine.ForceReceiver.maxGravity;
    }
}