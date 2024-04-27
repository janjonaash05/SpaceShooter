using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AfterEMP : MonoBehaviour
{
 


    private void Awake()
    {
        HelperSpawnerManager.OnEMPSpawn += OnEMPEngage;

        SpinnerChargeUp.OnLaserShotPlayerDeath += EMPDisrupt;

    }

    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMPEngage;

        SpinnerChargeUp.OnLaserShotPlayerDeath -= EMPDisrupt;
    }

    

    void OnEMPEngage()
    {








        EMPDisrupt();
        CoverInColor();
        ScaleChange();


    }




    /// <summary>
    /// Gets all components implementing IEMPDisruptable and attempts to call OnEMP on them
    /// </summary>
    void EMPDisrupt() 
    {





        

        var disruptions = GetComponents<IEMPDisruptable>();

        foreach (var _ in disruptions)
        {
            try
            {
                _.OnEMP();
            }
            catch (Exception) { }
           
        }

    }


    /// <summary>
    /// Fills the materials array with the upgrade material and reassigns it back to the renderer
    /// </summary>
    void CoverInColor()
    {
        Material[] mats = GetComponent<Renderer>().materials;

        Material mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();


        Array.Fill(mats, mat);

        GetComponent<Renderer>().materials = mats;

    }


    /// <summary>
    /// LERPs from the original scale to zero over set amount of time, destroys GameObject after
    /// </summary>
    void ScaleChange()
    {
        IEnumerator scaleChange()
        {




            float duration = HelperSpawnerManager.LIFETIME;
            Vector3 original = transform.localScale;
            Vector3 target = Vector3.zero;

            float lerp = 0f;

            while (lerp < duration)
            {

                lerp += Time.deltaTime;
                transform.localScale = Vector3.Lerp(original, target, lerp / duration);
                yield return null;

            }



            Destroy(gameObject);


        }

        StartCoroutine(scaleChange());



    }
}
