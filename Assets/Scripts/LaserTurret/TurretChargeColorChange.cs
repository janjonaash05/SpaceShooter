using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeColorChange : MonoBehaviour
{
    
    [SerializeField] int ID;

    Renderer rend;
    Material off_mat;





    void AssignMats(Material mat, bool turn_off)
    {
        rend.material = (turn_off) ? off_mat : mat;

    }




    private void OnDestroy()
    {
        LaserTurretCommunicationChannels.Channel1.OnTurretChargeColorChange -= AssignMats;
        LaserTurretCommunicationChannels.Channel2.OnTurretChargeColorChange -= AssignMats;


    }


    private void Awake()
    {

        rend = GetComponent<Renderer>();
        off_mat = rend.material;



        switch (ID)
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnTurretChargeColorChange += AssignMats;
                break;
            case 2:
                LaserTurretCommunicationChannels.Channel2.OnTurretChargeColorChange += AssignMats;
                break;
        }






        try
        {
            rend.material = GameObject.FindWithTag((ID == 1) ? Tags.TURRET_HEAD_CHARGE_1 : Tags.TURRET_HEAD_CHARGE_2).GetComponent<Renderer>().material;
        }
        catch
        {
        }

    }


    void Start()
    {

      






    }
}
