using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHeadColorChange : MonoBehaviour
{
    


    Renderer rend;
    [SerializeField] int ID;
    void Start()
    {

        rend = GetComponent<Renderer>();

        var channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.OnGeneralTargetingStart += ActivateColor;
        channel.OnGeneralTargetingEnd += DeactivateColor;




        off_mat = rend.materials[2]; ;
    }


    Material off_mat;




    /// <summary>
    /// Gets the on material from the child, assigns it at a specific index.
    /// </summary>
    void ActivateColor()
    {
        Material on_mat = transform.GetChild(0).GetComponent<Renderer>().material;
        rend.materials = new Material[] { rend.materials[0], rend.materials[1], on_mat };
    }


    /// <summary>
    /// Assigns the off material at the specific index.
    /// </summary>
    void DeactivateColor()
    {

        rend.materials = new Material[] { rend.materials[0], rend.materials[1], off_mat };
    }
}
