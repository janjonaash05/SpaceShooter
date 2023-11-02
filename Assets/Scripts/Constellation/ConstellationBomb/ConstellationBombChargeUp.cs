using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationBombChargeUp : MonoBehaviour
{


    [SerializeField] float max_scale;
    [SerializeField]
    float[] rotation_speeds;

    

    Rigidbody rb;

    float scale_increment;
   public float scale_degree { get; private set; } = 0;
    // Start is called before the first frame update


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Start()
    {
        scale_increment = max_scale / 8;
        StarFallIntoBomb.OnStarFallen += AddSize;
        rotation = new Vector3
            (
           GenerateRandomRotationAxis(),

            GenerateRandomRotationAxis(),

            GenerateRandomRotationAxis()


            );




        rotation *= rotation_speeds[0];

    }




    private void OnDestroy()
    {
        StarFallIntoBomb.OnStarFallen -= AddSize;
    }


    float GenerateRandomRotationAxis()
    {
        return new System.Random().NextDouble() * new System.Random().Next(2) == 0 ? 1 : -1;
    
    
    }


    Vector3 rotation;

    // Update is called once per frame
    void FixedUpdate()
    {





        rb.MoveRotation(rb.rotation * Quaternion.Euler(Time.fixedDeltaTime * rotation_speed *rotation));


    }


    float rotation_speed = 0;
    void AddSize(Material m)
    {
        transform.localScale += Vector3.one * scale_increment;
        scale_degree++;

        switch (scale_degree)
        {
            case 0: case 1: rotation_speed = rotation_speeds[0]; ; break;
            case 2: case 3: rotation_speed = rotation_speeds[1]; ; break;
            case 4: case 5: rotation_speed = rotation_speeds[2]; ; break;
            case 6: case 7: rotation_speed = rotation_speeds[3]; ; break;
            case 8: rotation_speed = rotation_speeds[4]; ; break;

            default:  break;



        }



    }




}
