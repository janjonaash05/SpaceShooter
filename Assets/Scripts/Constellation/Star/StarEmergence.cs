using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEmergence : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] Vector3 initScale, target_scale;
    void Start()
    {
        RotateTowardsPlayer();
      //  Emerge();
      ScaleChange(emergeCondition);

        HelperSpawnerManager.OnEMPSpawn += OnEMP;
    }



    void OnEMP()
    {
        StopAllCoroutines();
        ScaleChange(shrivelCondition);
    }

    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMP;
    }


    void RotateTowardsPlayer()
    {
        Vector3 rotationDirection = (Camera.main.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(rotationDirection);
        transform.Rotate(Vector3.up, 90);

        transform.localScale = initScale;



    }


    public void Emerge()
    {




        IEnumerator emerge()
        {

            float lerp = 0f;
            while (transform.localScale.z < target_scale.z)
            {

                lerp += Time.deltaTime / 100;
                transform.localScale = Vector3.Lerp(transform.localScale, target_scale, lerp);
                yield return null;

            }



        }

        StartCoroutine(emerge());
    }


    delegate (bool scale_change_condition, bool destroy_after) ScaleChangeCondition(float val1, float val2);


    ScaleChangeCondition emergeCondition = (v1, v2) => (v1 < v2, false);
    ScaleChangeCondition shrivelCondition = (v1, v2) => (v1 > v2, true);




    void ScaleChange(ScaleChangeCondition condition) 
    {
        IEnumerator scaleChange()
        {

            float lerp = 0f;
            Vector3 original_scale = initScale;
            while (condition(transform.localScale.z, target_scale.z).scale_change_condition)
            {

                lerp += Time.deltaTime /100;
                transform.localScale = Vector3.Lerp(original_scale, target_scale, lerp);
                yield return null;

            }



            if (condition(0, 0).destroy_after) { Destroy(gameObject); }


        }

        StartCoroutine(scaleChange());



    }





    public void Shrivel()
    {



        IEnumerator shrivel()
        {

            float lerp = 0f;
            target_scale = Vector3.zero;
            Vector3 original_scale = transform.localScale;
            while (transform.localScale.z > target_scale.z)
            {

                lerp += Time.deltaTime / 100;
                transform.localScale = Vector3.Lerp(original_scale, target_scale, lerp);
                yield return null;

            }

            Destroy(gameObject);

        }

        StartCoroutine(shrivel());
    }



    // Update is called once per frame
    void Update()
    {

    }
}
