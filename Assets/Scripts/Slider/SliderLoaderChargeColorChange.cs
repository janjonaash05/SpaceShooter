using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLoaderChargeColorChange : MonoBehaviour
{
    
    public Material On, Off;  


    


    void Start()
    {
        transform.parent.GetComponent<SliderLoaderRecharge>().OnActivation += Activate;

        transform.parent.GetComponent<SliderLoaderRecharge>().OnDeactivation += Deactivate;
    }


  
    /// <summary>
    /// If this component isn't null, sets the renderer material to On.
    /// </summary>
    void Activate()
    {
        if (this != null) { GetComponent<Renderer>().material = On; }
    }

    /// <summary>
    /// If this component isn't null, sets the renderer material to Off.
    /// </summary>
    void Deactivate()
    {
        if (this != null) GetComponent<Renderer>().material = Off;

    }


}
