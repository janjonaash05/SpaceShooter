using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GroundDeathColorChange : MonoBehaviour
{
    // Start is called before the first frame update








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
        CONTROL_HEAD, HARPOON_CONTROL_HEAD, SLIDER_RECHARGE_HEAD, TURRET_STATION, SIDE_UTILITY_HEAD,
        TURRET_HEAD,
        SHIELD_ADAPTER, SHIELD_STORAGE, SHIELD_EMITTER, TOKEN_TRANSPORTER

    }


    Renderer rend;
    Material changing_mat;

    bool locked = true;


    Dictionary<DeathTarget, (Action color_change, Action manager_listener)> target_action_dict;

    void Start()
    {
        rend = GetComponent<Renderer>();
        SpinnerColorChange.OnMaterialChange += (m) => changing_mat = m;
        target_action_dict = new()
        {

            {DeathTarget.CORE,( NonChildrenFullColorChange, () => {GroundDeathManager.OnCoreDeath += () => locked = false; }) },
            {DeathTarget.CORE_RING, ( NonChildrenFullColorChange, () => {GroundDeathManager.OnCoreRingDeath += () => locked = false; }) },
            {DeathTarget.GROUND_MAIN_PRIMARY, ( GroundPrimaryColorChange, () => {GroundDeathManager.OnGroundMainPrimaryDeath += () => locked = false; }) },
            {DeathTarget.GROUND_MAIN_SECONDARY, ( GroundSecondaryColorChange, () => {GroundDeathManager.OnGroundMainSecondaryDeath += () => locked = false; }) },
            {DeathTarget.CONTROL_PAD,( NonChildrenFullColorChange, () => {GroundDeathManager.OnControlPadsDeath += () => locked = false; }) },
            {DeathTarget.GROUND_SIDE_PRIMARY, ( GroundPrimaryColorChange, () => {GroundDeathManager.OnGroundSidePrimaryDeath += () => locked = false; }) },
            {DeathTarget.GROUND_SIDE_SECONDARY, ( GroundSecondaryColorChange, () => {GroundDeathManager.OnGroundSideSecondaryDeath += () => locked = false; }) },


        };
    }

    [SerializeField] List<DeathTarget> death_targets;
    List<Action> DeathActions;






    void Update()
    {

    }





    void NonChildrenFullColorChange()
    {

        Material[] mats = rend.materials;

        Array.Fill(mats, changing_mat);

        rend.materials = mats;



    }



    void GroundPrimaryColorChange()
    {
        int primary_index = 1;




        Material[] mats = rend.materials;

        mats[primary_index] = changing_mat;

        rend.materials = mats;



    }

    void GroundSecondaryColorChange()
    {
        int secondary_index = 0;




        Material[] mats = rend.materials;

        mats[secondary_index] = changing_mat;

        rend.materials = mats;



    }







}
