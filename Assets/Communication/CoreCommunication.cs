using System;
using System.Collections;
using Unity.VisualScripting;
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

        SHIELD_CAPACITY = DifficultyManager.SHIELD_MAX_CAPACITY;



      

        LaserTurretCommunicationChannels.Channel1.OnManualTargeting += (g) => Raise_ValueChange(0, -1);
        LaserTurretCommunicationChannels.Channel2.OnManualTargeting += (g) => Raise_ValueChange(0, -1);

        SPINNER_INDEX_HOLDER = new(1, 1, MaterialIndexHolder.Target.SPINNER, MaterialIndexHolder.Edge.LOWER);

        CORE_INDEX_HOLDER = new(4, 4, MaterialIndexHolder.Target.CORE, MaterialIndexHolder.Edge.UPPER);

        OnCommunicationInit?.Invoke();
    }




    public static void Raise_OnBombFallen(Material m)
    {

        if (SHIELD_CAPACITY > 0)
        {
            SHIELD_CAPACITY--;
            OnBombFallen?.Invoke(mat_holder.SIDE_TOOLS_COLOR());
        }
        else
        {
            Raise_ShieldDepleted();
            Raise_ValueChange(0, 1);
            OnBombFallen?.Invoke(m);
        }



    }





    public static void Raise_ShieldRecharged() 
    {
        OnShieldRecharged?.Invoke();
        SHIELD_CAPACITY = DifficultyManager.SHIELD_MAX_CAPACITY;
    }



    public static void Raise_ShieldDepleted()
    {
        OnShieldDepleted?.Invoke();

    }


    public static void Raise_ValueChange(int parent, int child)
    {
        //   OnValueChangedCore?.Invoke(value);



        Debug.Log(CORE_INDEX_HOLDER + " PRE ");
        Debug.Log(SPINNER_INDEX_HOLDER + " PRE ");

        Debug.Log(parent + " " + child);





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



