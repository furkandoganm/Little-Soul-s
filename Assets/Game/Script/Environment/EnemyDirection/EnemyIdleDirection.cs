using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleDirection : MonoBehaviour
{
    [field: SerializeField] public EnemyDirectionState State { get; private set; }
    [field: SerializeField] public EnemyStateMachine Enemy { get; private set; }
    [field: SerializeField] public EnemyIdleDirection Twin { get; private set; }
    

    public void TargetLocation()
    {
        if (State == EnemyDirectionState.TargetingTwin)
        {
            // Enemy.NavMeshAgent.Move(Twin.transform.position);
            Enemy.TargetLocation = Twin.transform.position;
            Enemy.Rotation *= -1f;
            // Debug.Log("assssssssss");
        }
    }
    // public void Turn()
    // {
    //     if (State == EnemyDirectionState.TargetingTwin)
    //     {
    //         
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            TargetLocation();
        }

        // Debug.Log(other.tag);
        // Turn();
    }
}

public enum EnemyDirectionState
{
    TargetingTwin
}