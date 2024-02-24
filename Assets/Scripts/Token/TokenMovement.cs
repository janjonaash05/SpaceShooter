using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokenMovement : MonoBehaviour
{


    public enum TokenType {FRIENDLY, ENEMY }

    public static event Action OnEnemyTokenProcedure;
    public static event Action OnFriendlyTokenProcedure;

    public enum TokenDirection {TRANSPORTER, CENTER, HARPOON_STATION };


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






    int HP = 4;


    public event Action<int> OnHealthDecrease;

    Transform[] transporter_transforms;
    Transform center_transform;
    Transform harpoon_station_transform;



    TokenDirection dir;

    Vector3 target;

    [SerializeField] float speed;



    void Start()
    {

        transporter_transforms = GameObject.FindGameObjectsWithTag(Tags.TOKEN_TRANSPORT).Select(x => x.transform).ToArray();

        target = transporter_transforms[UnityEngine.Random.Range(0,4)].position; ;

        Debug.LogError(transporter_transforms.Length + " transportLength");


        center_transform = GameObject.FindWithTag(Tags.TOKEN_CENTER).transform;

        harpoon_station_transform = GameObject.FindWithTag(Tags.HARPOON_STATION).transform;
        dir = TokenDirection.TRANSPORTER;
    }

    void Update()
    {



        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);


        float edgeDistance = dir switch
        {
            TokenDirection.CENTER => 0.001f,
            TokenDirection.TRANSPORTER => 1f,
            TokenDirection.HARPOON_STATION => 1.5f,
            _ => 0
        };


        if (Vector3.Distance(transform.position, target) < edgeDistance) 
        {
            Action toExecute = dir switch
            {
                TokenDirection.TRANSPORTER => () => 
                {
                    dir = TokenDirection.CENTER;
                    target = center_transform.position; 
                    
                    HP--;
                    OnHealthDecrease?.Invoke(HP);
                    if (HP == 0)
                    {
                        Destroy(gameObject);
                    }
                    transform.position = transporter_transforms[UnityEngine.Random.Range(0, 4)].position;
                }
                ,
                TokenDirection.CENTER => () => 
                { 
                    dir = TokenDirection.TRANSPORTER; 
                    target = transporter_transforms[UnityEngine.Random.Range(0, 4)].position;
                }
                ,
                TokenDirection.HARPOON_STATION => () => 
                {
                    Destroy(gameObject);
                }
                ,
                _ => () => { }
            };

            toExecute();
        }

       
        

        /*

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
        */
    }


    public void Stop() 
    {
        speed = 0;
        dir = TokenDirection.HARPOON_STATION;
        target = harpoon_station_transform.position;
        
    
    }

 

}
