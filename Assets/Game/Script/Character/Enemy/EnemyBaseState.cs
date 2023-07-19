using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
// using System.Numerics;
using UnityEngine.EventSystems;
// using Vector3 = UnityEngine.Vector3;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine StateMachine;
    // private Vector3 currentVelocity;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected void Move(Vector3 position)
    {
        
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        // StateMachine.CharacterController.Move(motion* deltaTime);
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Rotate(Vector3 rotation, float deltaTime)
    {
        // StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,
        //     Quaternion.LookRotation(rotation), deltaTime * StateMachine.RotationDamping);

        // StateMachine.transform.rotation = Quaternion.LookRotation(rotation);
        StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,
            Quaternion.LookRotation(rotation), StateMachine.RotationDamping);
    }

    protected bool IsInChasingRange()
    {
        if ((StateMachine.PlayerChasingRange * StateMachine.PlayerChasingRange) >=
            (StateMachine.Player.transform.position - StateMachine.transform.position).sqrMagnitude)
        {
            return true;
        }

        return false;
    }
    protected bool IsInSuspicionRange()
    {
        if ((StateMachine.PlayerChasingRange * StateMachine.PlayerChasingRange) * 1.2f >=
            (StateMachine.Player.transform.position - StateMachine.transform.position).sqrMagnitude)
        {
            return true;
        }

        return false;
    }
}