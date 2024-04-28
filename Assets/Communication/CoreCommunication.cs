using System;
using System.Collections;
using System.Runtime.CompilerServices;

using UnityEngine;

public static class CoreCommunication
{

    public static event Action<int> OnValueChangedCore;
    public static event Action OnParentValueChangedCore;

    public static event Action<int> OnValueChangedSpinner;

    public static event Action<Material> OnBombFallen;

    static public event Action OnCommunicationInit;

    static public event Action OnShieldDepleted, OnShieldRecharged;

    public const float CHANGE_TIME = 1f;

    public static event Action OnSpinnerChargeUpStart, OnSpinnerChargeUpEnd, OnSpinnerInitialColorUp;





    static public MaterialIndexHolder SPINNER_INDEX_HOLDER { get; private set; }

    static public MaterialIndexHolder CORE_INDEX_HOLDER { get; private set; }

    static public int SHIELD_CAPACITY { get; private set; }

    static MaterialHolder mat_holder;


    public static void Awake()
    {

        mat_holder = MaterialHolder.Instance();

        SHIELD_CAPACITY = UpgradesManager.SHIELD_MAX_CAPACITY;


        LaserTurretCommunicationChannels.Channel1.OnManualTargeting += ManualTargeting;
        LaserTurretCommunicationChannels.Channel2.OnManualTargeting += ManualTargeting;

        SPINNER_INDEX_HOLDER = new(1, 1, MaterialIndexHolder.Target.SPINNER, MaterialIndexHolder.Edge.LOWER);

        CORE_INDEX_HOLDER = new(4, 4, MaterialIndexHolder.Target.CORE, MaterialIndexHolder.Edge.UPPER);


        OnCommunicationInit?.Invoke();
    }


    static void ManualTargeting(GameObject g) => Raise_ValueChange(0, -1);


    /// <summary>
    /// <para>If shield capacity is bigger than 0, decreases the capacity and invokes OnBombFallen with the side tools material.</para>
    /// <para>Also, if the decreased capacity amounts to 0, then calls Raise_ShieldDepleted().</para>
    /// </summary>
    public static void DamageShieldOnly() 
    {
        if (SHIELD_CAPACITY > 0) 
        {

            SHIELD_CAPACITY--;
            OnBombFallen?.Invoke(mat_holder.SIDE_TOOLS_COLOR());

            if (SHIELD_CAPACITY == 0)
            {
                Raise_ShieldDepleted();
                
            }
        }

    }




    /// <summary>
    /// <para>If shield capacity is bigger than 0, decreases the capacity and invokes OnBombFallen with the side tools material.</para>
    /// <para>Also, if the decreased capacity amounts to 0, then calls Raise_ShieldDepleted() and returns.</para>
    /// <para>If the shield capacity is 0, invokes OnBombFallen with the arg material and calls Raise_ValueChange with 0 parent and 1 child value. </para>
    /// </summary>
    /// <param name="m"></param>
    public static void Raise_OnBombFallen(Material m)
    {
        if (SHIELD_CAPACITY > 0)
        {
            SHIELD_CAPACITY--;
            OnBombFallen?.Invoke(mat_holder.SIDE_TOOLS_COLOR());


            if (SHIELD_CAPACITY == 0)
            {
                Raise_ShieldDepleted();
                return;
            }

        }
        else 
        {
            OnBombFallen?.Invoke(m);
            Raise_ValueChange(0, 1);

        }




    }





    public static void Raise_ShieldRecharged() 
    {
        OnShieldRecharged?.Invoke();
        SHIELD_CAPACITY = UpgradesManager.SHIELD_MAX_CAPACITY;
    }



    public static void Raise_ShieldDepleted()
    {
        OnShieldDepleted?.Invoke();

    }

    /// <summary>
    /// <para>Gets the result of change on the spinner index holder, invokes OnSpinnerChargeUpStart or OnSpinnerInitialColorUp events based on the result.</para>
    /// <para>If child is nonpositive, OnSpinnerChargeUpEnd is invoked.</para>
    /// <para>Gets the core index holder parent value, applies negative child and parent change, and invokes OnParentValueChangedCore if the parent values pre and post change differ. </para>
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public static void Raise_ValueChange(int parent, int child)
    {
        int result_spinner = SPINNER_INDEX_HOLDER.ChangeIndex(parent, child);




        switch (result_spinner)
        {
            case 1: OnSpinnerChargeUpStart?.Invoke(); break;
            case -1: OnSpinnerInitialColorUp?.Invoke(); break;
            default: break;

        }

        if (child <= 0) { OnSpinnerChargeUpEnd?.Invoke(); }


        int core_parent_prechange = CORE_INDEX_HOLDER.Parent;
        int result_core = CORE_INDEX_HOLDER.ChangeIndex(-parent, -child);



        if (CORE_INDEX_HOLDER.Parent != core_parent_prechange) OnParentValueChangedCore?.Invoke();


    }
















}



