using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLoaderChargeColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Material On, Off;  // Start is called before the first frame update


    // Update is called once per frame


    void Start()
    {
        transform.parent.GetComponent<SliderLoaderRecharge>().OnActivation += Activate;

        transform.parent.GetComponent<SliderLoaderRecharge>().OnDeactivation += Deactivate;
    }




    void Activate()
    {
       if(this!=null) GetComponent<Renderer>().material = On;
    }

    void Deactivate()
    {
        if (this != null) GetComponent<Renderer>().material = Off;

    }


}
