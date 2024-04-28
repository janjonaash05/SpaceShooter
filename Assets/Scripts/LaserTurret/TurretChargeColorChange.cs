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
        channel.OnTurretChargeColorChange -= AssignMats;

    }

    LaserTurretChannel channel;
    private void Awake()
    {

        rend = GetComponent<Renderer>();
        off_mat = rend.material;

        channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.OnTurretChargeColorChange += AssignMats;

     





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
