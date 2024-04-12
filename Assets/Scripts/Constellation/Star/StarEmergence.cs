using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StarEmergence : MonoBehaviour, IEMPDisruptable
{
    // Start is called before the first frame update


     Vector3 target_scale;
    void Start()
    {
      

    }


    private void Awake()
    {


        target_scale = transform.localScale;

        Emerge(1f);
       
        
    }



  




    public void OnEMP()
    {
        StopAllCoroutines();
    }

 


   public void RotateTowardsPlayer()
    {



        /*
        Vector3 rotationDirection = (Camera.main.transform.position - transform.position);

        Quaternion targetRot = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        */



          transform.rotation = Quaternion.LookRotation(GameObject.FindWithTag(Tags.PLAYER).transform.position - transform.position);




        transform.Rotate(Vector3.up, 90);





        transform.localScale = Vector3.zero;



    }


   





    //FIX
    void Emerge(float duration) 
    {
        IEnumerator scaleChange()
        {

            float lerp = 0f;
            Vector3 original = Vector3.zero;
            Vector3 target = target_scale;



            while (lerp < duration)
            {



                lerp += Time.deltaTime;
                transform.localScale = Vector3.Lerp(original, target, lerp / duration);
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
