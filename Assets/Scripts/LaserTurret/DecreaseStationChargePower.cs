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




    bool paused = false, recharging = false;





    public event Action OnRechargeStart, OnRechargeEnd;



    Material before_turnoff;


    float max_size_y;
    float size_y;
    void Start()
    {
        max_size_y = transform.localScale.y;
        size_y = max_size_y;

        power = max_power;


        //  target_bomb.GetComponent<TargetBomb>().OnBarrageStart += Decrease;


        OnRechargeStart += () => recharging = true;
        OnRechargeEnd += () => recharging = false;


        switch (ID) 
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingSuccess += Decrease;



                OnRechargeStart += LaserTurretCommunicationChannels.Channel1.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunicationChannels.Channel1.Raise_EnableAutoTargeting;

                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += () => paused = true;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += () => paused = false;


                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += EndEmission;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += () => { if (recharging) StartEmission(); };



                break;

            case 2:
                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingSuccess += Decrease;



                OnRechargeStart += LaserTurretCommunicationChannels.Channel2.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunicationChannels.Channel2.Raise_EnableAutoTargeting;

                LaserTurretCommunicationChannels.Channel2.OnControlDisabled += () => paused = true;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += () => paused = false;



                LaserTurretCommunicationChannels.Channel2.OnControlDisabled +=  EndEmission;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += () => { if (recharging) StartEmission(); };





                break;


        }



         ps = transform.parent.GetChild(1).GetComponent<ParticleSystem>();
         ps_rend = ps.GetComponent<ParticleSystemRenderer>();








        OnRechargeStart += StartEmission;
        OnRechargeEnd += EndEmission;


    }

    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;

    void StartEmission() 
    {
        var em = ps.emission;
        em.enabled = true;
        ps_rend.material = GetComponent<Renderer>().material;
        ps_rend.trailMaterial = GetComponent<Renderer>().material;
    }


    void EndEmission() 
    {
        var em = ps.emission;
        em.enabled = false;
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

            if(paused) { yield return null; continue; }

            size_y += delta_size_unit;
            transform.localScale = new Vector3(transform.localScale.x, size_y, transform.localScale.z);
            yield return new WaitForSeconds(recharge_delay);

        }

        transform.localScale = new Vector3(transform.localScale.x, max_size_y, transform.localScale.z);
        power = max_power;









    }
}
