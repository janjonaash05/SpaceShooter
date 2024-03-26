using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static MaterialIndexHolder;
using static UnityEngine.GraphicsBuffer;



public enum BombDestructionType {MANUAL, AUTO, TARGET, BLACK_HOLE }

public class DamageBomb : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float min_scale_down_size, scale_down_increment;
    GameObject bomb;

    public bool DisabledRewards { get; set; }
    CancellationTokenSource cancel_source;
    protected CancellationToken token;
    void Start()
    {
        bomb = gameObject;



        cancel_source = new();
        token = cancel_source.Token;

    }



    private void Awake()
    {
        HelperSpawnerManager.OnBlackHoleSpawn += () => scale_down_increment /= 10;
    }




    // Update is called once per frame

    private void OnDestroy()
    {
        cancel_source.Cancel();
        cancel_source.Dispose();
    }





    public virtual async Task StartDamage( BombDestructionType bombDestructionType)
    {



        



        int  score = GetComponent<IScoreEnumerable>().ScoreReward();

        Debug.Log(score + " score");

        UICommunication.Raise_ScoreChange(bombDestructionType switch { BombDestructionType.MANUAL => score, BombDestructionType.AUTO => (score > 0) ? 1:0, BombDestructionType.TARGET => 0 , _ => 0});



        Destroy(GetComponent<Collider>());

        if (bombDestructionType != BombDestructionType.BLACK_HOLE) 
        {
            Destroy(GetComponent<BombFall>());
            await GetComponent<BombColorChange>().CoverInColor();

            
        }

        //   Action damage = (bombDestructionType == BombDestructionType.TARGET) ? DamageByTarget : () => DamageByPlayer(bombDestructionType);


        Action damageMethod = bombDestructionType switch
        {
            BombDestructionType.TARGET => DamageByTarget,
            BombDestructionType.MANUAL => () => DamageByPlayer(bombDestructionType),
            BombDestructionType.AUTO => () => DamageByPlayer(bombDestructionType),
            BombDestructionType.BLACK_HOLE => () => {  var _ = DamageByBlackHole(); }

        }; ;

        damageMethod();


    }






    async Task DamageByBlackHole()
    {
        var scaledown = ScaleDown(token);

        await scaledown;
        Destroy(gameObject);

    }


    void DamageByTarget()
    {






        Vector3 target = GameObject.FindGameObjectWithTag(Tags.BOMB_TARGET).transform.position;

        Vector3 rotationDirection = (target - transform.position);

        transform.rotation = Quaternion.LookRotation(rotationDirection);

        //  Debug.DrawRay(transform.position, rotationDirection, Color.red, 1f);

        _ = ScaleDown(token);

        transform.GetChild(1).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();

       
        //  StartCoroutine(DamageByCore_FallIntoCore());


        Destroy(gameObject, transform.GetChild(1).GetComponent<ParticleSystem>().main.duration);


    }


    void DamageByPlayer(BombDestructionType bombDestructionType)
    {
        
        Destroy(GetComponent<Renderer>());

        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }




    /*

    IEnumerator DamageByCore_FallIntoCore()
    {


        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 target = GameObject.FindGameObjectWithTag(Tags.CORE).transform.position;



        while (Vector3.Distance(rb.position,target) > 0.1)
        {
            rb.MovePosition(rb.position + (target - rb.position) * Time.fixedDeltaTime * 0.1f);
            yield return new WaitForFixedUpdate();

        }
    }
    */



    protected async Task ScaleDown(CancellationToken token)
    {

        while (bomb.transform.localScale.x > min_scale_down_size)
        {
            if (token.IsCancellationRequested) { break; }
            bomb.transform.localScale += (-scale_down_increment * Vector3.one);




            await Task.Yield();
        }






    }


}



