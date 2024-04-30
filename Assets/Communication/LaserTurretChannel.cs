using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LaserTurretChannel
{


    public static int MAX_TURRET_CAPACITY { get; private set; }

    public int TURRET_CAPACITY { get; private set; }


    public event Action<GameObject> OnManualTargeting;

    public event Action<string> OnAutoTargetingAttempt;
    public event Action OnAutoTargetingSuccess; // for decrease power



    /// <summary>
    /// used for the color change of the turret head during targeting
    /// </summary>
    public event Action OnGeneralTargetingStart, OnGeneralTargetingEnd;


    public event Action<Material> OnColorCollider_ControlColorChange, OnAutoCollider_ControlColorChange;
    public event Action<Material, bool> OnTurretChargeColorChange;


    public event Action OnControlEnabled, OnControlDisabled;


    public event Action OnAutoTargetingEnabled, OnAutoTargetingDisabled;

    public event Action OnTurretCapacityChanged;
    public event Action OnTurretCapacityDepleted, OnTurretCapacityRecharged;

    Material ChargeMaterial;
    public COLOR ChargeColorName { get; private set; }

    private bool is_targeting, is_barraging, is_control_disabled, is_auto_targeting_disabled;



    string tag;
    public void Awake(string tag)
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



        OnAutoTargetingEnabled += () => { is_auto_targeting_disabled = false; };
        OnAutoTargetingDisabled += () => { is_auto_targeting_disabled = true; };



    }



    /// <summary>
    /// Invokes OnTurretCapacityRecharged and OnAutoTargetingEnabled, sets the turret capacity back to max.
    /// </summary>
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


    public void SetBarraging(bool barraging)
    {
        is_barraging = barraging;
    }


    /// <summary>
    /// Calls SetTargeting with true, invokes OnGeneralTargetingStart.
    /// </summary>
    public void Raise_TargetingStart()
    {

        SetTargeting(true);
        OnGeneralTargetingStart?.Invoke();

    }

    /// <summary>
    /// Calls SetTargeting with false, invokes OnGeneralTargetingEnd.
    /// </summary>
    public void Raise_TargetingEnd()
    {
        SetTargeting(false);
        OnGeneralTargetingEnd?.Invoke();

    }





    /// <summary>
    /// <para>Calls AttemptRaise_TurretCharge_ColorChange() with a discard material, NONE as color value and true for disabled.</para>
    /// <para>Calls Raise_DisableControl(), waits a set amount of time, then Raise_EnableControl()</para>
    /// </summary>
    /// <param name="ms"></param>
    public async void DisableControlFor(float ms)
    {

        AttemptRaise_TurretCharge_ColorChange(new Material(Shader.Find("Specular")), COLOR.NONE, true);

        Raise_DisableControl();


        await Task.Delay((int)ms);

        Raise_EnableControl();



    }


    public void Raise_DisableAutoTargeting()
    {
        OnAutoTargetingDisabled?.Invoke();
    }


    public void Raise_EnableAutoTargeting()
    {
        OnAutoTargetingEnabled?.Invoke();

    }

    /// <summary>
    /// <para>Sets the ChargeMaterial to friendly secondary, and ChargeColorName to NONE.</para>
    /// <para>Sets is_control_disabled to true and invokes OnControlDisabled.</para>
    /// </summary>
    public void Raise_DisableControl()
    {


        ChargeMaterial = MaterialHolder.Instance().FRIENDLY_SECONDARY();
        ChargeColorName = COLOR.NONE;

        is_control_disabled = true;
        OnControlDisabled?.Invoke();
    }



    /// <summary>
    /// Sets is_control_disabled to false and invokes OnControlEnabled.
    /// </summary>
    public void Raise_EnableControl()
    {
        is_control_disabled = false;
        OnControlEnabled?.Invoke();

    }


    /// <summary>
    /// <para>If arg turn_off is true, invokes OnTurretChargeColorChange with the ChargeMaterial and true</para>
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="color_name"></param>
    /// <param name="turn_off"></param>
    public void AttemptRaise_TurretCharge_ColorChange(Material mat, COLOR color_name, bool turn_off)
    {

        if (turn_off)
        {
            OnTurretChargeColorChange?.Invoke(ChargeMaterial, true);
            return;
        }



        if (is_targeting || is_control_disabled) { return; }


        ChargeMaterial = mat;
        ChargeColorName = color_name;
        OnTurretChargeColorChange?.Invoke(ChargeMaterial, false);


    }




    /// <summary>
    /// Unless either is_targeting, is_barraging or is_control_disabled are true, invokes OnColorCollider_ControlColorChange.
    /// </summary>
    /// <param name="mat"></param>
    public void AttemptRaise_ColorCollider_ControlColorChange(Material mat)
    {
        if (is_targeting || is_barraging || is_control_disabled) { return; }

        OnColorCollider_ControlColorChange?.Invoke(mat);



    }

    /// <summary>
    /// Unless is_control_disabled or is_auto_targeting_disabled are true, invokes OnAutoCollider_ControlColorChange.
    /// </summary>
    /// <param name="mat"></param>
    public void AttemptRaise_AutoCollider_ControlColorChange(Material mat)
    {
        if (is_control_disabled || is_auto_targeting_disabled) { return; }
        OnAutoCollider_ControlColorChange?.Invoke(mat);



    }



    /// <summary>
    /// <para>Assigns is_cluster based on if the hit's BombType is CLUSTER_UNIT.</para>
    /// <para>Assigns invalid_color_name based on if the ChargeColorName doesn't match the hit's BombColorName.</para>
    /// Unless either is_targeting, is_barraging, is_control_disabled, invalid_color_name or is_cluster are true, invokes OnAutoTargetingAttempt.
    /// </summary>
    /// <param name="hit"></param>
    public void AttemptRaise_ManualTargeting(RaycastHit hit)
    {

        bool is_cluster = hit.transform.GetComponent<BombFall>().BombType == BombType.CLUSTER_UNIT;
        bool invalid_color_name = ChargeColorName != hit.transform.GetComponent<BombColorChange>().BombColorName;


        if (is_targeting || is_barraging || is_control_disabled || invalid_color_name || is_cluster) { return; }

        OnManualTargeting?.Invoke(hit.transform.gameObject);


    }



    /// <summary>
    /// Unless either is_targeting, is_barraging, is_control_disabled or is_auto_targeting_disabled are true, invokes OnAutoTargetingAttempt.
    /// </summary>
    public void AttempRaise_AutoTargetingAttempt()
    {
        if (is_targeting || is_barraging || is_control_disabled || is_auto_targeting_disabled) { return; }

        OnAutoTargetingAttempt?.Invoke(tag);
    }




    /// <summary>
    /// <para>Invokes OnAutoTargetingSuccess.</para>
    /// <para>If the turret capacity is bigger than 0:</para>
    /// <para>- Decreases turret capacity, invokes OnTurretCapacityChanged.</para>
    /// <para>- If the decreased turret capacity is equal to 0:</para>
    /// <para>-- Sets the max turret capacity to the current turret capacity value from the UpgradesManager.</para>
    /// <para>-- Invokes OnTurretCapacityDepleted and OnAutoTargetingDisabled.</para>
    /// <para>If the turret capacity is smaller than 0: </para>
    /// <para>- Invokes OnTurretCapacityChanged.</para>
    /// </summary>
    public void Raise_AutoTargetingSuccess()
    {
        OnAutoTargetingSuccess?.Invoke();

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

    }





}
