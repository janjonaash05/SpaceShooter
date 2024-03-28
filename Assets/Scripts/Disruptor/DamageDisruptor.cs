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
