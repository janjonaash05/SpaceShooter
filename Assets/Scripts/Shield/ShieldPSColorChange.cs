using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPSColorChange : MonoBehaviour
{
    // Start is called before the first frame update



    
    void Start()
    {



        CoreCommunication.OnBombFallen += (m) =>
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();

            ParticleSystemRenderer ps_rend = ps.GetComponent<ParticleSystemRenderer>();

            ps_rend.material = m;
            ps_rend.trailMaterial = m;

            var emission = ps.emission;

            emission.enabled = true;

            ps.Play();






        };






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
