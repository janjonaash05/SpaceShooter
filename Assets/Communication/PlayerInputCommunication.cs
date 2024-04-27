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
        OnSliderControlClick, OnSliderFullAutoClick, OnSliderBoltClick,
        OnUpgradeStationArrowDownClick, OnUpgradeStationArrowUpClick, OnUpgradeStationClick,
     OnHelperStationArrowDownClick, OnHelperStationArrowUpClick, OnHelperStationClick;





    public static event Action OnMouseDown, OnMouseUp;

    static readonly Dictionary<string, Action<RaycastHit>> TAG_CLICK_DICT = new()
    {
        { Tags.COLOR_COLLIDER_1, (h) => OnColorCollider1Click?.Invoke(h) },
            { Tags.COLOR_COLLIDER_2, (h) => OnColorCollider2Click?.Invoke(h)  },

            { Tags.AUTO_COLLIDER_1, (h) => OnAutoCollider1Click?.Invoke(h)  },
            { Tags.AUTO_COLLIDER_2, (h) => OnAutoCollider2Click?.Invoke(h)  },

            { Tags.LASER_TARGET_1, (h) => OnLaserTarget1Click?.Invoke(h)  },
            { Tags.LASER_TARGET_2, (h) => OnLaserTarget2Click?.Invoke(h)  },


             { Tags.SLIDER_CONTROL_COLLIDER, (h) => OnSliderControlClick?.Invoke(h)  },

             { Tags.SLIDER_FULL_AUTO_COLLIDER, (h) => OnSliderFullAutoClick?.Invoke(h)  },
             { Tags.SLIDER_BOLT_COLLIDER, (h) => OnSliderBoltClick?.Invoke(h)  },


            { Tags.HARPOON_COLLIDER, (h) => OnHarpoonColliderClick?.Invoke(h) },

            { Tags.UPGRADE_STATION, (h) => OnUpgradeStationClick?.Invoke(h) },

            { Tags.UPGRADE_STATION_ARROW_UP, (h) => OnUpgradeStationArrowUpClick?.Invoke(h)  },
            { Tags.UPGRADE_STATION_ARROW_DOWN, (h) => OnUpgradeStationArrowDownClick?.Invoke(h)  },


            { Tags.HELPER_STATION, (h) => OnHelperStationClick?.Invoke(h) },

            { Tags.HELPER_STATION_ARROW_UP, (h) => OnHelperStationArrowUpClick?.Invoke(h)  },
            { Tags.HELPER_STATION_ARROW_DOWN, (h) => OnHelperStationArrowDownClick?.Invoke(h)  },

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
            action = TAG_CLICK_DICT[hit.transform.tag];

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




    

   



}
