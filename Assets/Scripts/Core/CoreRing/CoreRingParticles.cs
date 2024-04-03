using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRingParticles : MonoBehaviour 
{




    CoreRingColorChange colorChange;






    ParticleSystemRenderer ps_renderer;
    ParticleSystem ps;



    void ParentValueChangedCore() 
    {

        ps.emissionRate = CoreCommunication.CORE_INDEX_HOLDER.Parent switch
        {

            0 => 0,
            1 => 10,
            2 => 50,
            3 => 200,
            4 => 300,
            5 => 500,



        };
    }




    void Start()
    {
        colorChange = GetComponent<CoreRingColorChange>();
        
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_renderer = ps.GetComponent<ParticleSystemRenderer>();




        ps.enableEmission = true;



        CoreCommunication.OnParentValueChangedCore += ParentValueChangedCore;
    }


    private void OnDestroy()
    {
        CoreCommunication.OnParentValueChangedCore -= ParentValueChangedCore;
    }









    void Update()
    {
        ps_renderer.material = colorChange.changing_mat;
        ps_renderer.trailMaterial = colorChange.changing_mat;
    }
}
