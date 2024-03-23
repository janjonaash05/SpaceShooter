using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFall : MonoBehaviour, IScoreEnumerable
{


    public float MoveSpeed { get; private set; }
    Vector3 rotation_speed;
    [SerializeField] float rotation_speed_multiplier;


    [SerializeField] float min_speed, max_speed;

    public bool DisabledRewards { get; set; }

    Rigidbody rb;


    public event Action<float> OnMoveSpeedSet;


    bool scaled_up = false;

    public void SetScaledUp() => scaled_up = true;

    // Start is called before the first frame update


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

        MoveSpeed = UnityEngine.Random.Range(min_speed, max_speed);
        OnMoveSpeedSet?.Invoke(MoveSpeed);


        rotation_speed = MoveSpeed * 500 * UnityEngine.Random.insideUnitSphere;
    }

    // Update is called once per frame

    private void OnDestroy()
    {rb.constraints=
        RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {
        

        //rb.MovePosition(rb.position + Time.fixedDeltaTime *move_speed  );


        
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Time.fixedDeltaTime *rotation_speed));


        if (!scaled_up) return;
        rb.MovePosition(rb.position + MoveSpeed * Time.fixedDeltaTime * (target - rb.position));


    }


    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.BOMB_TARGET))
        {
            Destroy(gameObject.GetComponent<BombFall>());
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(BombDestructionType.TARGET);


            CoreCommunication.Raise_OnBombFallen(GetComponent<BombColorChange>().bomb_color);


        }
        else if (col.transform.CompareTag(Tags.BLACK_HOLE)) 
        {
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(BombDestructionType.BLACK_HOLE);

        }

        
    }








    public int ScoreReward()
    {
        if (DisabledRewards) { return 0; }

        DisabledRewards = true;

        return Mathf.RoundToInt(transform.localScale.x / 50  + VectorSum(rotation_speed) / 75 +  MoveSpeed * 75);
    }


    float VectorSum(Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);


    }
}
