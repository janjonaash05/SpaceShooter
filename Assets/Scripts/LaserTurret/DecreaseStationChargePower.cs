using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseStationChargePower : MonoBehaviour
{
    // Start is called before the first frame update
    public float max_power;
    float power;
    public float power_levels;
    public float recharge_delay;
    //public GameObject turret_control_head;
    //public GameObject AutoCollider;

    [SerializeField] float delta_size_unit;

    [SerializeField]
    int ID;



    public event Action OnRechargeStart, OnRechargeEnd;


    float max_size_y;
    float size_y;
    void Start()
    {
        max_size_y = transform.localScale.y;
        size_y = max_size_y;

        power = max_power;


        //  target_bomb.GetComponent<TargetBomb>().OnBarrageStart += Decrease;




        switch (ID) 
        {
            case 1:
                LaserTurretCommunication1.OnAutoTargetingSuccess += Decrease;



                OnRechargeStart += LaserTurretCommunication1.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunication1.Raise_EnableAutoTargeting;
                break;

            case 2:
                LaserTurretCommunication2.OnAutoTargetingSuccess += Decrease;



                OnRechargeStart += LaserTurretCommunication2.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunication2.Raise_EnableAutoTargeting;

                break;


        }


       









    }

    // Update is called once per frame
    void Update()
    {

    }






    public void Decrease()
    {

        power -= max_power / power_levels;
        size_y -= max_size_y / power_levels;
        transform.localScale = new Vector3(transform.localScale.x, size_y, transform.localScale.z);


        if (power == 0)
        {

            StartCoroutine(StartRecharge());

        }


    }


    IEnumerator StartRecharge()
    {



        yield return new WaitForSeconds(ControlColorChange.DARKENING_WAIT_TIME);
        OnRechargeStart?.Invoke();

        yield return StartCoroutine(Recharge());


        OnRechargeEnd?.Invoke();




    }



    IEnumerator Recharge()
    {


        while (size_y < max_size_y)
        {

            size_y += delta_size_unit;
            transform.localScale = new Vector3(transform.localScale.x, size_y, transform.localScale.z);
            yield return new WaitForSeconds(recharge_delay);

        }

        transform.localScale = new Vector3(transform.localScale.x, max_size_y, transform.localScale.z);
        power = max_power;









    }
}
