using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurretHead : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject turret_head_charge;
    public Vector3 rotation;
    public float charge_speed_multiplier;
    public float ShootModeMultiplier;




    Vector3 idleRotation;
    Vector3 activeRotation;
    //  bool lockedOn = false;
    void Start()
    {


        idleRotation = rotation;
        activeRotation = rotation + Vector3.one* ShootModeMultiplier;

      


        turret_head_charge = transform.GetChild(0).gameObject;
        GetComponent<TargetBomb>().OnTargetingEnd += DisengageShootMode;
        GetComponent<TargetBomb>().OnTargetingStart += EngageShootMode;
    }


    void Update()
    {
        //    if(!lockedOn){
        transform.Rotate(rotation * Time.deltaTime);
        turret_head_charge.transform.Rotate(charge_speed_multiplier * Time.deltaTime * -rotation);
        //  }


    }


    public void EngageShootMode()
    {
        rotation = activeRotation;
    }

    public void DisengageShootMode()
    {
        rotation = idleRotation;
    }






}
