using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControlParticles : MonoBehaviour
{
    

    [SerializeField] int ID;




    Material changing_mat;

    Material[] mats_storage;





    void LaserShotPlayerDeath() => ps.startSize = 0;


    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= LaserShotPlayerDeath;

        channel.OnControlDisabled -= EnableParticles;
        channel.OnControlEnabled -= DisableParticles;



    }




    LaserTurretChannel channel;

    void Start()
    {
        ps = transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();
        ps_emission = ps.emission;


        SpinnerChargeUp.OnLaserShotPlayerDeath += LaserShotPlayerDeath;

        channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.OnControlDisabled += EnableParticles;
        channel.OnControlEnabled += DisableParticles;


        mats_storage = ID == 1 ? MaterialHolder.Instance().COLOR_SET_1() : MaterialHolder.Instance().COLOR_SET_2();







    }



    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;
    ParticleSystem.EmissionModule ps_emission;


    /// <summary>
    /// Plays the TURRET_CONTROLS_DISABLED sound, enables the particle system emission, starts the ColorChange coroutine.
    /// </summary>
    void EnableParticles() { AudioManager.PlayActivitySound(AudioManager.ActivityType.TURRET_CONTROLS_DISABLED); ps_emission.enabled = true; StartCoroutine(ColorChange()); }

    /// <summary>
    /// Stops the TURRET_CONTROLS_DISABLED sound, disables the particle system emission, stops all coroutines.
    /// </summary>
    void DisableParticles() { AudioManager.StopActivitySound(AudioManager.ActivityType.TURRET_CONTROLS_DISABLED); ps_emission.enabled = false; StopAllCoroutines(); }



    /// <summary>
    /// Endlessly loops over all materials in mat storage and sets them as the particle system's renderer, with a set time delay.
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorChange()
    {
        while (true)
        {

            foreach (Material m in mats_storage)
            {
                ps_rend.material = m;
                yield return new WaitForSeconds(0.5f);

            }

        }


    }

}
