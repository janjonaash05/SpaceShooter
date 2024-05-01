using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurretHead : MonoBehaviour
{
    

    GameObject turret_head_charge;
    Vector3 rotation;
    const float CHARGE_SPEED_MULT = 3;
    const  float SHOOT_MODE_MULT = 600;


    [SerializeField] int ID;

    Vector3 idleRotation;
    Vector3 activeRotation;


    void Start()
    {
        rotation = new(100,100, ID == 1 ? 100: -100);

        idleRotation = rotation;
        activeRotation = rotation + Vector3.one* SHOOT_MODE_MULT;

      


        turret_head_charge = transform.GetChild(0).gameObject;
        GetComponent<TargetBomb>().OnTargetingEnd += DisengageShootMode;
        GetComponent<TargetBomb>().OnTargetingStart += EngageShootMode;
    }

    /// <summary>
    /// Rotates the transform and the charge in opposite directions.
    /// </summary>
    void Update()
    {
      
        transform.Rotate(rotation * Time.deltaTime);
        turret_head_charge.transform.Rotate(CHARGE_SPEED_MULT * Time.deltaTime * -rotation);



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
