using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem TargetingPS { get; private set; }
    public Renderer ParticleSystemRenderer { get; private set; }

    private void Start()
    {
        ParticleSystemRenderer = TargetingPS.GetComponent<Renderer>();
        TargetingPS.Pause();
        ParticleSystemRenderer.enabled = false;
    }

    public void Target(bool isTargeted)
    {
        ParticleSystemRenderer.enabled = isTargeted;
        if (isTargeted)
        {
            TargetingPS.Play();
            EnableChildRenderers(TargetingPS.transform);
            // foreach (Transform child in TargetingPS.transform)
            // {
            //     child.GetComponent<Renderer>().enabled = true;
            // }
        }
        else
        {
            TargetingPS.Pause();
            DisableChildRenderers(TargetingPS.transform);
            // foreach (Transform child in TargetingPS.transform)
            // {
            //     child.GetComponent<Renderer>().enabled = false;
            // }
        }
    }
    
    private void DisableChildRenderers(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ParticleSystem childParticleSystem = child.GetComponent<ParticleSystem>();
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childParticleSystem != null && childRenderer != null)
            {
                childRenderer.enabled = false;
            }

            DisableChildRenderers(child);
        }
    }
    
    private void EnableChildRenderers(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ParticleSystem childParticleSystem = child.GetComponent<ParticleSystem>();
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (childParticleSystem != null && childRenderer != null)
            {
                childRenderer.enabled = true;
            }

            EnableChildRenderers(child);
        }
    }
}
