using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AfterEMP : MonoBehaviour
{
    void Start()
    {

    }



    private void Awake()
    {
        HelperSpawnerManager.OnEMPSpawn += OnEMPEngage;

        SpinnerChargeUp.OnLaserShotPlayerDeath += OnLaserShotPlayerDeath;

    }

    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMPEngage;

        SpinnerChargeUp.OnLaserShotPlayerDeath -= OnLaserShotPlayerDeath;
    }


    




    void OnLaserShotPlayerDeath() 
    {
        EMPDisrupt();

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
            catch (Exception e) { Debug.LogError(e); }
           
        }

    }


    /// <summary>
    /// Fills the materials array with the material and reassigns it back to the renderer
    /// </summary>
    void CoverInColor()
    {
        Material[] mats = GetComponent<Renderer>().materials;

        Material mat = MaterialHolder.Instance().FRIENDLY_UPGRADE();


        Array.Fill(mats, mat);

        GetComponent<Renderer>().materials = mats;

    }



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
