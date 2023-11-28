using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class PlayerInputCommunication
{


    public static event Action<RaycastHit>
        OnColorCollider1Click, OnColorCollider2Click,
        OnAutoCollider1Click, OnAutoCollider2Click,
        OnLaserTarget1Click, OnLaserTarget2Click,

        OnSliderControlClick, OnSliderFullAutoClick, OnSliderBoltClick;





    public static event Action OnMouseDown, OnMouseUp;

    static Dictionary<string, Action<RaycastHit>> tag_click_dictionary = new()
    {
        { Tags.COLOR_COLLIDER_1, Raise_OnColorCollider1Click},
            { Tags.COLOR_COLLIDER_2, Raise_OnColorCollider2Click },

            { Tags.AUTO_COLLIDER_1, Raise_OnAutoCollider1Click },
            { Tags.AUTO_COLLIDER_2, Raise_OnAutoCollider2Click },

            { Tags.LASER_TARGET_1, Raise_OnLaserTarget1Click },
            { Tags.LASER_TARGET_2, Raise_OnLaserTarget2Click },


             { Tags.SLIDER_CONTROL_COLLIDER, Raise_OnSliderControlClick },

             { Tags.SLIDER_FULL_AUTO_COLLIDER, Raise_OnSliderFullAutoClick },
             { Tags.SLIDER_BOLT_COLLIDER, Raise_OnSliderBoltClick },



        };




    public static void Raise_RaycastClick(RaycastHit hit)
    {
        Action<RaycastHit> action = (hit) => { };



        try
        {
            action = tag_click_dictionary[hit.transform.tag];
            Debug.Log("action " + hit.transform.tag);
        }
        catch (Exception)
        {
            action(hit);
        }


        action?.Invoke(hit);



    }




    static void Raise_OnColorCollider1Click(RaycastHit hit)
    {

        OnColorCollider1Click?.Invoke(hit);
    }

    static void Raise_OnColorCollider2Click(RaycastHit hit)
    {

        OnColorCollider2Click?.Invoke(hit);
    }


    static void Raise_OnAutoCollider1Click(RaycastHit hit)
    {

        OnAutoCollider1Click?.Invoke(hit);
    }

    static void Raise_OnAutoCollider2Click(RaycastHit hit)
    {

        OnAutoCollider2Click?.Invoke(hit);
    }

    static void Raise_OnLaserTarget1Click(RaycastHit hit)
    {

        OnLaserTarget1Click?.Invoke(hit);
    }

    static void Raise_OnLaserTarget2Click(RaycastHit hit)
    {

        OnLaserTarget2Click?.Invoke(hit);
    }


    static void Raise_OnSliderControlClick(RaycastHit hit)
    {

        OnSliderControlClick?.Invoke(hit);
    }

    static void Raise_OnSliderFullAutoClick(RaycastHit hit)
    {

        OnSliderFullAutoClick?.Invoke(hit);
    }

    static void Raise_OnSliderBoltClick(RaycastHit hit)
    {

        OnSliderBoltClick?.Invoke(hit);
    }





}
