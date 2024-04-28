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
    


    [SerializeField] float min_scale_down_size, scale_down_increment;


    public bool DisabledRewards { get; set; }
    CancellationTokenSource cancel_source;
    protected CancellationToken token;
    void Start()
    {
        



        cancel_source = new();
        token = cancel_source.Token;

    }



    void BlackHoleSpawn() => scale_down_duration = 2f;



    private void Awake()
    {
        HelperSpawnerManager.OnBlackHoleSpawn += BlackHoleSpawn;

       
    }




    

    private void OnDestroy()
    {

        HelperSpawnerManager.OnBlackHoleSpawn -= BlackHoleSpawn;



        cancel_source.Cancel();
        cancel_source.Dispose();
    }




    /// <summary>
    /// <para>Gets the ScoreReward, Raises ScoreChange with the score, modified based on the destruction type.</para>
    /// <para>Destroys the collider, and if the destruction type isn't BLACK_HOLE, destroys BombFall and awaits CoverInColor().</para>
    /// <para>Executes a method based on the destruction type from the dictionary.</para>
    /// </summary>
    /// <param name="bombDestructionType"></param>
    /// <returns></returns>
    public virtual async Task StartDamage( BombDestructionType bombDestructionType)
    {


        int  score = GetComponent<IScoreEnumerable>().ValidateScoreReward();


        UICommunication.Raise_ScoreChange(bombDestructionType switch { BombDestructionType.MANUAL => score, BombDestructionType.AUTO => (score > 0) ? 1:0, BombDestructionType.TARGET => 0 , _ => 0});



        Destroy(GetComponent<Collider>());

        if (bombDestructionType != BombDestructionType.BLACK_HOLE) 
        {
            Destroy(GetComponent<BombFall>());
            await GetComponent<BombColorChange>().CoverInColor();

            
        }



        Action damageMethod = bombDestructionType switch
        {
            BombDestructionType.TARGET => DamageByTarget,
            BombDestructionType.MANUAL => () => DamageByPlayer(bombDestructionType),
            BombDestructionType.AUTO => () => DamageByPlayer(bombDestructionType),
            BombDestructionType.BLACK_HOLE => () => {  var _ = DamageByBlackHole(); }

        }; ;

        damageMethod();


    }





    /// <summary>
    /// Awaits ScaleDown and destroys the gameObject.
    /// </summary>
    /// <returns></returns>
    async Task DamageByBlackHole()
    {
        var scaledown = ScaleDown(token);

        await scaledown;
        Destroy(gameObject);

    }

    /// <summary>
    /// Starts ScaleDown, plays the dissolve particle system, awaits its end and destroys the gameObject.
    /// </summary>
    void DamageByTarget()
    {


        _ = ScaleDown(token);

        transform.GetChild(1).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();

       


        Destroy(gameObject, transform.GetChild(1).GetComponent<ParticleSystem>().main.duration);


    }

    /// <summary>
    /// Destroys the renderer, plays the explode particle system, awaits its end and destroys the gameObject.
    /// </summary>
    /// <param name="bombDestructionType"></param>
    void DamageByPlayer(BombDestructionType bombDestructionType)
    {
        
        Destroy(GetComponent<Renderer>());

        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }





    float scale_down_duration = 0.2f;

    /// <summary>
    /// LERPS the localScale from start to 0 ver a set amount of time.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    protected async Task ScaleDown(CancellationToken token)
    {
        float duration = scale_down_duration;
        Vector3 start_size = transform.localScale;
        float lerp = 0;

        while (lerp < duration) 
        {
            if (token.IsCancellationRequested) { break; }
            lerp += Time.deltaTime;

            transform.localScale = Vector3.Lerp(start_size, Vector3.zero,lerp / duration); ;


            await Task.Yield();

        }
    }


}



