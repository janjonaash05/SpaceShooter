using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SliderBulletCollision : MonoBehaviour
{



    public int DamagePotential;


    /// <summary>
    /// Calls an action based on the hit object's tag, then destroys this gameObject.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {



        Action<Collision> action = other.transform.tag switch
        {
            Tags.DISRUPTOR => DisruptorAction,
            Tags.STAR => StarAction,
            _ => (_) => { }


        };

        action(other);


        Destroy(gameObject);

    }

    /// <summary>
    /// <para>Attempts to damage the Disruptor with DamagePotential.If the DamagePotential is equal to the DISRUPTOR_START_HEALTH, returns.<para>
    /// <para>Gets the Disruptor's damage particle system, plays it and disables it when it ends. </para>
    /// </summary>
    /// <param name="other"></param>
    void DisruptorAction(Collision other)
    {

        if (other.gameObject.TryGetComponent(out DamageDisruptor damageDisruptor))
        {
            damageDisruptor.Damage(DamagePotential);
        }

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



        }
        catch { }
    }


    /// <summary>
    /// Destroys the Star's collider and calls Destroy on the DestroyStar component.
    /// </summary>
    /// <param name="other"></param>
    void StarAction(Collision other) 
    {
        Destroy(other.gameObject.GetComponent<Collider>());
        other.gameObject.GetComponent<DestroyStar>().Destroy();
    }









}
