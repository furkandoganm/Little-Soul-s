using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField, Header("Scripts"), Space]
    public Animator Animator { get; private set; }

    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField, Header("Common Values"), Space]
    public float CrossFadeDuration { get; private set; }

    [field: SerializeField] public float AnimationDampTime { get; private set; }
    [field: SerializeField] public float StrollingSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public Vector3 MaxScale { get; private set; }


    public GameObject Player { get; private set; }

    public Vector3 TargetLocation;
    public Vector3 Rotation;
    // public Vector3 Rotation { get; set; }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Rotation = transform.forward;
        // NavMeshAgent.speed = StrollingSpeed;

        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updatePosition = true;

        SwitchState(new EnemyStrollingState(this));
    }
}