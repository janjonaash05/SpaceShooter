using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisruptor : MonoBehaviour
{


    float health;
    void Start()
    {
        health = DifficultyManager.DISRUPTOR_START_HEALTH;
    }







    /// <summary>
    /// Subtracts from health, if health is 0, Destroys this object and engages in DestroyDisruptor object.
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(float amount)
    {

        health -= amount;


        if (health <= 0)
        {
            Destroy(this);
            GetComponent<DestroyDisruptor>().Engage();

        }

    }







}
