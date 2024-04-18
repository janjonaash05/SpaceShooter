using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPSColorChange : MonoBehaviour
{
    




    void Start()
    {



        CoreCommunication.OnBombFallen += Play;







    }



    private void OnDestroy()
    {
        CoreCommunication.OnBombFallen -= Play;
    }




    void Play(Material m)
    {




        AudioManager.PlayActivitySound(CoreCommunication.SHIELD_CAPACITY == 0? AudioManager.ActivityType.SHIELD_PASS : AudioManager.ActivityType.SHIELD_BLOCK    );

        ParticleSystem ps = GetComponent<ParticleSystem>();

        ParticleSystemRenderer ps_rend = ps.GetComponent<ParticleSystemRenderer>();

        ps_rend.material = m;
        ps_rend.trailMaterial = m;

        var emission = ps.emission;

        emission.enabled = true;


        ps.Play();


    }









    
    void Update()
    {

    }
}
