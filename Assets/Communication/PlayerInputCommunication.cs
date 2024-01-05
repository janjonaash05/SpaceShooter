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

        OnHarpoonColliderClick,
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


            { Tags.HARPOON_COLLIDER, Raise_OnHarpoonColliderClick }
        };





    public static void Raise_MouseDown() 
    {
        OnMouseDown?.Invoke();

    }

    public static void Raise_MouseUp() 
    {
        OnMouseUp?.Invoke();
    
    }

    public static bool Raise_RaycastClick(RaycastHit hit)
    {
        Action<RaycastHit> action = (hit) => { };



        bool valid_click;

        try
        {
            action = tag_click_dictionary[hit.transform.tag];
            Debug.Log("action " + hit.transform.tag);

            valid_click = true;
        }
        catch (Exception)
        {
            valid_click = false;
           // action(hit); 
        }


        action?.Invoke(hit);

        return valid_click;

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


    static void Raise_OnHarpoonColliderClick(RaycastHit hit)
    {
        OnHarpoonColliderClick?.Invoke(hit);
    }






}
