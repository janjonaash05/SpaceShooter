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
        HelperSpawnerManager.OnEMPSpawn += OnEMP;
    }
    // Update is called once per frame
    void Update()
    {

    }



    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMP;
    }




    

    void OnEMP()
    {








        EMPDisrupt();
        StopAllCoroutines();
        CoverInColor();
        ScaleChange();


    }



    void EMPDisrupt() 
    {
        var disruptions = GetComponents<IEMPDisruptable>();

        foreach (var _ in disruptions)
        {
            _.OnEMP();   
        }

    }



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
