using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonUpgradeFaceSwitch : MonoBehaviour
{
    List<Renderer> faces_rend;

    int index = 0;




    Material on_mat, off_mat;


    List<UpgradesManager.UpgradeType> upgrades_list = new()
    {UpgradesManager.UpgradeType.TURRET_CAPACITY, UpgradesManager.UpgradeType.TURRET_RECHARGE, UpgradesManager.UpgradeType.SLIDER_RECHARGE,UpgradesManager.UpgradeType.SLIDER_SPEED };



    UpgradesManager.UpgradeType current_upgrade;



    Dictionary<int, int> order_index_dict = new() { { 1, 2 }, { 2, 5 }, { 3, 3 }, { 4, 4 } };

    void Start()
    {
        on_mat = transform.GetChild(0).GetComponent<Renderer>().materials[^1];

        off_mat = transform.GetChild(0).GetComponent<Renderer>().materials[^2];






        ShowUpgradeDegree();


        PlayerInputCommunication.OnUpgradeStationArrowDownClick += (_) => { index -= (index == 0) ? 0 : 1; ShowFace(); ShowUpgradeDegree(); };
        PlayerInputCommunication.OnUpgradeStationArrowUpClick += (_) => { index += (index == faces_rend.Count - 1) ? 0 : 1; ShowFace(); ShowUpgradeDegree(); };

        PlayerInputCommunication.OnUpgradeStationClick += (_) =>
        {
            if (UICommunication.Tokens > 0 && UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade] < UpgradesManager.MAX_VALUE)
            {


                UpgradesManager.IncreaseValue(current_upgrade);

                UICommunication.Raise_TokenChange(-1);
                ShowUpgradeDegree();
            }





        };


        faces_rend = new();

        foreach (Transform child in transform)
        {
            if (child.CompareTag(Tags.UPGRADE_FACE))
            {
                faces_rend.Add(child.GetComponent<Renderer>());

            }
        }


    }




    void ShowUpgradeDegree()
    {






        current_upgrade = upgrades_list[index];


        int degree = UpgradesManager.UPGRADE_VALUE_DICT[current_upgrade];


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



    void ShowFace()
    {
        foreach (Renderer r in faces_rend)
        {
            r.enabled = false;

        }


        faces_rend[index].enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
