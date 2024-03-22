using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUpgradeFaceSwitch : HarpoonFaceSwitch
{


    protected readonly Dictionary<int, int> order_index_dict = new() { { 1, 2 }, { 2, 5 }, { 3, 3 }, { 4, 4 } };






    readonly List<UpgradesManager.UpgradeType> upgrades_list = new()
    {UpgradesManager.UpgradeType.TURRET_CAPACITY, UpgradesManager.UpgradeType.TURRET_RECHARGE, UpgradesManager.UpgradeType.SLIDER_RECHARGE,UpgradesManager.UpgradeType.SLIDER_SPEED };


    UpgradesManager.UpgradeType current_upgrade;




    

    protected override void Start() 
    {
     
        base.Start();




        ShowUpgradeDegree();

        PlayerInputCommunication.OnUpgradeStationArrowDownClick += (_) => { ArrowDown(); ShowUpgradeDegree(); };

        PlayerInputCommunication.OnUpgradeStationArrowUpClick += (_) => { ArrowUp(); ShowUpgradeDegree(); };

        PlayerInputCommunication.OnUpgradeStationClick += (_) =>
        {
            if (UICommunication.Tokens > 0 && UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade] < UpgradesManager.MAX_VALUE)
            {
                UpgradesManager.IncreaseValue(current_upgrade);
                UICommunication.Raise_TokenChange(-1);
                ShowUpgradeDegree();
            }
        };


        AssignFaceRenderers(StationType.UPGRADE);

       

    }




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
        transform.GetChild(0).GetComponent<Renderer>().materials = mats;


    }


   
}
