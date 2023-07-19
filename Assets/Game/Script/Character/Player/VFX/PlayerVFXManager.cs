using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    [field: SerializeField] public VisualEffect FootStep { get; private set; }
    
    public void UpdateFootStep(bool state)
    {
        if (state)
        {
            // FootStep.SetActive(true);
            FootStep.Play();
        }
        else
        {
            // FootStep.SetActive(false);
            FootStep.Stop();
        }
    }
}