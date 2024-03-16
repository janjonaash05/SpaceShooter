using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenMovement : MonoBehaviour
{


    public enum TokenType { FRIENDLY, ENEMY }

    public static event Action OnEnemyTokenProcedure;
    public static event Action OnFriendlyTokenProcedure;

    public enum TokenDirection { TRANSPORTER, CENTER, HARPOON_STATION };



    ParticleSystem ps_caught, ps_destroyed;



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


    public enum TokenSpeed
    {
        SLOW = 5, MEDIUM = 7, FAST = 9


    }



    int HP = 4;


    public event Action<int> OnHealthDecrease;




    TokenSpeed speed;
    [SerializeField] TokenType type;
    TokenDirection dir;

    Vector3 target;





    void Awake()
    {




        ps_caught = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_destroyed = transform.GetChild(1).GetComponent<ParticleSystem>();






        speed = UnityEngine.Random.Range(0, 3) switch { 0 => TokenSpeed.SLOW, 1 => TokenSpeed.MEDIUM, 2 => TokenSpeed.FAST, _=> TokenSpeed.SLOW};

        dir = TokenDirection.CENTER;
        target = TokenSpawning.center_transform.position;
    }

    void Update()
    {



        transform.position = Vector3.MoveTowards(transform.position, target, (int)speed * Time.deltaTime);



        float edgeDistance = dir switch
        {
            TokenDirection.CENTER => 0.001f,
            TokenDirection.TRANSPORTER => 0.5f,
            TokenDirection.HARPOON_STATION => 4.5f,
            _ => 0
        };




        Debug.Log(Vector3.Distance(transform.position, target) + " "+dir);

        if (Vector3.Distance(transform.position, target) < edgeDistance)
        {
            Action toExecute = dir switch
            {
                TokenDirection.TRANSPORTER => () =>
                {
                    dir = TokenDirection.CENTER;
                    target = TokenSpawning.center_transform.position;

                    HP--;
                    OnHealthDecrease?.Invoke(HP);
                    if (HP == 0)
                    {
                        StartCoroutine(PlayDestroyed());
                        return;
                    }
                    transform.position = TokenSpawning.transporter_collider_transforms[UnityEngine.Random.Range(0, 4)].position;
                }
                ,
                TokenDirection.CENTER => () =>
                {
                    dir = TokenDirection.TRANSPORTER;
                    target = TokenSpawning.transporter_collider_transforms[UnityEngine.Random.Range(0, 4)].position;
                }
                ,
                TokenDirection.HARPOON_STATION => () =>
                {
                    StartCoroutine(PlayCaught());
                }
                ,
                _ => () => { }
            };

            toExecute();
        }




       
    }


    public void Stop()
    {
        speed = 0;
        dir = TokenDirection.HARPOON_STATION;
        target = TokenSpawning.harpoon_station_transform.position;


    }




    IEnumerator PlayPS(ParticleSystem ps) 
    {

        ps.enableEmission = true;
        ps.Play();
     //   Debug.LogError("PLAYING PS waiting "+ps.main.duration) ;
        yield return new WaitForSeconds(ps.main.duration);
    
    
    
    }

    IEnumerator PlayCaught() 
    {




        Debug.LogError("CAUGHT, ACTIVE PS "+ ps_caught.emission.enabled);
        if (ps_caught.emission.enabled) { yield break; }
        if (type == TokenType.FRIENDLY)
        {
            UICommunication.Raise_TokenChange(1);
        }

        StartCoroutine(Shrink());

        yield return StartCoroutine( PlayPS(ps_caught));

       

        Destroy(gameObject);

    
    }


    IEnumerator PlayDestroyed()
    {
        speed = 0;

        if (ps_destroyed.emission.enabled) yield break;
        if (type == TokenType.ENEMY)
        {
          
        }

        StartCoroutine(Shrink());

        yield return StartCoroutine(PlayPS(ps_destroyed));


       

        Destroy(gameObject);

        

    }

    readonly float min_scale_down_size = 0.0001f;
    readonly float scale_down_increment_width = 0.1f;
    readonly float scale_down_increment_length = 0.1f / 5f;

    IEnumerator Shrink() 
    {
        while (transform.localScale.x > min_scale_down_size)
        {
            transform.localScale += new Vector3(-scale_down_increment_width, -scale_down_increment_length, -scale_down_increment_width);


            yield return null;
        }

    }



}
