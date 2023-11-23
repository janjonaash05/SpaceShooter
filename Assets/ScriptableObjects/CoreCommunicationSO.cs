using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreCommunicationSO : ScriptableObject
{

    public static event Action<int> OnValueChangedCore;

    public static event Action<int,int> OnValueChangedSpinner;






    Queue<int> value_queue = new();


    public static void Raise_OnValueChanged(int value) 
    {
        OnValueChangedCore?.Invoke(value);


        


        int parent = value / 4;
        int child = value % 4;

        OnValueChangedSpinner?.Invoke(parent,child);

        
    
    
    
    }


    
   


    










}
