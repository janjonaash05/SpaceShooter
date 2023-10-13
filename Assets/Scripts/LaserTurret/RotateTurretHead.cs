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
    bool lockedOn = false;
    void Start()
    {
        turret_head_charge = transform.GetChild(0).gameObject;
        GetComponent<TargetBomb>().OnTargetingEnd += DisengageShootMode;
        GetComponent<TargetBomb>().OnTargetingStart += EngageShootMode;
    }

    
    void Update()
    {
        if(!lockedOn){
          transform.Rotate(rotation * Time.deltaTime);
          turret_head_charge.transform.Rotate(-rotation * charge_speed_multiplier* Time.deltaTime);
        }

    
    }




    public void EngageShootMode(){

      rotation = rotation * ShootModeMultiplier;
       


    }

    public void DisengageShootMode(){


      rotation = rotation / ShootModeMultiplier;
    }





    
}
