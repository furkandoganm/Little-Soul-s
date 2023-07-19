using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public List<Target> targets = new List<Target>();

    public Target currentTarget { get; private set;}

    [field: SerializeField] public CinemachineTargetGroup cineTargetingGroup;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target))
            return;
        // target.TargetingVFX.enabled = true;
        targets.Add(target);

        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target))
            return;
        // target.TargetingVFX.enabled = false;
        // RemoveTarget(target);
        targets.Remove(target);
        if (targets == null || targets.Count == 0)
            CancelTarget();
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0 || targets == null)
            return false;
        if (targets.Count == 1)
        {
            currentTarget = targets[0];
            // cineTargetingGroup.AddMember(currentTarget.transform, 1f, 2f);
        }
        return true;
    }

    public void CancelTarget()
    {
        if (currentTarget == null) return;
        
        // cineTargetingGroup.RemoveMember(currentTarget.transform);
        currentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if (currentTarget == target)
        {
            cineTargetingGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}