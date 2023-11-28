using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControlColorChange : ControlColorChange
{
    // Start is called before the first frame update
    [SerializeField] GameObject auto_collider;

   




    [SerializeField] int ID;


    private readonly List<int> COLOR_INDEXES = new() { 2, 3, 4, 5 };
    public const int AUTO_INDEX = 6;


    bool turned_off = false;

    List<Color> pre_turnoff_emission_color_list;


    public override void Start()
    {
        base.Start();
      // station_decrease_power.OnRechargeStart += DisableAutoTargeting;
       // station_decrease_power.OnRechargeEnd += EnableAutoTargeting;


        switch (ID)
        {
            case 1:
                LaserTurretCommunication1.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunication1.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunication1.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunication1.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunication1.OnControlDisabled += TurnOff;
                LaserTurretCommunication1.OnControlEnabled += TurnOn;


                break;

            case 2:

                LaserTurretCommunication2.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunication2.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunication2.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunication2.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunication2.OnControlDisabled += TurnOff;
                LaserTurretCommunication2.OnControlEnabled += TurnOn;






                break;

        }





        //      GetComponent<LaserControlDisableManager>().OnDisabled += TurnOff;
        //    GetComponent<LaserControlDisableManager>().OnEnabled += TurnOn;

        pre_turnoff_emission_color_list = new();

        foreach (var mat in GetComponent<Renderer>().materials)
        {

            pre_turnoff_emission_color_list.Add(mat.GetColor(EMISSION_COLOR));
        }
    }


    void TurnOff()
    {
        turned_off = true;



        StopAllCoroutines();

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];

        for (int i = 0; i < mats.Length; i++)
        {


            mats[i] = GetComponent<Renderer>().materials[i];


            if (COLOR_INDEXES.Contains(i) || i == AUTO_INDEX)
            {
                mats[i].SetColor(EMISSION_COLOR, block_material.GetColor(EMISSION_COLOR));

                Debug.Log(mats[i].GetColor(EMISSION_COLOR));
                continue;

            }



        }


        GetComponent<Renderer>().materials = mats;



    }

    void TurnOn()
    {

        turned_off = false;
        Material[] mats = GetComponent<Renderer>().materials;

        for (int i = 0; i < mats.Length; i++)
        {





            mats[i].SetColor(EMISSION_COLOR, pre_turnoff_emission_color_list[i]);






        }

        GetComponent<Renderer>().materials = mats;

    }

    void DisableAutoTargeting()
    {

        auto_collider.GetComponent<Renderer>().material = block_material;


        Debug.Log("DisableAutoTargeting");


        Renderer rend = GetComponent<Renderer>();

        Material[] mats = rend.materials;
        mats[AUTO_INDEX].SetColor(EMISSION_COLOR, block_material.GetColor(EMISSION_COLOR));


        rend.materials = mats;

    }










    void EnableAutoTargeting()
    {
        auto_collider.GetComponent<Renderer>().material = allow_material;
        if (turned_off) { return; }

        Debug.Log("ableAutoTargeting");

        Renderer rend = GetComponent<Renderer>();


        Material[] mats = rend.materials;
        mats[AUTO_INDEX].SetColor(EMISSION_COLOR, allow_material.GetColor(EMISSION_COLOR));

        rend.materials = mats;


    }

}
