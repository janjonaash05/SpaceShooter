using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLoaderControlColorChange : MonoBehaviour
{
    public enum SliderLoaderControlType:int { BOLT =4, FULL_AUTO = 6 }


    // Start is called before the first frame update
    [SerializeField] Material off_mat;
    [SerializeField] Material on_mat;
    




    //const int index_to_change_full_auto = 6;
    //const int index_to_change_bolt = 4;
    void Start()
    {

        PlayerInputCommunication.OnSliderFullAutoClick += TurnOnFullAuto;
        PlayerInputCommunication.OnSliderFullAutoClick += TurnOffBolt;

        PlayerInputCommunication.OnSliderBoltClick += TurnOnBolt;
        PlayerInputCommunication.OnSliderBoltClick += TurnOffFullAuto;











    }




    


    void TurnOffBolt(RaycastHit hit)
    {
    
    Engage(off_mat, SliderLoaderControlType.BOLT);
    }


    void TurnOnBolt(RaycastHit hit)
    {
        Engage(on_mat, SliderLoaderControlType.BOLT);
    }


    void TurnOffFullAuto(RaycastHit hit) 
    {
        Engage(off_mat, SliderLoaderControlType.FULL_AUTO);
    }


    void TurnOnFullAuto(RaycastHit hit)
    {
        Engage(on_mat, SliderLoaderControlType.FULL_AUTO);
    }




    void Engage(Material change_mat, SliderLoaderControlType control_type) 
    {

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = (i == (int) control_type) ? change_mat : GetComponent<Renderer>().materials[i];
        }


        GetComponent<Renderer>().materials = mats;

    }
}
