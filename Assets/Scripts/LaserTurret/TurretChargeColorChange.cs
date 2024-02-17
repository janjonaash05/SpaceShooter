using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int ID;

    Renderer rend;
    Material off_mat;

    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;

    void Start()
    {

        rend = GetComponent<Renderer>();
        off_mat = rend.material;


        if (transform.parent.childCount > 1)
        {

             ps = transform.parent.GetChild(1).GetComponent<ParticleSystem>();
             ps_rend = ps.GetComponent<ParticleSystemRenderer>();
        }

        /*TODO*/

        switch (ID)
        {

            case 1:
                LaserTurretCommunicationChannels.Channel1.OnTurretChargeColorChange += (mat, turn_off) => { rend.material = (turn_off) ? off_mat : mat; };


                if (transform.parent.childCount > 1) LaserTurretCommunicationChannels.Channel1.OnTurretChargeColorChange += (mat, turn_off) =>
                {

                    Debug.Log(transform.parent.childCount);



                    ps_rend.material = mat;
                    ps_rend.trailMaterial = mat;



                };
                break;
            case 2:
                LaserTurretCommunicationChannels.Channel2.OnTurretChargeColorChange += (mat, turn_off) => rend.material = (turn_off) ? off_mat : mat;


                if (transform.parent.childCount > 1) LaserTurretCommunicationChannels.Channel2.OnTurretChargeColorChange += (mat, turn_off) =>
                {

                 

                    ps_rend.material = mat;
                    ps_rend.trailMaterial = mat;




                };
                break;


        }







    }
}
