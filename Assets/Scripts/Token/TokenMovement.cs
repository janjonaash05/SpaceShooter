using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMovement : MonoBehaviour
{



    /// <summary>
    /// CENTER:
    /// 
    /// 
    /// pos: Vector3(32.3651352,21.8995304,3.24249268e-05)
    /// rot: Vector3(315,270,180)
    /// scale: Vector3(0.1,0.1,0.1)
    /// 
    /// 
    /// 
    /// TRANSPORT TOP-LEFT
    /// 
    /// pos: Vector3(26.5435314,27.7211418,8.60595512)
    /// rot: Vector3(315,270,180)
    /// 
    /// 
    /// </summary>

    bool headingCenter = false;



    int HP = 4;


    public event Action<int> OnHealthDecrease;

   [SerializeField] Transform[] transporter_transforms;

   [SerializeField] Transform center_transform;



    Vector3 target;

    [SerializeField] float speed;



    void Start()
    {
        target = transporter_transforms[UnityEngine.Random.Range(0,4)].position; ;
        headingCenter = false;
        
    }

    void Update()
    {

        

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float edgeDistance = headingCenter ? 0.001f : 1f;
        if (Vector3.Distance(transform.position, target) < edgeDistance) 
        {

            if (headingCenter)
            {

                target = transporter_transforms[UnityEngine.Random.Range(0, 4)].position;
                headingCenter = false;

            }
            else 
            {
                target = center_transform.position;
                transform.position = transporter_transforms[UnityEngine.Random.Range(0, 4)].position;
                headingCenter = true;


                HP--;

                OnHealthDecrease?.Invoke(HP);


                if (HP == 0)
                {
                    Destroy(gameObject);
                }

            }


        }
    }


    public void Stop() 
    {
        speed = 0;
        Destroy(this);
    
    }

 

}
