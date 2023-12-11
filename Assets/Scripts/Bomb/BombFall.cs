using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFall : MonoBehaviour, IScoreEnumerable
{


    [SerializeField] Vector3 move_speed;
    Vector3 rotation_speed;
    [SerializeField] float rotation_speed_multiplier;
    

    [SerializeField] float min_down, max_down, min_side, max_side;

    public bool DisabledRewards { get; set; }

    Rigidbody rb;

    GameObject core;

    // Start is called before the first frame update


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        core = GameObject.FindWithTag(Tags.CORE);


        target = core.transform.position + Random.insideUnitSphere;
       
    }



    Vector3 target;
    void Start()
    {





        DisabledRewards = false;




        move_speed = new Vector3(Random.Range(min_side, max_side), 0, -Random.Range(min_down, max_down));

        rotation_speed = rotation_speed_multiplier * Random.insideUnitSphere;
    }

    // Update is called once per frame

    private void OnDestroy()
    {rb.constraints=
        RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {


        //rb.MovePosition(rb.position + Time.fixedDeltaTime *move_speed  );


        rb.MovePosition(rb.position +    (target-rb.position) * Time.fixedDeltaTime*0.1f);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Time.fixedDeltaTime *rotation_speed));


    }


    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.CORE))
        {

        
            Destroy(gameObject.GetComponent<BombFall>());
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(false);



          //  spinner.GetComponent<SpinnerColorChange>().ChangeIndexHolder(0, 1);
        //    core_ring.GetComponent<CoreRingColorChange>().DecreaseDegree();



            CoreCommunication.Raise_ValueChange(0, 1);







        }
    }


    public int ScoreReward()
    {
        if (DisabledRewards) { return 0; }
        return Mathf.RoundToInt(transform.localScale.x / 50 + VectorSum(move_speed) / 75 + VectorSum(rotation_speed) / 75);
    }


    float VectorSum(Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);


    }
}
