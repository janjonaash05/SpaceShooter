using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPSColorChange : MonoBehaviour
{



    ParticleSystem ps;

    ParticleSystemRenderer ps_rend;

    void Start()
    {
        CoreCommunication.OnBombFallen += Play;

        ps = GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();

    }



    private void OnDestroy()
    {
        CoreCommunication.OnBombFallen -= Play;
    }





    /// <summary>
    /// <para>Plays either SHIELD_PASS or SHIELD_BLOCK sound, based on if the SHIELD_CAPACITY is 0.  </para>
    /// <para>Sets particle system renderer and trail renderer material to arg m.</para>
    /// <para>Enables emission and plays the particle system.</para>
    /// </summary>
    /// <param name="m"></param>
    void Play(Material m)
    {
        AudioManager.PlayActivitySound(CoreCommunication.SHIELD_CAPACITY == 0? AudioManager.ActivityType.SHIELD_PASS : AudioManager.ActivityType.SHIELD_BLOCK    );

        

        ps_rend.material = m;
        ps_rend.trailMaterial = m;

        var emission = ps.emission;

        emission.enabled = true;


        ps.Play();


    }


}
