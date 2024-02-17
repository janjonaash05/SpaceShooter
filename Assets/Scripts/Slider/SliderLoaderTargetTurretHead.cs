using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class SliderLoaderTargetTurretHead : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject turret_head, turret_head_charge, turret_station;
    Material white;


    SliderLoaderRecharge loader_recharge;

    [SerializeField] float burst_speed, pulsate_speed;

    bool turret_active;

    delegate void OnDepletionAction();
    void Start()
    {

        white = MaterialHolder.Instance().SIDE_TOOLS_COLOR();


        loader_recharge = turret_station.transform.parent.GetComponent<SliderLoaderRecharge>();


        SliderControlActivation.OnEngagement += (b) => turret_active = b;

        loader_recharge.OnDepletion += (loader_recharge is SliderLoaderFullAutoRecharge) ? DestroyLaser : BurstPulsate;


        loader_recharge.OnFullRecharge += ActivateColor;
        loader_recharge.OnDepletion += DeactivateColor;




        transform.position = turret_station.transform.position;
        transform.Translate(Vector3.forward * -4);
        Vector3 rotationDirection = (turret_head_charge.transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;

        transform.Rotate(Vector3.up, 90);
        transform.Rotate(Vector3.right, -90);


        if (loader_recharge is SliderLoaderFullAutoRecharge)
        {

            PlayerInputCommunication.OnMouseDown += () => { if (turret_active) SetupLaser(); };
            PlayerInputCommunication.OnMouseUp += DestroyLaser;
        }


    }

    // Update is called once per frame






    GameObject laser;
    Vector3 origin, target;
    void Update()
    {

        if (laser != null && loader_recharge.IsActive)
        {
            Pulsate();


        }








    }



    void ActivateColor()
    {
        GetComponent<Renderer>().materials = new Material[] { GetComponent<Renderer>().materials[0], GetComponent<Renderer>().materials[1], white };

    }

    void DeactivateColor()
    {
        GetComponent<Renderer>().materials = new Material[] { GetComponent<Renderer>().materials[0], GetComponent<Renderer>().materials[1], GetComponent<Renderer>().materials[1] };

    }



    void DestroyLaser()
    {
        Destroy(laser);
    }
    public void SetupLaser()
    {
        if (loader_recharge.IsRecharging || !loader_recharge.IsActive || laser != null) { return; }

        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser.GetComponent<Collider>());
        laser.GetComponent<Renderer>().sharedMaterial = white;
        laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        origin = transform.position;
        target = turret_head_charge.transform.position;

        Aim();
    }



    Vector3 initScale;
    void Aim()
    {


        float distance = Vector3.Distance(origin, target);

        laser.transform.localScale = new Vector3(laser.transform.localScale.x, distance / 2f, laser.transform.localScale.z);
        Vector3 middleVector = (origin + target) / 2f;
        Vector3 rotationDirection = (target - origin);
        laser.transform.up = rotationDirection;

        laser.transform.position = middleVector;

        initScale = laser.transform.localScale;





    }
    void Pulsate()
    {

        float sizeIncrease = Mathf.Sin(Time.time * pulsate_speed) * 0.075f;
        laser.transform.localScale = new Vector3(initScale.x + sizeIncrease, laser.transform.localScale.y, initScale.z + sizeIncrease);


    }




    void BurstPulsate()
    {


        IEnumerator Burst()
        {

            SetupLaser();



            float step_amount = 5;

            for (float i = 0; i < step_amount; i++)
            {
                laser.transform.localScale = new Vector3(laser.transform.localScale.x + burst_speed, laser.transform.localScale.y, laser.transform.localScale.z + burst_speed);
                yield return null;
            }


            for (float i = 0; i < step_amount; i++)
            {
                laser.transform.localScale = new Vector3(laser.transform.localScale.x - burst_speed, laser.transform.localScale.y, laser.transform.localScale.z - burst_speed);
                yield return null;
            }



            DestroyLaser();
        }
        StartCoroutine(Burst());


    }



}
