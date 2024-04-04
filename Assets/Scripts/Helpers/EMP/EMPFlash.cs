using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using UnityEngine;

public class EMPFlash : MonoBehaviour
{
    // Start is called before the first frame update


    Material on_mat, off_mat;
    Renderer rend;



    Material[] default_mats;


    Vector3 default_scale;



    Material[] color_mats;

    void Awake()
    {
        on_mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();
        off_mat = MaterialHolder.Instance().FRIENDLY_SECONDARY();


       color_mats = new Material[] { MaterialHolder.Instance().FRIENDLY_UPGRADE(), MaterialHolder.Instance().FRIENDLY_UPGRADE(), MaterialHolder.Instance().FRIENDLY_UPGRADE()};

        rend = GetComponent<Renderer>();


        default_mats = new Material[3];
        default_scale = transform.localScale;


        Array.Copy(GetComponent<Renderer>().materials, default_mats, default_mats.Length);


        StartCoroutine(ScaleUpDown());




        // StartCoroutine(ChangeColorOverTime());

    }

    // Update is called once per frame
    void Update()
    {

    }




    IEnumerator ScaleUpDown()
    {


        float scale_duration = 0.5f;



        GetComponent<Renderer>().materials = color_mats;
        yield return StartCoroutine(ScaleChange(Vector3.zero, default_scale, scale_duration));
        GetComponent<Renderer>().materials = default_mats;
        yield return new WaitForSeconds(HelperSpawnerManager.LIFETIME - (scale_duration * 2));
        GetComponent<Renderer>().materials = color_mats;
        yield return StartCoroutine(ScaleChange(default_scale, Vector3.zero, scale_duration));



    }



    IEnumerator ScaleChange(Vector3 original, Vector3 target, float duration)
    {






        float lerp = 0f;

        while (lerp < duration)
        {

            lerp += Time.deltaTime;
            transform.localScale = Vector3.Lerp(original, target, lerp / duration);
            Debug.LogError(transform.localScale);
            yield return null;

        }





    }

 












}
