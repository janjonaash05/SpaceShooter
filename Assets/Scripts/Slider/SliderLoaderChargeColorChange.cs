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


    private void Awake()
    {
    }

    void Activate()
    {
        if (this != null) { GetComponent<Renderer>().material = On; }
    }

    void Deactivate()
    {
        if (this != null) GetComponent<Renderer>().material = Off;

    }


}
