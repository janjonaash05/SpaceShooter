using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Android;

public class BlackHoleRotation : MonoBehaviour
{
    

    ParticleSystem ps;


    void Awake()
    {



       


        ps = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(ScaleUpDown());
        //StartCoroutine(SpeedUpDown());
        StartCoroutine(EmissionRateGrowShrink());
        //  StartCoroutine(ParticleSizeGrowShrink());


       


    }


    private void Start()
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.BLACK_HOLE_SPAWN);
    }


    Vector3 rot_speed;





   




    IEnumerator ChangeScaleOverTime(Vector3 original, Vector3 target, float duration)
    {
        float counter = 0f;
        while (counter < duration)
        {

            counter += Time.deltaTime;

            transform.localScale = Vector3.Lerp(original, target, counter / duration);

            yield return null;
        }
    }

  






    IEnumerator ChangeEmissionRateOverTime(float original, float target, float duration)
    {


        var emission = ps.emission;
        float counter = 0f;

        while (counter < duration)
        {

            counter += Time.deltaTime;
            emission.rateOverTime = Mathf.Lerp(original, target, counter / duration);

            yield return null;
        }
    }


  




    IEnumerator ScaleUpDown()
    {

        Vector3 target_scale = transform.localScale;


        yield return StartCoroutine(ChangeScaleOverTime(Vector3.zero, target_scale, HelperSpawnerManager.LIFETIME / 2));
        yield return StartCoroutine(ChangeScaleOverTime(target_scale, Vector3.zero, HelperSpawnerManager.LIFETIME / 2));

    }


  



    IEnumerator EmissionRateGrowShrink() 
    {
        float target_rate = 100;

        yield return StartCoroutine(ChangeEmissionRateOverTime(0, target_rate, HelperSpawnerManager.LIFETIME / 4));
        yield return new WaitForSeconds(HelperSpawnerManager.LIFETIME / 2);
        yield return StartCoroutine(ChangeEmissionRateOverTime(target_rate, 0, HelperSpawnerManager.LIFETIME / 4));



    }








    
    void Update()
    {

   

        transform.Rotate(rot_speed * Time.deltaTime);
    }
}
