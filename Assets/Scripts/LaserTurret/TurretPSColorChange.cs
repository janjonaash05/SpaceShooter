using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPSColorChange : MonoBehaviour
{


    [SerializeField] int ID;
    void Start()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystemRenderer ps_rend = ps.GetComponent<ParticleSystemRenderer>();



        LaserTurretCommunicationChannels.GetChannelByID(ID).OnTurretChargeColorChange += (mat, turn_off) => { ps_rend.material = mat; ps_rend.trailMaterial = mat; };

  
    }


}
