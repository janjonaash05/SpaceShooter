using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliderBulletCollision : MonoBehaviour
{
    // Start is called before the first frame update


    public int DamagePotential;



    void DisableEmission(ref bool emission_enabled)
    {
        emission_enabled = false;
    }



    private void OnCollisionEnter(Collision other)
    {

        //  Destroy(other.gameObject.GetComponent<DisruptorMovement>());
        //  Destroy(other.gameObject.GetComponent<DisruptorColorChange>());


        Debug.Log(DamagePotential + "dmg");

        switch (other.transform.tag)
        {
            case Tags.DISRUPTOR:






                other.gameObject.GetComponent<DamageDisruptor>().Damage(DamagePotential);


                if (DamagePotential == DifficultyManager.DISRUPTOR_START_HEALTH)
                {
                    return;
                }

                var system = other.transform.GetChild(2).GetComponent<ParticleSystem>();
                var emission = system.emission;
                emission.enabled = true;

                system.Play();
                Invoke(nameof(DisableEmission), system.main.duration);







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
