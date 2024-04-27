using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LaserTurretChannel
{


    public static int MAX_TURRET_CAPACITY { get; private set; }

    public int TURRET_CAPACITY { get; private set; }


    public  event Action<GameObject> OnManualTargeting;

    public  event Action<string> OnAutoTargetingAttempt;
    public   event Action OnAutoTargetingSuccess; // for decrease power



    /// <summary>
    /// used for the color change of the turret head during targeting
    /// </summary>
    public  event Action OnGeneralTargetingStart, OnGeneralTargetingEnd;


    public  event Action<Material> OnColorCollider_ControlColorChange, OnAutoCollider_ControlColorChange;
    public  event Action<Material, bool> OnTurretChargeColorChange;


    public  event Action OnControlEnabled, OnControlDisabled;


    public  event Action OnAutoTargetingEnabled, OnAutoTargetingDisabled;

    public event Action OnTurretCapacityChanged;
    public event Action OnTurretCapacityDepleted, OnTurretCapacityRecharged;

    Material ChargeMaterial;
    public COLOR ChargeColorName { get; private set; }

    private  bool is_targeting, is_barraging, is_control_disabled, is_auto_targeting_disabled;



    string tag;
    public  void Awake(string tag)
    {
        this.tag = tag;


        ChargeColorName = COLOR.NONE;


        is_control_disabled = false;
        is_auto_targeting_disabled = false;
        is_barraging = false;
        is_targeting = false;


        MAX_TURRET_CAPACITY = UpgradesManager.GetCurrentTurretCapacityValue();
        
        TURRET_CAPACITY = MAX_TURRET_CAPACITY;


        UpgradesManager.OnTurretCapacityValueChange += () => MAX_TURRET_CAPACITY = UpgradesManager.GetCurrentTurretCapacityValue();




        OnAutoTargetingSuccess += () =>
        {
            if (TURRET_CAPACITY > 0)
            {
                TURRET_CAPACITY--;
                OnTurretCapacityChanged?.Invoke();


                if (TURRET_CAPACITY == 0)
                {
                    MAX_TURRET_CAPACITY = UpgradesManager.GetCurrentTurretCapacityValue();
                    OnTurretCapacityDepleted?.Invoke();
                    OnAutoTargetingDisabled?.Invoke();
                    return;
                }

            }
            else
            {
                OnTurretCapacityChanged?.Invoke();

            }

        };




        OnAutoTargetingEnabled += () => { is_auto_targeting_disabled = false; };
        OnAutoTargetingDisabled += () => { is_auto_targeting_disabled = true; };



    }




    public void Raise_TurretCapacityRecharged()
    {
        OnTurretCapacityRecharged?.Invoke();
        OnAutoTargetingEnabled?.Invoke();
        TURRET_CAPACITY = MAX_TURRET_CAPACITY;
    }





    
    public void SetTargeting(bool targeting)
    {
        is_targeting = targeting;

    }


    public  void SetBarraging(bool barraging)
    {
        is_barraging = barraging;
    }



    public  void Raise_TargetingStart()
    {

        SetTargeting(true);
        OnGeneralTargetingStart?.Invoke();

    }


    public  void Raise_TargetingEnd()
    {
        SetTargeting(false);
        OnGeneralTargetingEnd?.Invoke();

    }




    public  async void DisableControlFor(float ms)
    {

        AttemptRaise_TurretCharge_ColorChange(new Material(Shader.Find("Specular") ), COLOR.NONE, true);

        Raise_DisableControl();


        await Task.Delay((int)ms);

        Raise_EnableControl();



    }


    public  void Raise_DisableAutoTargeting()
    {
       // is_auto_targeting_disabled = true;
        OnAutoTargetingDisabled?.Invoke();
    }


    public  void Raise_EnableAutoTargeting()
    {
      //  is_auto_targeting_disabled = false;
        OnAutoTargetingEnabled?.Invoke();

    }


    public  void Raise_DisableControl()
    {


        ChargeMaterial = MaterialHolder.Instance().FRIENDLY_SECONDARY();
        ChargeColorName = COLOR.NONE;

        is_control_disabled = true;
        OnControlDisabled?.Invoke();
    }


    public  void Raise_EnableControl()
    {
        is_control_disabled = false;
        OnControlEnabled?.Invoke();

    }



    public  void AttemptRaise_TurretCharge_ColorChange(Material mat, COLOR color_name, bool turn_off)
    {

        if (turn_off)
        {
            OnTurretChargeColorChange?.Invoke(ChargeMaterial, true);
            return;
        }



        if (is_targeting || is_control_disabled) { return; }


        ChargeMaterial = mat;
        ChargeColorName = color_name ;
        OnTurretChargeColorChange?.Invoke(ChargeMaterial, false);


    }





    public  void AttemptRaise_ColorCollider_ControlColorChange(Material mat)
    {
        if (is_targeting || is_barraging || is_control_disabled) { return; }

        OnColorCollider_ControlColorChange?.Invoke(mat);
       


    }


    public  void AttemptRaise_AutoCollider_ControlColorChange(Material mat)
    {
        if (is_control_disabled || is_auto_targeting_disabled) { return; }
        OnAutoCollider_ControlColorChange?.Invoke(mat);



    }

    public void AttemptRaise_ManualTargeting(RaycastHit hit)
    {
        if (is_targeting || is_barraging || is_control_disabled) { return; }

        if (hit.transform.GetComponent<BombFall>().BombType == BombType.CLUSTER_UNIT) return;


        /*

        if (charge_mat.name.Contains(hit.transform.gameObject.GetComponent<BombColorChange>().BombMaterial.name)) 
        {

            OnManualTargeting?.Invoke(hit.transform.gameObject);
        }


        */


        if (ChargeColorName == hit.transform.GetComponent<BombColorChange>().BombColorName) 
        {

            OnManualTargeting?.Invoke(hit.transform.gameObject);
        }

    }


    public  void AttempRaise_AutoTargetingAttempt()
    {
        if (is_targeting || is_barraging || is_control_disabled || is_auto_targeting_disabled) { return; }

        OnAutoTargetingAttempt?.Invoke(tag);
    }


    public  void Raise_AutoTargetingSuccess()
    {
        OnAutoTargetingSuccess?.Invoke();

    }





}
