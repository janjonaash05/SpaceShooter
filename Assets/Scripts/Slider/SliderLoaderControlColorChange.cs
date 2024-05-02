using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SliderLoaderControlColorChange : MonoBehaviour
{
    public enum SliderLoaderControlType:int { BOLT =4, FULL_AUTO = 6 }



    

    
    [SerializeField] Material off_mat;
    [SerializeField] Material on_mat;
    




    //const int index_to_change_full_auto = 6;
    //const int index_to_change_bolt = 4;
    void Start()
    {

        PlayerInputCommunication.OnSliderFullAutoClick += TurnOnFullAuto;
        PlayerInputCommunication.OnSliderFullAutoClick += TurnOffBolt;
        PlayerInputCommunication.OnSliderFullAutoClick += PlaySound;

        PlayerInputCommunication.OnSliderBoltClick += TurnOnBolt;
        PlayerInputCommunication.OnSliderBoltClick += TurnOffFullAuto;
        PlayerInputCommunication.OnSliderBoltClick += PlaySound;











    }



    private void OnDestroy()
    {
        PlayerInputCommunication.OnSliderFullAutoClick -= TurnOnFullAuto;
        PlayerInputCommunication.OnSliderFullAutoClick -= TurnOffBolt;
        PlayerInputCommunication.OnSliderFullAutoClick -= PlaySound;

        PlayerInputCommunication.OnSliderBoltClick -= TurnOnBolt;
        PlayerInputCommunication.OnSliderBoltClick -= TurnOffFullAuto;
        PlayerInputCommunication.OnSliderBoltClick -= PlaySound;
    }



    void PlaySound(RaycastHit _) => AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_CONTROL_CLICK);


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



    /// <summary>
    /// Copies the renderer materials, changes the material at index which matches to the SliderLoaderControlType in value to change_mat, reassigns them.
    /// </summary>
    /// <param name="change_mat"></param>
    /// <param name="control_type"></param>
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
