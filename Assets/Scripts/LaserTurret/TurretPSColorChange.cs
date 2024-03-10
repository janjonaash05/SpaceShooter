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







        switch (ID) 
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnTurretChargeColorChange += (mat, turn_off) => { ps_rend.material = mat; ps_rend.trailMaterial = mat; };
                break;
            case 2:
                LaserTurretCommunicationChannels.Channel2.OnTurretChargeColorChange += (mat, turn_off) => { ps_rend.material = mat; ps_rend.trailMaterial = mat; };
                break;
        
        
        
        
        
        
        
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
