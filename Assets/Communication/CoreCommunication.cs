using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CoreCommunication 
{

    public static event Action<int> OnValueChangedCore;

    public static event Action<int> OnValueChangedSpinner;





    public static float CHANGE_TIME = 1f;


    public static event Action OnSpinnerChargeUpStart, OnSpinnerChargeUpEnd, OnSpinnerInitialColorUp;

    static public MaterialIndexHolder SPINNER_INDEX_HOLDER { get; private set; }

    static public MaterialIndexHolder CORE_INDEX_HOLDER { get; private set; }

    public static void Awake() 
    {
        SPINNER_INDEX_HOLDER = new(0,4,MaterialIndexHolder.Target.SPINNER);

        CORE_INDEX_HOLDER = new(5, 0, MaterialIndexHolder.Target.CORE);

    }





    


    public static void Raise_ValueChange(int parent, int child) 
    {
     //   OnValueChangedCore?.Invoke(value);


        





        int result_spinner = SPINNER_INDEX_HOLDER.ChangeIndex(parent, child);




        switch (result_spinner)
        {
            case 1: OnSpinnerChargeUpStart?.Invoke(); break;
            case -1: OnSpinnerInitialColorUp?.Invoke(); break;
            default: break;
        
        }

        if (child <= 0) { OnSpinnerChargeUpEnd?.Invoke(); }

        int result_core = CORE_INDEX_HOLDER.ChangeIndex(parent, child);













    
    }





    






   

        

    }





public class MaterialIndexHolder : IndexHolder
{

    const int max_parent = 4, min_parent = 1, max_child = 4, min_child = 1;


    static Dictionary<string, int> spinner_mat_dict = new()
        {
            { "11", 18 },
            { "12", 17 },
            { "13", 16 },
            { "14", 15 },
            { "21", 14 },
            { "22", 13 },
            { "23", 12 },
            { "24", 11 },
            { "31", 10 },
            { "32", 9 },
            { "33", 8 },
            { "34", 7 },
            { "41", 6 },
            { "42", 5 },
            { "43", 3 },
            { "44", 4 },
        };
    static Dictionary<string, int> core_mat_dict = new()
        {


            { "11", 2 },
            { "12", 12 },
            { "13", 7 },
            { "14", 11 },
            { "21", 10 },
            { "22", 6 },
            { "23", 4 },
            { "24", 8 },
            { "31", 9 },
            { "32", 17 },
            { "33", 5 },
            { "34", 16 },
            { "41", 15 },
            { "42", 3 },
            { "43", 14 },
            { "44", 13 },











        };



 

    public override string ToString()
    {
        return parent + ", " + child + ", " + target;
    }


    Target target;

    public MaterialIndexHolder(int parent, int child, Target target) : base(min_parent, max_parent, min_child, max_child)
    {


        this.parent = parent;
        this.child = child;
        this.target = target;

    }

    public enum Target { CORE, SPINNER }




    public List<int> AllMatIndexesByHolder(bool color)
    {


        int direction = (color) ? -1 : 1;

        direction *= target == Target.CORE ? -1 : 1;




        var list = new List<int>();

        var copyHolder = new MaterialIndexHolder(parent, child, target);

        int changeResult = 0;
        while (changeResult != direction)
        {

           
            list.Add(GetCurrentMatIndex(copyHolder));

            changeResult = copyHolder.ChangeIndex(0, direction);
            Debug.Log(copyHolder);

        }






        return list;
    }





    int GetCurrentMatIndex(MaterialIndexHolder holder)
    {

        // Debug.Log("GetBy " + holder.parent.ToString() + holder.child.ToString());
        if (parent != 0)
        {





          


            return target == Target.SPINNER ?
                spinner_mat_dict[holder.parent.ToString() + holder.child.ToString()]
                : core_mat_dict[holder.parent.ToString() + holder.child.ToString()];
        }
        else
        {

            return -1;
        }

    }











}



