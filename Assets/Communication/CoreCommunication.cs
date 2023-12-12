using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class CoreCommunication 
{

    public static event Action<int> OnValueChangedCore;

    public static event Action OnParentValueChangedCore;

    public static event Action<int> OnValueChangedSpinner;


    


    static public  event Action OnCommunicationInit;





    public const float CHANGE_TIME = 1f;


    public static event Action OnSpinnerChargeUpStart, OnSpinnerChargeUpEnd, OnSpinnerInitialColorUp;
    public static event Action OnCoreFullParticlesStart, OnCoreFullParticlesEnd, OnCoreBreakdownStart, OnCoreBreakdownEnd;




    static public MaterialIndexHolder SPINNER_INDEX_HOLDER { get; private set; }

    static public MaterialIndexHolder CORE_INDEX_HOLDER { get; private set; }

    public static void Awake() 
    {






        LaserTurretCommunication1.OnManualTargeting += (g) => Raise_ValueChange(0, -1);
        LaserTurretCommunication2.OnManualTargeting += (g) => Raise_ValueChange(0, -1);




        SPINNER_INDEX_HOLDER = new(1,1,MaterialIndexHolder.Target.SPINNER, MaterialIndexHolder.Edge.LOWER );
       



        CORE_INDEX_HOLDER = new(4, 4, MaterialIndexHolder.Target.CORE, MaterialIndexHolder.Edge.UPPER);

        
       



        OnCommunicationInit?.Invoke();
    }





    


    public static void Raise_ValueChange(int parent, int child) 
    {
        //   OnValueChangedCore?.Invoke(value);



        Debug.Log(CORE_INDEX_HOLDER + " PRE ");
        Debug.Log(SPINNER_INDEX_HOLDER + " PRE ");

        Debug.Log(parent + " "+child);





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



        switch (result_core) 
        {
            case 1: OnCoreFullParticlesStart?.Invoke(); break;
            default: break;



        }
        if (child >= 0) { OnCoreFullParticlesEnd?.Invoke(); }











    }





    






   

        

    }



