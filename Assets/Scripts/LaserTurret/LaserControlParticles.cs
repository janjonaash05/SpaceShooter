using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControlParticles : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int ID;




    Material changing_mat;

    Material[] mats_storage;



    void Start()
    {
        ps = transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();
        ps_emission = ps.emission;

        



        switch (ID)
        {
            case 1:
                LaserTurretCommunication1.OnControlDisabled += EnableParticles;
                LaserTurretCommunication1.OnControlEnabled += DisableParticles;

                mats_storage = MaterialHolder.Instance().COLOR_SET_1();

                break;

            case 2:

                LaserTurretCommunication2.OnControlDisabled += EnableParticles;
                LaserTurretCommunication2.OnControlEnabled += DisableParticles;


                mats_storage = MaterialHolder.Instance().COLOR_SET_2();
                break;

        }


    }


    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;
    ParticleSystem.EmissionModule ps_emission;



    void EnableParticles() { ps_emission.enabled = true; StartCoroutine(ColorChange()); }

    void DisableParticles() { ps_emission.enabled = false; StopAllCoroutines(); }


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


    // Update is called once per frame
    void Update()
    {

    }
}