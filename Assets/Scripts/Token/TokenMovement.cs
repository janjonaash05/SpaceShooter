using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMovement : MonoBehaviour
{


    bool headingCenter = false;


   [SerializeField] Transform[] transporter_transforms;

   [SerializeField] Transform center_transform;



    Vector3 target;

    [SerializeField] float speed;

    void Start()
    {
        target = transporter_transforms[Random.Range(0,4)].position; ;
        headingCenter = false;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target) < 0.001f) 
        {

            if (headingCenter)
            {

                target = transporter_transforms[Random.Range(0, 4)].position;
                headingCenter = false;

            }
            else 
            {
                target = center_transform.position;
                transform.position = transporter_transforms[Random.Range(0, 4)].position;
                headingCenter = true;

            }


        }
    }


    

 

}
