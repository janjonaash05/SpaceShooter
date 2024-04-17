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


    public void Awake()
    {
        
    }


    protected void OnDepletionInvoke()
    {
        OnDepletion?.Invoke();
        _isRecharging = true;
    
    
    }
    protected void OnFullRechargeInvoke()
    {
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
