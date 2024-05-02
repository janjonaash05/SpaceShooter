using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StarEmergence : MonoBehaviour, IEMPDisruptable
{



    Vector3 target_scale;



    private void Awake()
    {


        target_scale = transform.localScale;

        Emerge(1f);


        AudioManager.PlayActivitySound(AudioManager.ActivityType.STAR_SPAWN);




    }








    public void OnEMP()
    {
        StopAllCoroutines();
    }



    /// <summary>
    /// Aligns and adjusts the transform rotation so that it faces the player, sets localScale to 0.
    /// </summary>
    public void InitAndRotateTowardsPlayer()
    {


        transform.rotation = Quaternion.LookRotation(GameObject.FindWithTag(Tags.PLAYER).transform.position - transform.position);

        transform.Rotate(Vector3.up, 90);
 
        transform.localScale = Vector3.zero;



    }








    /// <summary>
    /// LERPs the localScale from 0 to target over arg duration.
    /// </summary>
    /// <param name="duration"></param>
    void Emerge(float duration)
    {
        IEnumerator scaleChange()
        {

            float lerp = 0f;
            Vector3 target = target_scale;



            while (lerp < duration)
            {



                lerp += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, target, lerp / duration);
                yield return null;

            }




        }

        StartCoroutine(scaleChange());



    }





    public void Shrivel()
    {



        IEnumerator shrivel()
        {

            float lerp = 0f;
            float duration = 0.35f;

            Vector3 original_scale = transform.localScale;




            while (lerp < duration)
            {

                lerp += Time.deltaTime;
                transform.localScale = Vector3.Lerp(original_scale, Vector3.zero, lerp / duration);
                yield return null;

            }

            Destroy(gameObject);

        }

        StartCoroutine(shrivel());
    }


}
