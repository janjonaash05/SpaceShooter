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
        StartCoroutine(EmissionRateGrowShrink());
    }


    private void Start()
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.BLACK_HOLE_SPAWN);
    }


    Vector3 rot_speed;





   



    /// <summary>
    /// LERPs localScale from original to target size over a duration
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
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







    /// <summary>
    /// LERPs emission rateOverTime from original to target size over a duration
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
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






    /// <summary>
    /// Yields ChangeScaleOverTime()  twice, from zero to target scale, then from target scale to zero.
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUpDown()
    {

        Vector3 target_scale = transform.localScale;


        yield return StartCoroutine(ChangeScaleOverTime(Vector3.zero, target_scale, HelperSpawnerManager.LIFETIME / 2));
        yield return StartCoroutine(ChangeScaleOverTime(target_scale, Vector3.zero, HelperSpawnerManager.LIFETIME / 2));

    }




    /// <summary>
    /// Yields ChangeEmissionRateOverTime() twice, from zero to target rate, then from target rate to zero.
    /// </summary>
    /// <returns></returns>

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
