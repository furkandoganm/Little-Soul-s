using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ForceReceiver : MonoBehaviour
{
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float Drag { get; private set; }

    private float verticalVelocity;
    public float currentGravity { get; set; }
    private Vector3 impact;
    private Vector3 dampingVelocity;
    
    public float maxGravity;
    public float fallingGravity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Start()
    {
        currentGravity = maxGravity;
    }

    void Update()
    {
        
        if (verticalVelocity < 0.01f && Controller.isGrounded)
        {
            // verticalVelocity = Physics.gravity.y * Time.deltaTime;
            verticalVelocity = currentGravity * Time.deltaTime;
        }
        else
        {
            // verticalVelocity += Physics.gravity.y * Time.deltaTime;
            verticalVelocity += currentGravity * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, Drag);
    }

    public void AddImpact(Vector3 force)
    {
        impact += force;
    }
}
