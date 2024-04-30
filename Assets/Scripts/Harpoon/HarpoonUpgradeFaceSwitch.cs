using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUpgradeFaceSwitch : HarpoonFaceSwitch
{


    protected readonly Dictionary<int, int> order_index_dict = new() { { 1, 2 }, { 2, 5 }, { 3, 3 }, { 4, 4 } };






    readonly List<UpgradesManager.UpgradeType> upgrades_list = new()
    {UpgradesManager.UpgradeType.TURRET_CAPACITY, UpgradesManager.UpgradeType.TURRET_RECHARGE, UpgradesManager.UpgradeType.SLIDER_RECHARGE,UpgradesManager.UpgradeType.SLIDER_SPEED };


    UpgradesManager.UpgradeType current_upgrade;


    private void OnDestroy()
    {
        PlayerInputCommunication.OnUpgradeStationArrowDownClick -= UpgradeStationArrowDownClick;

        PlayerInputCommunication.OnUpgradeStationArrowUpClick -= UpgradeStationArrowUpClick;

        PlayerInputCommunication.OnUpgradeStationClick -= UpgradeStationClick;
    }



    protected override void Start()
    {

        base.Start();




        ShowUpgradeDegree();





        PlayerInputCommunication.OnUpgradeStationArrowDownClick += UpgradeStationArrowDownClick;

        PlayerInputCommunication.OnUpgradeStationArrowUpClick += UpgradeStationArrowUpClick;

        PlayerInputCommunication.OnUpgradeStationClick += UpgradeStationClick;


        AssignFaceRenderers(StationType.UPGRADE);



    }



    /// <summary>
    /// Plays the UPGRADE_STATION_CLICK sound, calls  ArrowDown() and ShowUpgradeDegree().
    /// </summary>
    /// <param name="_"></param>
    void UpgradeStationArrowDownClick(RaycastHit _)
    {


        AudioManager.PlayActivitySound(AudioManager.ActivityType.UPGRADE_STATION_CLICK);
        ArrowDown(); ShowUpgradeDegree();

    }

    /// <summary>
    /// Plays the UPGRADE_STATION_CLICK sound, calls  ArrowUp() and ShowUpgradeDegree().
    /// </summary>
    /// <param name="_"></param>
    void UpgradeStationArrowUpClick(RaycastHit _)
    {

        AudioManager.PlayActivitySound(AudioManager.ActivityType.UPGRADE_STATION_CLICK);
        ArrowUp(); ShowUpgradeDegree();

    }



    /// <summary>
    /// If the Token amount is greater than 0 and the current upgrade is not at max:
    /// <para>Calls IncreaseValue with the current upgrade, calls Raise_TokenChange with a -1 value and calls ShowUpgradeDegree().</para>
    /// </summary>
    /// <param name="_"></param>
    void UpgradeStationClick(RaycastHit _) 
    {
        if (UICommunication.Tokens > 0 && UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade] < UpgradesManager.MAX_VALUE)
        {

          
            UpgradesManager.IncreaseValue(current_upgrade);
            UICommunication.Raise_TokenChange(-1);
            ShowUpgradeDegree();
        }

    }


    /// <summary>
    /// Assigns the current_upgrade and the degree.
    /// <para>Assigns the on material based on whether the degree is at max value or not.</para>
    /// <para>Sets all materials in the index order dictionary. to off, then changes some to on according to the degree. Changes the materials on the arrow up/down index to their appropriate colors.</para>
    /// <para>Assigns the materials to the child renderer.</para>
    /// </summary>
    void ShowUpgradeDegree()
    {
        current_upgrade = upgrades_list[face_index];


        int degree =   UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade];


        Material on = degree == UpgradesManager.MAX_VALUE ? MaterialHolder.Instance().FRIENDLY_UPGRADE() : on_mat;
        Material[] mats = transform.GetChild(0).GetComponent<Renderer>().materials;


        for (int i = 1; i <= UpgradesManager.MAX_VALUE; i++)
        {
            mats[order_index_dict[i]] = off_mat;


        }

        for (int i = 1; i <= degree; i++)
        {

            mats[order_index_dict[i]] = on;

        }


        mats[ARROW_DOWN_COLOR_INDEX] = GetArrowDownColor();
        mats[ARROW_UP_COLOR_INDEX] = GetArrowUpColor();


        transform.GetChild(0).GetComponent<Renderer>().materials = mats;


    }


   
}
