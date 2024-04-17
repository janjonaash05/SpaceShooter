using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using UnityEngine;

public class EMPFlash : MonoBehaviour
{
    


    Material on_mat, off_mat;
    Renderer rend;



    Material[] default_mat;


    Vector3 default_scale;



    Material[] color_mats, default_mats;

  readonly  Dictionary<int, int> order_index_dict = new() { {1,3 }, {2,2 }, {3,0 }, {4,1 } };




    void Awake()
    {
        on_mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();
        off_mat = MaterialHolder.Instance().FRIENDLY_SECONDARY();


        color_mats = new Material[] { MaterialHolder.Instance().FRIENDLY_UPGRADE(), MaterialHolder.Instance().FRIENDLY_UPGRADE(), MaterialHolder.Instance().FRIENDLY_UPGRADE()};

        rend = GetComponent<Renderer>();



        default_mats = new Material[] { rend.materials[0], rend.materials[1], rend.materials[2], rend.materials[3] };

        
        default_scale = transform.localScale;

      






       // Array.Copy(GetComponent<Renderer>().materials, default_mats, default_mats.Length);


        StartCoroutine(ScaleUpDown());
        StartCoroutine(ColorChange());


        // StartCoroutine(ChangeColorOverTime());

    }

    
    void Update()
    {

    }


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

    IEnumerator ScaleUpDown()
    {


        float scale_duration = HelperSpawnerManager.LIFETIME / 8f;

        Debug.LogError("ds "+ scale_duration);

       // GetComponent<Renderer>().materials = color_mats;
        yield return StartCoroutine(ScaleChange(Vector3.zero, default_scale, scale_duration));
        //GetComponent<Renderer>().materials = default_mat;
        yield return new WaitForSeconds(scale_duration * 6f);
       // GetComponent<Renderer>().materials = color_mats;
        yield return StartCoroutine(ScaleChange(default_scale, Vector3.zero, scale_duration));



    }



    IEnumerator ScaleChange(Vector3 original, Vector3 target, float duration)
    {






        float lerp = 0f;

        while (lerp < duration)
        {

            lerp += Time.deltaTime;
            transform.localScale = Vector3.Lerp(original, target, lerp / duration);
            Debug.LogError(transform.localScale + "local scale");
            yield return null;

        }





    }

 












}
