using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRingParticles : MonoBehaviour 
{




    CoreRingColorChange colorChange;






    ParticleSystemRenderer ps_renderer;
    ParticleSystem ps;

    void Start()
    {
        colorChange = GetComponent<CoreRingColorChange>();
        
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_renderer = ps.GetComponent<ParticleSystemRenderer>();




        ps.enableEmission = true;



        CoreCommunication.OnCoreFullParticlesStart += () => ps.enableEmission = true;
        CoreCommunication.OnCoreFullParticlesEnd += () => ps.enableEmission = false;
    }









    void Update()
    {
        ps_renderer.material = colorChange.changing_mat;
    }
}
