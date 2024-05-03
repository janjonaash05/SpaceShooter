using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenMovement : MonoBehaviour, IScoreEnumerable
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

    Transform target;



    void StopOnDeath()
    {
        speed = 0;
    }


    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= StopOnDeath;
    }



    Dictionary<TokenDirection, Action> token_collision_action_dict;

    void Awake()
    {
        token_collision_action_dict = new()
        {
            {TokenDirection.TRANSPORTER, TransporterCollision },
            {TokenDirection.CENTER, CenterCollision },
            {TokenDirection.HARPOON_STATION, HarpoonStationCollision },


        };


        SpinnerChargeUp.OnLaserShotPlayerDeath += StopOnDeath;



        ps_caught = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_destroyed = transform.GetChild(1).GetComponent<ParticleSystem>();


        DisabledRewards = type == TokenType.FRIENDLY;



        speed = UnityEngine.Random.Range(0, 3) switch { 0 => TokenSpeed.SLOW, 1 => TokenSpeed.MEDIUM, 2 => TokenSpeed.FAST, _ => TokenSpeed.SLOW };

        dir = TokenDirection.CENTER;
        target = TokenSpawning.CENTER_TRANSFORM;






    }

    /// <summary>
    /// Moves towards the target position at speed.
    /// <para>Calculates the edge distance based on the direction.</para>
    /// <para>When reached the edge distance, sets speed to a random one from selection and calls the collision action based on the direction.</para>
    /// </summary>
    void Update()
    {



        transform.position = Vector3.MoveTowards(transform.position, target.position, (int)speed * Time.deltaTime);



        float edgeDistance = dir switch
        {
            TokenDirection.CENTER => 0.001f,
            TokenDirection.TRANSPORTER => 0.5f,
            TokenDirection.HARPOON_STATION => 4.5f,
            _ => 0
        };





        if (Vector3.Distance(transform.position, target.position) < edgeDistance)
        {



            speed = new System.Random().Next(3) switch
            {
                0 => TokenSpeed.SLOW,
                1 => TokenSpeed.MEDIUM,
                2 => TokenSpeed.FAST

            };

            token_collision_action_dict[dir]();


        }





    }





    /// <summary>
    /// Sets the direction to CENTER.
    /// <para>Plays the TOKEN_TRANSPORTED sound. </para>
    /// <para>Decreases HP.</para>
    /// <para>If the HP reaches 0, starts the PlayDestroyed coroutine and returns.</para>
    /// <para>Sets the target as the CENTER_TRANSFORM.</para>
    /// <para>Sets the position as a random TRANSPORTER_COLLIDER_TRANSFORM.</para>
    /// </summary>
    void TransporterCollision()
    {
        dir = TokenDirection.CENTER;
        AudioManager.PlayActivitySound(AudioManager.ActivityType.TOKEN_TRANSPORTED);

        HP--;
        OnHealthDecrease?.Invoke(HP);
        if (HP == 0)
        {
            StartCoroutine(PlayDestroyed());
            return;
        }
        target = TokenSpawning.CENTER_TRANSFORM;
        transform.position = TokenSpawning.TRANSPORTER_COLLIDER_TRANSFORMS[UnityEngine.Random.Range(0, 4)].position;
    }

    /// <summary>
    /// Sets the direction to TRANSPORTER, assigns the target as a random TRANSPORTER_COLLIDER_TRANSFORM.
    /// </summary>
    void CenterCollision()
    {
        dir = TokenDirection.TRANSPORTER;
        target = TokenSpawning.TRANSPORTER_COLLIDER_TRANSFORMS[UnityEngine.Random.Range(0, 4)];
    }


    void HarpoonStationCollision() => StartCoroutine(PlayCaught());


    /// <summary>
    /// Sets speed to 0, direction to HARPOON_STATION, target to the HARPOON_STATION_TRANSFORM.
    /// </summary>
    public void Stop()
    {
        speed = 0;
        dir = TokenDirection.HARPOON_STATION;
        target = TokenSpawning.HARPOON_STATION_TRANSFORM;


    }



    /// <summary>
    /// Enables the arg particle system's emission, plays it, waits for it to finish.
    /// </summary>
    /// <param name="ps"></param>
    /// <returns></returns>
    IEnumerator PlayPS(ParticleSystem ps)
    {

        ps.enableEmission = true;
        ps.Play();
        yield return new WaitForSeconds(ps.main.duration);



    }


    /// <summary>
    /// If the caught particle system emission is already enabled, returns.
    /// <para>Plays either the TOKEN_CAUGHT_FRIENDLY or TOKEN_CAUGHT_ENEMY sound based on the TokenType.</para>
    /// <para>If the type is FRIENDLY, calls Raise_TokenChange with 1 on UICommunication.</para>
    /// <para>Starts the Shrink coroutine.</para>
    /// <para>calls Raise_ScoreChange with CalculateScoreReward() on UICommunication.</para>
    /// <para>Yields the PlayPS coroutine with the caught particle system.</para>
    /// <para>Destroys the gameObject.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayCaught()
    {




        if (ps_caught.emission.enabled) { yield break; }
        AudioManager.PlayActivitySound(type == TokenType.FRIENDLY ? AudioManager.ActivityType.TOKEN_CAUGHT_FRIENDLY : AudioManager.ActivityType.TOKEN_CAUGHT_ENEMY);

        if (type == TokenType.FRIENDLY)
        {
            UICommunication.Raise_TokenChange(1);
        }

        StartCoroutine(Shrink());
        UICommunication.Raise_ScoreChange(CalculateScoreReward());
        yield return StartCoroutine(PlayPS(ps_caught));




        Destroy(gameObject);


    }





    /// <summary>
    /// Sets speed to 0.
    /// <para>Plays either the TOKEN_DESTROYED_FRIENDLY or TOKEN_DESTROYED_ENEMY sound based on the TokenType.</para>
    /// <para>If the destroyed particle system emission is already enabled, returns.</para>
    /// <para>If the type is ENEMY, ChangeRandomDifficulty with TOKEN on DifficultyManager.</para>
    /// <para>Starts the Shrink coroutine.</para>
    /// <para>Calls SetColorDelayed on the target this gameObject's color material and the destroyed particle system duration. </para>
    /// <para>Disables this gameObject's renderer. </para>
    /// <para>Yields the PlayPS coroutine with the destroyed particle system.</para>
    /// <para>Destroys the gameObject.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDestroyed()
    {
        speed = 0;


        AudioManager.PlayActivitySound(type == TokenType.FRIENDLY ? AudioManager.ActivityType.TOKEN_DESTROYED_FRIENDLY : AudioManager.ActivityType.TOKEN_DESTROYED_ENEMY);





        if (ps_destroyed.emission.enabled) yield break;
        if (type == TokenType.ENEMY)
        {
            DifficultyManager.ChangeRandomDifficulty(DifficultyManager.AffectedFeatureCircumstance.TOKEN);
        }

        StartCoroutine(Shrink());

        target.parent.GetChild(0).GetComponent<TokenTransportColorChange>().SetColorDelayed(GetComponent<Renderer>().materials[^1], ps_destroyed.main.duration);
        GetComponent<Renderer>().enabled = false;
        yield return StartCoroutine(PlayPS(ps_destroyed));




        Destroy(gameObject);



    }




    /// <summary>
    /// LERPs the localScale from start to 0 over a set duration.
    /// </summary>
    /// <returns></returns>
    IEnumerator Shrink()
    {
        float lerp = 0;
        float duration = 0.25f;



        Vector3 start = transform.localScale;


        while (lerp < duration) 
        {
            lerp += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, lerp / duration);
            yield return null;

        }


    }


    public bool DisabledRewards { get; set; }


    public int CalculateScoreReward()
    {
        return HP * 4;
    }
}
