using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{

    [SerializeField] Material white;
    [SerializeField] float min_scale_down_size, scale_down_increment;


    /// <summary>
    /// Destroys StarFall, fills the Renderer materials with white.
    /// </summary>
    void CoverInWhite()
    {
        TryGetComponent(out StarFall b);
        Destroy(b);


        Material[] mats = new Material[GetComponent<Renderer>().materials.Length]; 
        Array.Fill(mats, white);
        GetComponent<Renderer>().materials = mats;

       

    }





    /// <summary>
    /// <para>Plays the STAR_DESTROYED sound.</para>
    /// <para>Destroys StarChargeUp, StarEmergence and calls to CoverInColor().</para>
    /// <para>Raises Score Change.</para>
    /// <para>Starts ScaleDown.</para>
    /// <para>Plays the particle system, waits for it to finish and destroys the gameObject.</para>
    /// </summary>
    public void Destroy()
    {




        AudioManager.PlayActivitySound(AudioManager.ActivityType.STAR_DESTROYED);
        Destroy(GetComponent<StarChargeUp>());
        Destroy(GetComponent<StarEmergence>());
        CoverInWhite();

        // ScoreCounter.Increase();

        UICommunication.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ValidateScoreReward());

        _ = ScaleDown();



        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }


    /// <summary>
    /// LERPs the localScale from start to zero over time, destroys the gameObject after.
    /// </summary>
    /// <returns></returns>
    async Task ScaleDown()
    {



        float lerp = 0;
        float duration = 0.5f;

        Vector3 start = transform.localScale;


        while (transform.localScale.y > min_scale_down_size)
        {
            lerp += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, lerp / duration);
            
            await Task.Yield();
        }

        Destroy(gameObject);






    }





}
