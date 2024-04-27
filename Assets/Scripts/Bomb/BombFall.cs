using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BombType { NORMAL, CLUSTER_UNIT }
public class BombFall : MonoBehaviour, IScoreEnumerable
{




    [SerializeField] BombType bomb_type;

    public BombType BombType => bomb_type;

    public float MoveSpeed { get; private set; }
    Vector3 rotation_speed;
    [SerializeField] float rotation_speed_multiplier;




    public bool DisabledRewards { get; set; }

    Rigidbody rb;


    public event Action<float> OnMoveSpeedSet;


    bool scaled_up = false;

    public void SetScaledUp() => scaled_up = true;

    


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag(Tags.BOMB_TARGET).transform.position;//  core.transform.position + Random.insideUnitSphere;



        HelperSpawnerManager.OnBlackHoleSpawn += () => { target = HelperSpawnerManager.Instance().BlackHolePosition; MoveSpeed = 2; };

    }



    Vector3 target;
    void Start()
    {
        DisabledRewards = false;




        (float min_speed, float max_speed) = DifficultyManager.BOMBxSPEED_INTERVAL_DIFFICULTY_DICT[DifficultyManager.DIFFICULTY];
        MoveSpeed = UnityEngine.Random.Range(min_speed, max_speed);
        OnMoveSpeedSet?.Invoke(MoveSpeed);


        rotation_speed = MoveSpeed * 500 * UnityEngine.Random.insideUnitSphere;
    }

    

    private void OnDestroy()
    {
        rb.constraints =
        RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Time.fixedDeltaTime * rotation_speed));


        if (!scaled_up) return;
        rb.MovePosition(rb.position + MoveSpeed * Time.fixedDeltaTime * (target - rb.position));


    }

    /// <summary>
    /// <para>If collided with bomb target, destroys BombFall, starts its damage procedure and does a damage to player action based on the BombType.</para>
    /// <para>If collided the black hole, starts its damage procedure</para>
    /// 
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.BOMB_TARGET))
        {
            Destroy(gameObject.GetComponent<BombFall>());
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(BombDestructionType.TARGET);


            Action damageAction = bomb_type == BombType.NORMAL ? 
                () => CoreCommunication.Raise_OnBombFallen(GetComponent<BombColorChange>().BombMaterial)
                : CoreCommunication.DamageShieldOnly;
            damageAction();

        }
        else if (col.transform.CompareTag(Tags.BLACK_HOLE))
        {
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(BombDestructionType.BLACK_HOLE);

        }


    }







    /// <summary>
    /// If DisabledRewards isn't true, sets it to true and returns a calculated score reward.
    /// </summary>
    /// <returns></returns>
    public int ScoreReward()
    {
        if (DisabledRewards) { return 0; }

        DisabledRewards = true;

        return Mathf.RoundToInt(transform.localScale.x / 50 + VectorSum(rotation_speed) / 75 + MoveSpeed * 75);
    }

    /// <summary>
    /// Sums the absolute values of the xyz vector values.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float VectorSum(Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);


    }
}
