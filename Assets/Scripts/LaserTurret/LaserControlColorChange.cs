using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                LaserTurretCommunicationChannels.Channel1.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunicationChannels.Channel1.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += TurnOff;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += TurnOn;


                break;

            case 2:

                LaserTurretCommunicationChannels.Channel2.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunicationChannels.Channel2.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunicationChannels.Channel2.OnControlDisabled += TurnOff;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += TurnOn;






                break;

        }

      

        normal_mats = GetComponent<Renderer>().materials;

        
        auto_off_mat = MaterialHolder.Instance().TURRET_CONTROL_AUTO_COLOR_OFF();
        color_off_mat = auto_off_mat;

        off_mats = new Material[] 
        {
            normal_mats[0],normal_mats[1],
            color_off_mat,color_off_mat,
            color_off_mat,color_off_mat,
            auto_off_mat
        };

        //      GetComponent<LaserControlDisableManager>().OnDisabled += TurnOff;
        //    GetComponent<LaserControlDisableManager>().OnEnabled += TurnOn;

        pre_turnoff_emission_color_list = new();

        foreach (var mat in GetComponent<Renderer>().materials)
        {
            pre_turnoff_emission_color_list.Add(mat.GetColor(EMISSION_COLOR));
        }
    }


    Material[] normal_mats, off_mats;

    Material color_off_mat;
    Material auto_off_mat;

    void TurnOff()
    {
        turned_off = true;



        StopAllCoroutines();

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];

        GetComponent<Renderer>().materials = off_mats;

     /*   for (int i = 0; i < mats.Length; i++)
        {


            mats[i] = GetComponent<Renderer>().materials[i];


            if (COLOR_INDEXES.Contains(i) || i == AUTO_INDEX)
            {
                mats[i].SetColor(EMISSION_COLOR, block_material.GetColor(EMISSION_COLOR));

             
                continue;

            }



        }

        GetComponent<Renderer>().materials = mats;

*/

    }

    void TurnOn()
    {

        turned_off = false;
        Material[] mats = GetComponent<Renderer>().materials;

        GetComponent<Renderer>().materials = normal_mats;
        /*
        for (int i = 0; i < mats.Length; i++)
        {





            mats[i].SetColor(EMISSION_COLOR, pre_turnoff_emission_color_list[i]);






        }

        GetComponent<Renderer>().materials = mats;
        */
    }

    void DisableAutoTargeting()
    {






        StopAllCoroutines();

        auto_collider.GetComponent<Renderer>().material = block_material;


        Debug.LogWarning("DisableAutoTargeting");


        Renderer rend = GetComponent<Renderer>();

        Material[] mats = rend.materials; ;

        // mats[AUTO_INDEX].SetColor(EMISSION_COLOR, block_material.GetColor(EMISSION_COLOR));
        mats[AUTO_INDEX] = auto_off_mat;

        rend.materials = mats;

    }










    void EnableAutoTargeting()
    {
        auto_collider.GetComponent<Renderer>().material = allow_material;
        if (turned_off) { return; }

        Debug.LogWarning("ableAutoTargeting");

        Renderer rend = GetComponent<Renderer>();


        Material[] mats = rend.materials;
        // mats[AUTO_INDEX].SetColor(EMISSION_COLOR, allow_material.GetColor(EMISSION_COLOR));


        mats[AUTO_INDEX] = normal_mats[AUTO_INDEX];

        rend.materials = mats;


    }

}
