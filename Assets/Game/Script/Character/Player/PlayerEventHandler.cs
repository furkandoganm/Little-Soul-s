using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    // private readonly int RollHash = Animator.StringToHash("Roll");
    private readonly int IsRollHash = Animator.StringToHash("IsRoll");

    [field:SerializeField] public PlayerStateMachine StateMachine { get; private set; }

    #region Falling

    private readonly int FallingStateHash = Animator.StringToHash("FallingState");

    public void EndFalling()
    {
        StateMachine._movement = StateMachine.transform.forward;
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }

    public void StandUp()
    {
        StateMachine.Animator.SetInteger(FallingStateHash, 5);
    }

    public void StartRolling()
    {
        StateMachine._movement = transform.forward * StateMachine.RollingSpeed;
    }

    public void EndRolling()
    {
        StateMachine._movement = transform.forward;
    }

    #endregion

    #region FreeLook
    
    public void StopRolling()
    {
        // StateMachine.IsRoll = false;
        // StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        // StateMachine.Animator.Play(FreeLookBlendTreeHash);
        StateMachine.Animator.SetBool(IsRollHash, false);
    }

    public void FreeLookRollSpeedStart()
    {
        StateMachine.IsRoll = true;
        GetComponent<PlayerVFXManager>().UpdateFootStep(true);
    }
    
    public void FreeLookRollSpeedEnd()
    {
        StateMachine.IsRoll = false;
        GetComponent<PlayerVFXManager>().UpdateFootStep(false);
    }

    #endregion

    #region MyRegion

    

    #endregion
    
}
