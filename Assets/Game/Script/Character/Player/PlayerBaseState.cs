using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine StateMachine;
    protected Ray ray;
    protected RaycastHit raycastHit;
    protected LayerMask layerMask;
    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Rotate(Vector3 motion, float deltaTime)
    {
        StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,
            Quaternion.LookRotation(motion), deltaTime * StateMachine.RotationDamping);
    }

    protected void Fall(bool isFalling)
    {
        ray = new Ray(StateMachine.GroundPoint.position, -StateMachine.GroundPoint.up * 20f);
        foreach (LayerMask layerToHit in StateMachine.LayersToHit)
        {
            // Physics.Raycast(ray, out raycastHit, 50f, layerToHit);
            if (Physics.Raycast(ray, out raycastHit, 50f, layerToHit) && raycastHit.distance > 5f)
            {
                layerMask = layerToHit;
                Debug.DrawRay(StateMachine.GroundPoint.position, -StateMachine.GroundPoint.up, Color.green);
                if (!isFalling)
                    StateMachine.SwitchState(new PlayerFallingState(StateMachine));
            }
        }
    }
}