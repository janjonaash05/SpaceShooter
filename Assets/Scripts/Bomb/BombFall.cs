using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFall : MonoBehaviour, IScoreEnumerable
{


     float move_speed;
    Vector3 rotation_speed;
    [SerializeField] float rotation_speed_multiplier;


    [SerializeField] float min_speed, max_speed;

    public bool DisabledRewards { get; set; }

    Rigidbody rb;

   



    bool scaled_up = false;

    public void SetScaledUp() => scaled_up = true;

    // Start is called before the first frame update


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag(Tags.BOMB_TARGET).transform.position + Random.insideUnitSphere; //  core.transform.position + Random.insideUnitSphere;
       
    }



    Vector3 target;
    void Start()
    {
        DisabledRewards = false;

        move_speed = Random.Range(min_speed, max_speed);

        rotation_speed = move_speed * 500 * Random.insideUnitSphere;
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
        rb.MovePosition(rb.position + move_speed * Time.fixedDeltaTime * (target - rb.position));


    }


    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.BOMB_TARGET))
        { 
            Destroy(gameObject.GetComponent<BombFall>());
            _ = gameObject.GetComponent<DamageBomb>().StartDamage(false);




            CoreCommunication.Raise_ValueChange(0, 1);
        }
    }








    public int ScoreReward()
    {
        if (DisabledRewards) { return 0; }
        return Mathf.RoundToInt(transform.localScale.x / 50  + VectorSum(rotation_speed) / 75 +  move_speed * 75);
    }


    float VectorSum(Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);


    }
}
