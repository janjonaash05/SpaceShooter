using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using UnityEngine;

public class EMPFlash : MonoBehaviour
{



    Material on_mat;
    Renderer rend;


    Vector3 default_scale;

    Material[]  default_mats;

  readonly  Dictionary<int, int> order_index_dict = new() { {1,3 }, {2,2 }, {3,0 }, {4,1 } };


    private void Start()
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.EMP_SPAWN);
    }



    /// <summary>
    /// Assigns necessary variables, starts ScaleUpDown and ColorChange coroutines. 
    /// </summary>
    void Awake()
    {
        on_mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();

        rend = GetComponent<Renderer>();

        default_mats = new Material[] { rend.materials[0], rend.materials[1], rend.materials[2], rend.materials[3] };

        
        default_scale = transform.localScale;



        StartCoroutine(ScaleUpDown());
        StartCoroutine(ColorChange());



    }

   

    /// <summary>
    /// Changes each material entry to on mat over time
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorChange() 
    {
        
        for (int i = 1; i <= 4; i++)
        {
            Material[] mats = rend.materials;

            Array.Copy(default_mats, mats, mats.Length);

            for (int j = i; j >=1; j--)
            {
                mats[order_index_dict[j]] = on_mat;
            }

            rend.materials = mats;

            yield return new WaitForSeconds(HelperSpawnerManager.LIFETIME/4);
        }


    
    }


    /// <summary>
    /// Yields ScaleChange from 0 to target, waits, then ScaleChange from target to 0.
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUpDown()
    {


        float scale_duration = HelperSpawnerManager.LIFETIME / 8f;


        yield return StartCoroutine(ScaleChange(Vector3.zero, default_scale, scale_duration));
        yield return new WaitForSeconds(scale_duration * 6f);
        yield return StartCoroutine(ScaleChange(default_scale, Vector3.zero, scale_duration));



    }


    /// <summary>
    /// LERPs localScale over a duration.
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator ScaleChange(Vector3 original, Vector3 target, float duration)
    {

        float lerp = 0f;

        while (lerp < duration)
        {

            lerp += Time.deltaTime;
            transform.localScale = Vector3.Lerp(original, target, lerp / duration);
            yield return null;

        }





    }

 












}
