using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField, Header("Scripts"), Space]
    public InputReader InputReader { get; private set; }

    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public TimeManager TimeManager { get; private set; }
    [field: SerializeField] public Targeting Targeting { get; private set; }
    // [field: SerializeField] public Camera CloseCamera { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

    [field: SerializeField, Header("Object"), Space]
    public Transform GroundPoint { get; private set; }

    [field: SerializeField, Header("Free Look State"), Space]
    public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public float FreeLookMovementCoefficient { get; private set; }

    [field: SerializeField, Header("Targeting State"), Space]
    public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField] public float TargetingMovementCoefficient { get; private set; }

    // [field: SerializeField] public float Gravity { get; private set; }
    [field: SerializeField, Header("Common Values"), Space]
    public float RotationDamping { get; private set; }

    [field: SerializeField] public float AnimationDampTime { get; private set; }
    [field: SerializeField] public float CrossFadeDuration { get; private set; }

    [field: SerializeField, Header("Falling State"), Space]
    public float DamageDistance { get; private set; }

    [field: SerializeField] public Vector2 FallingMovementValues { get; private set; }

    [field: SerializeField] public List<LayerMask> LayersToHit { get; private set; }

    // [field: SerializeField] public LayerMask LayerToHit { get; private set; }
    [field: SerializeField] public float SlowingDistance { get; private set; }
    [field: SerializeField] public float RollingSpeed { get; private set; }
    [field: SerializeField] public float RollingDampTime { get; private set; }

    [field: SerializeField, Header("VFX"), Space]
    public PlayerVFXManager PlayerVFXManager { get; private set; }

    [field: SerializeField, Header("MultiTargeting"), Space]
    public float MultiTargetingSpeed { get; private set; }

    // [field: SerializeField] public float MultiTargetingRotationDampTime { get; private set; }

    [field: SerializeField, Header("MultiTargeting"), Space]
    public Attack[] Attacks { get; private set; }

    protected Camera MainCamera;

    public bool IsRoll;
    public bool IsTargeted;

    public Vector3 _movement;

    private void Awake()
    {
        MainCamera = Camera.main;
        IsRoll = false;
    }

    private void Start()
    {
        SwitchState(new PlayerFreeLookState(this));
    }
}