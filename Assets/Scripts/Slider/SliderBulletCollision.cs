using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SliderBulletCollision : MonoBehaviour
{
    


    public int DamagePotential;



    void DisableEmission(ref bool emission_enabled)
    {
        emission_enabled = false;
    }



    private void OnCollisionEnter(Collision other)
    {

      

        switch (other.transform.tag)
        {
            case Tags.DISRUPTOR:








                other.gameObject.TryGetComponent(out DamageDisruptor damageDisruptor);
                damageDisruptor?.Damage(DamagePotential);


                if (DamagePotential == DifficultyManager.DISRUPTOR_START_HEALTH)
                {
                    return;
                }




                try
                {
                    var system = other.transform.GetChild(2).GetComponent<ParticleSystem>();
                    var emission = system.emission;
                    emission.enabled = true;

                    system.Play();

                    IEnumerator disableEmission()
                    {
                        yield return new WaitForSeconds(system.main.duration);
                        emission.enabled = false;

                    }


                    StartCoroutine(disableEmission());



                } catch (Exception) { }

              







                break;
            case Tags.STAR:
                Destroy(other.gameObject.GetComponent<Collider>());
                other.gameObject.GetComponent<DestroyStar>().Destroy();
                break;
            default: break;




        }














        Destroy(gameObject);

    }
}
