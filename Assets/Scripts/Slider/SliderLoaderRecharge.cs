using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderLoaderRecharge : MonoBehaviour
{
    

    public event Action OnDepletion, OnFullRecharge;

    public event Action OnActivation, OnDeactivation;

    protected bool _isRecharging = false;
    public bool IsRecharging { get { return _isRecharging; } }



    protected bool _isActive = false;
    public bool IsActive { get { return _isActive; } }


    /// <summary>
    /// Plays the SLIDER_BOLT_RECHARGE_START sound, invokes OnDepletion, sets is_isRecharging to true.
    /// </summary>
    protected void OnDepletionInvoke()
    {


        AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_BOLT_RECHARGE_START);
        OnDepletion?.Invoke();
        _isRecharging = true;
    
    
    }
    /// <summary>
     /// Stops the SLIDER_BOLT_RECHARGE_START sound, invokes OnDepletion, sets is_isRecharging to true.
     /// </summary>
    protected void OnFullRechargeInvoke()
    {
        AudioManager.StopActivitySound(AudioManager.ActivityType.SLIDER_BOLT_RECHARGE_START);
        OnFullRecharge?.Invoke();
        _isRecharging = false;


    }




    public void OnActivationInvoke()
    {
        OnActivation?.Invoke();
        _isActive = true;


    }
    public void OnDeactivationInvoke()
    {
        OnDeactivation?.Invoke();
        _isActive = false;


    }




}
