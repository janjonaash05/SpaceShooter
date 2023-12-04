using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public static class LaserTurretCommunication2
{
    public static event Action<GameObject> OnManualTargeting;








    public static event Action<string> OnAutoTargetingAttempt;
    public static event Action OnAutoTargetingSuccess; // for decrease power



    /// <summary>
    /// used for the color change of the turret head during targeting
    /// </summary>
    public static event Action OnGeneralTargetingStart, OnGeneralTargetingEnd;




    public static event Action<Material> OnColorCollider_ControlColorChange, OnAutoCollider_ControlColorChange;
    public static event Action<Material, bool> OnTurretChargeColorChange;




    public static event Action OnControlEnabled, OnControlDisabled;



    public static event Action OnAutoTargetingEnabled, OnAutoTargetingDisabled;








    static Material charge_mat;

    private static bool is_targeting, is_barraging, is_control_disabled, is_auto_targeting_disabled;







    public static void Awake()
    {

      


        is_control_disabled = false;
        is_auto_targeting_disabled = false;
        is_barraging = false;
        is_targeting = false;






        //  OnControlDisabled += () => { is_control_disabled = true; Debug.Log("is disable"); };
        // OnControlEnabled += () => { is_control_disabled = false; Debug.Log("is enable"); };





        OnAutoTargetingEnabled += () => is_auto_targeting_disabled = true;
        OnAutoTargetingDisabled += () => is_auto_targeting_disabled = false;



    }





    public static void SetTargeting(bool targeting)
    {
        is_targeting = targeting;

    }


    public static void SetBarraging(bool barraging)
    {
        is_barraging = barraging;
    }



    public static void Raise_TargetingStart()
    {

        SetTargeting(true);
        OnGeneralTargetingStart?.Invoke();

    }


    public static void Raise_TargetingEnd()
    {
        SetTargeting(false);
        OnGeneralTargetingEnd?.Invoke();

    }




    public static async void DisableControlFor(float ms)
    {

        AttemptRaise_TurretCharge_ColorChange(new Material(Shader.Find("Specular")), true);

        Raise_DisableControl();


        await Task.Delay((int)ms);

        Raise_EnableControl();



    }



    public static void Raise_DisableAutoTargeting()
    {
        is_auto_targeting_disabled = true;
        OnAutoTargetingDisabled?.Invoke();
    }


    public static void Raise_EnableAutoTargeting()
    {
        is_auto_targeting_disabled = false;
        OnAutoTargetingEnabled?.Invoke();

    }


    private static void Raise_DisableControl()
    {

        is_control_disabled = true;
        OnControlDisabled?.Invoke();
    }


    private static void Raise_EnableControl()
    {
        is_control_disabled = false;
        OnControlEnabled?.Invoke();

    }



    public static void AttemptRaise_TurretCharge_ColorChange(Material mat, bool turn_off)
    {

        if (turn_off)
        {
            OnTurretChargeColorChange?.Invoke(charge_mat, true);
            return;
        }



        if (is_targeting || is_control_disabled) { return; }



        //   Debug.LogWarning("turretcharge change ");
        charge_mat = mat;
        OnTurretChargeColorChange?.Invoke(charge_mat, false);


    }





    public static void AttemptRaise_ColorCollider_ControlColorChange(Material mat)
    {
        if (is_targeting || is_barraging || is_control_disabled) { Console.WriteLine("nuh uh"); return; }

        OnColorCollider_ControlColorChange?.Invoke(mat);


    }


    public static void AttemptRaise_AutoCollider_ControlColorChange(Material mat)
    {
        if (is_targeting || is_control_disabled || is_barraging || is_auto_targeting_disabled) { Console.WriteLine("nuh uh"); return; }
        OnAutoCollider_ControlColorChange?.Invoke(mat);



    }





    public static void AttemptRaise_ManualTargeting(RaycastHit hit)
    {
        if (is_targeting || is_barraging || is_control_disabled) { return; }


        if (hit.transform.gameObject.GetComponent<BombColorChange>().Color.color.Equals(charge_mat.color))
        {
            OnManualTargeting?.Invoke(hit.transform.gameObject);
        }
    }


    public static void AttempRaise_AutoTargetingAttempt()
    {
        if (is_targeting || is_barraging || is_control_disabled || is_auto_targeting_disabled) { return; }


        OnAutoTargetingAttempt?.Invoke(Tags.LASER_TARGET_2);


    }


    public static void Raise_AutoTargetingSuccess()
    {
        OnAutoTargetingSuccess?.Invoke();

    }



}

