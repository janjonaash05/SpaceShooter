using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeathColorChange : MonoBehaviour
{









    enum DeathTarget
    {
        CORE,
        CORE_RING,
        GROUND_MAIN_PRIMARY,
        GROUND_MAIN_SECONDARY,
        CONTROL_PAD,
        GROUND_SIDE_SECONDARY,
        GROUND_SIDE_PRIMARY,
        CONTROL_STAND, SLIDER_RECHARGE_STATION, TURRET_PILLAR,
        CONTROL_HEAD, HARPOON_CONTROL_HEAD, SLIDER_RECHARGE_HEAD, TURRET_STATION, SLIDER_HEAD, HARPOON_HEAD,
        TURRET_HEAD,
        SHIELD_ADAPTER, SHIELD_STORAGE, SHIELD_EMITTER, TOKEN_TRANSPORTER

    }


    Renderer rend;
    Material changing_mat;

    bool locked = true;


    Dictionary<DeathTarget, (Action color_change_type, Action manager_listener)> target_actions_dict;




    void MaterialChange(Material m)
    {
        changing_mat = m;
        if (!locked) color_change();
    }







    private void OnDestroy()
    {
        SpinnerColorChange.OnMaterialChange -= MaterialChange;



        Action removeListener = death_target switch
        {

            DeathTarget.CORE => () => { GroundDeathManager.OnCoreDeath -= UnlockAndChange; }
            ,

            DeathTarget.CORE_RING => () => { GroundDeathManager.OnCoreRingDeath -= UnlockAndChange; }
            ,

            DeathTarget.GROUND_MAIN_PRIMARY => () => { GroundDeathManager.OnGroundMainPrimaryDeath -= UnlockAndChange; }
            ,

            DeathTarget.GROUND_MAIN_SECONDARY => () => { GroundDeathManager.OnGroundMainSecondaryDeath -= UnlockAndChange; }
            ,

            DeathTarget.CONTROL_PAD => () => { GroundDeathManager.OnControlPadsDeath -= UnlockAndChange; }
            ,

            DeathTarget.GROUND_SIDE_PRIMARY => () => { GroundDeathManager.OnGroundSidePrimaryDeath -= UnlockAndChange; }
            ,

            DeathTarget.GROUND_SIDE_SECONDARY => () => { GroundDeathManager.OnGroundSideSecondaryDeath -= UnlockAndChange; }
            ,

            DeathTarget.CONTROL_STAND => () => { GroundDeathManager.OnControlStandsDeath -= UnlockAndChange; }
            ,

            DeathTarget.SLIDER_RECHARGE_STATION => () => { GroundDeathManager.OnSliderRechargeStationDeath -= UnlockAndChange; }
            ,

            DeathTarget.TURRET_PILLAR => () => { GroundDeathManager.OnTurretPillarsDeath -= UnlockAndChange; }
            ,

            DeathTarget.CONTROL_HEAD => () => { GroundDeathManager.OnControlHeadsDeath -= UnlockAndChange; }
            ,

            DeathTarget.HARPOON_CONTROL_HEAD => () => { GroundDeathManager.OnHarpoonControlHeadsDeath -= UnlockAndChange; }
            ,

            DeathTarget.SLIDER_RECHARGE_HEAD => () => { GroundDeathManager.OnSliderRechargeHeadsDeath -= UnlockAndChange; }
            ,

            DeathTarget.TURRET_STATION => () => { GroundDeathManager.OnTurretStationsDeath -= UnlockAndChange; }
            ,

            DeathTarget.SLIDER_HEAD => () => { GroundDeathManager.OnSliderHeadDeath -= UnlockAndChange; }
            ,

            DeathTarget.HARPOON_HEAD => () => { GroundDeathManager.OnHarpoonHeadDeath -= UnlockAndChange; }
            ,

            DeathTarget.TURRET_HEAD => () => { GroundDeathManager.OnTurretHeadsDeath -= UnlockAndChange; }
            ,

            DeathTarget.SHIELD_ADAPTER => () => { GroundDeathManager.OnShieldAdapterDeath -= UnlockAndChange; }
            ,

            DeathTarget.SHIELD_STORAGE => () => { GroundDeathManager.OnShieldStorageDeath -= UnlockAndChange; }
            ,

            DeathTarget.SHIELD_EMITTER => () => { GroundDeathManager.OnShieldEmitterDeath -= UnlockAndChange; }
            ,

            DeathTarget.TOKEN_TRANSPORTER => () => { GroundDeathManager.OnTokenTransporterDeath -= UnlockAndChange; }
            ,

        };

        removeListener();







    }


    void Start()
    {
        rend = GetComponent<Renderer>();
        SpinnerColorChange.OnMaterialChange += MaterialChange;
        target_actions_dict = new()
        {

            {DeathTarget.CORE,( NonChildrenFull, () => {GroundDeathManager.OnCoreDeath +=  UnlockAndChange; } ) },
            {DeathTarget.CORE_RING, ( NonChildrenFull, () => {GroundDeathManager.OnCoreRingDeath +=  UnlockAndChange;} ) },
            {DeathTarget.GROUND_MAIN_PRIMARY, ( GroundPrimary, () => {GroundDeathManager.OnGroundMainPrimaryDeath +=  UnlockAndChange;} ) },
            {DeathTarget.GROUND_MAIN_SECONDARY, ( GroundSecondary, () => {GroundDeathManager.OnGroundMainSecondaryDeath +=  UnlockAndChange;} ) },
            {DeathTarget.CONTROL_PAD,( NonChildrenFull, () => {GroundDeathManager.OnControlPadsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.GROUND_SIDE_PRIMARY, ( GroundPrimary, () => {GroundDeathManager.OnGroundSidePrimaryDeath +=  UnlockAndChange;} ) },
            {DeathTarget.GROUND_SIDE_SECONDARY, ( GroundSecondary, () => {GroundDeathManager.OnGroundSideSecondaryDeath +=  UnlockAndChange;} ) },
            {DeathTarget.CONTROL_STAND,( NonChildrenFull, () => {GroundDeathManager.OnControlStandsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SLIDER_RECHARGE_STATION,( ChildrenFull, () => {GroundDeathManager.OnSliderRechargeStationDeath +=  UnlockAndChange;} ) },
            {DeathTarget.TURRET_PILLAR,( NonChildrenFull, () => {GroundDeathManager.OnTurretPillarsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.CONTROL_HEAD,( NonChildrenFull, () => {GroundDeathManager.OnControlHeadsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.HARPOON_CONTROL_HEAD,( ChildrenFull, () => {GroundDeathManager.OnHarpoonControlHeadsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SLIDER_RECHARGE_HEAD,( NonChildrenFull, () => {GroundDeathManager.OnSliderRechargeHeadsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.TURRET_STATION,( ChildrenFull, () => {GroundDeathManager.OnTurretStationsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SLIDER_HEAD,( ChildrenFull, () => {GroundDeathManager.OnSliderHeadDeath +=  UnlockAndChange;} ) },
            {DeathTarget.HARPOON_HEAD,( ChildrenFull, () => {GroundDeathManager.OnHarpoonHeadDeath +=  UnlockAndChange;} ) },
            {DeathTarget.TURRET_HEAD,( ChildrenFull, () => {GroundDeathManager.OnTurretHeadsDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SHIELD_ADAPTER,( NonChildrenFull, () => {GroundDeathManager.OnShieldAdapterDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SHIELD_STORAGE,( ChildrenFull, () => {GroundDeathManager.OnShieldStorageDeath +=  UnlockAndChange;} ) },
            {DeathTarget.SHIELD_EMITTER,( NonChildrenFull, () => {GroundDeathManager.OnShieldEmitterDeath +=  UnlockAndChange;} ) },
            {DeathTarget.TOKEN_TRANSPORTER,( NonChildrenFull, () => {GroundDeathManager.OnTokenTransporterDeath +=  UnlockAndChange;} ) },
        };



        target_actions_dict[death_target].manager_listener?.Invoke();

        color_change = target_actions_dict[death_target].color_change_type;



    }

    [SerializeField] DeathTarget death_target;

    Action color_change;







    void UnlockAndChange()
    {
        locked = false;
        color_change();
    }


    void _UTIL_FILL_MATERIALS(Renderer renderer)
    {
        Material[] mats = renderer.materials;

        Array.Fill(mats, changing_mat);

        renderer.materials = mats;
    }


    void NonChildrenFull()
    {

        _UTIL_FILL_MATERIALS(rend);



    }



    void GroundPrimary()
    {
        int primary_index = 1;
        Material[] mats = rend.materials;
        mats[primary_index] = changing_mat;
        rend.materials = mats;



    }

    void GroundSecondary()
    {
        int secondary_index = 0;
        Material[] mats = rend.materials;
        mats[secondary_index] = changing_mat;
        rend.materials = mats;
    }
    void ChildrenFull()
    {

        if (transform.TryGetComponent<Renderer>(out var rend)) _UTIL_FILL_MATERIALS(rend);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Renderer>(out var child_rend)) _UTIL_FILL_MATERIALS(child_rend); ;
        }

    }











}
