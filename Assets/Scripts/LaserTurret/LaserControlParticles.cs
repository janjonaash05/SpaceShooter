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




        switch (ID)
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnControlDisabled -= EnableParticles;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled -= DisableParticles;


                break;

            case 2:

                LaserTurretCommunicationChannels.Channel2.OnControlDisabled -= EnableParticles;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled -= DisableParticles;


                break;

        }
    }






    void Start()
    {
        ps = transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();
        ps_emission = ps.emission;


        SpinnerChargeUp.OnLaserShotPlayerDeath += LaserShotPlayerDeath;


        switch (ID)
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += EnableParticles;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += DisableParticles;

                mats_storage = MaterialHolder.Instance().COLOR_SET_1();

                break;

            case 2:

                LaserTurretCommunicationChannels.Channel2.OnControlDisabled += EnableParticles;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += DisableParticles;


                mats_storage = MaterialHolder.Instance().COLOR_SET_2();
                break;

        }


    }


    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;
    ParticleSystem.EmissionModule ps_emission;



    void EnableParticles() { AudioManager.PlayActivitySound(AudioManager.ActivityType.TURRET_CONTROLS_DISABLED); ps_emission.enabled = true; StartCoroutine(ColorChange()); }

    void DisableParticles() { AudioManager.StopActivitySound(AudioManager.ActivityType.TURRET_CONTROLS_DISABLED); ps_emission.enabled = false; StopAllCoroutines(); }


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


    
    void Update()
    {

    }
}
