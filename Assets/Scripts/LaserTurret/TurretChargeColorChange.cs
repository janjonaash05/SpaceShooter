using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeColorChange : MonoBehaviour
{
    
    [SerializeField] int ID;

    Renderer rend;
    Material off_mat;




    /// <summary>
    /// Sets the renderer material to either off_mat or arg mat, based on the arg turn_off.
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="turn_off"></param>
    void AssignMats(Material mat, bool turn_off)
    {
        rend.material = (turn_off) ? off_mat : mat;

    }




    private void OnDestroy()
    {
        channel.OnTurretChargeColorChange -= AssignMats;

    }

    LaserTurretChannel channel;


    /// <summary>
    /// Assigns essential variables, sets the renderer material to either the turret head charge 1 or 2 material, based on ID.
    /// </summary>
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

}
