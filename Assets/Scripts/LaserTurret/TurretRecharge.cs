using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretRecharge : MonoBehaviour
{
    
    int max_capacity;


    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;

    [SerializeField]
    int ID;

    int ARBITRARY_TURRET_RECHARGED_CAPACITY;

    Coroutine current_recharge_coroutine;

    const float POSITION_FOR_SIZE_DIVIDER = 75;

    bool recharging = false;

    public event Action OnRechargeStart, OnRechargeEnd;

    ParticleSystem ps;


    LaserTurretChannel channel;

    void Start()
    {


        max_capacity =
             LaserTurretChannel.MAX_TURRET_CAPACITY;
        charges = new();
        StartCoroutine(GenerateCharges());

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();

        channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.OnTurretCapacityChanged += Turret1CapacityValueChanged;
        channel.OnTurretCapacityDepleted += TurretCapacityDepleted;
        UpgradesManager.OnTurretCapacityValueChange += TurretCapacityUpgradeValueChange;

    }


    void Turret1CapacityValueChanged()
    {
        if (recharging) return;
        charges[channel.TURRET_CAPACITY].GetComponent<Renderer>().enabled = false;
    }

    /*
    void Turret2CapacityValueChanged()
    {
        if (recharging) return;
        charges[LaserTurretCommunicationChannels.Channel2.TURRET_CAPACITY].GetComponent<Renderer>().enabled = false;
    }
    */

    void TurretCapacityDepleted() => RechargeOnDepletion();


    void TurretCapacityUpgradeValueChange() 
    {
        if (recharging)
        {
            StartCoroutine(UpgradeRecharging());
            return;
        }


        StartCoroutine(Upgrade());

    }


    private void OnDestroy()
    {
        
        channel.OnTurretCapacityChanged -= Turret1CapacityValueChanged;
        channel.OnTurretCapacityDepleted -= TurretCapacityDepleted;

        UpgradesManager.OnTurretCapacityValueChange -= TurretCapacityUpgradeValueChange;
    }





    void RechargeOnDepletion()
    {
        if (!recharging) current_recharge_coroutine = StartCoroutine(Recharge(0));
    }



    IEnumerator UpgradeRecharging()
    {


        StopCoroutine(current_recharge_coroutine);

        yield return StartCoroutine(GenerateCharges());

        yield return current_recharge_coroutine = StartCoroutine(Recharge(ARBITRARY_TURRET_RECHARGED_CAPACITY));
    }


    IEnumerator Upgrade()
    {
        yield return StartCoroutine(GenerateCharges());
        int capacity = ID == 1 ? LaserTurretCommunicationChannels.Channel1.TURRET_CAPACITY : LaserTurretCommunicationChannels.Channel2.TURRET_CAPACITY;


        for (int i = UpgradesManager.GetCurrentTurretCapacityValue() - 1; i >= capacity; i--)
        {
            charges[i].GetComponent<Renderer>().enabled = false;

        }

    }



    IEnumerator GenerateCharges()
    {
        foreach (GameObject charge in charges)
        {
            try
            {
                Destroy(charge);
            }
            catch { }
            
            yield return null;

        }


        max_capacity = LaserTurretChannel.MAX_TURRET_CAPACITY;
 
        float start_size = charge_prefab.transform.localScale.z;
        float size = start_size / max_capacity;
        float positionUnit = size / (POSITION_FOR_SIZE_DIVIDER);

        float scaled_size = start_size / (max_capacity + (positionUnit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

        if (max_capacity % 2 == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 0; j < max_capacity / 2; j++)
                {
                    GameObject charge = Instantiate(charge_prefab, transform, false);
                    float pos = positionUnit + positionUnit * 2 * j;


                    charge.transform.localPosition = new(0, 0, pos * mult);
                    charge.transform.localScale = new(start_size, start_size, scaled_size);
                }

            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 1; j <= (max_capacity - 1) / 2; j++)
                {
                    GameObject charge = Instantiate(charge_prefab, transform, false);
                    float pos = positionUnit * 2 * j;

                    charge.transform.localPosition = new(0, 0, pos * mult);
                    charge.transform.localScale = new(start_size, start_size, scaled_size);

                }
            }

            GameObject charge_center = Instantiate(charge_prefab, transform, false);


            charge_center.transform.localPosition = Vector3.zero;
            charge_center.transform.localScale = new(start_size, start_size, scaled_size);
        }


        charges = new();


        string charge_tag = (ID == 1) ? Tags.TURRET_CHARGE_1 : Tags.TURRET_CHARGE_2;

        foreach (Transform child in transform)
        {

            if (child.CompareTag(charge_tag))
            {
                charges.Add(child.gameObject);
                yield return null;

            }

        }
        charges = charges.OrderBy(x => x.transform.localPosition.z).ToList();
    }

    IEnumerator Recharge(int skips_amount)
    {
        ARBITRARY_TURRET_RECHARGED_CAPACITY = 0;
        recharging = true;

        foreach (GameObject charge in charges)
        {
            try
            {
                charge.GetComponent<Renderer>().enabled = false;


            }
            catch (Exception)
            {
            }
        }



        ps.enableEmission = true;

        int i = 0;
        foreach (GameObject charge in charges)
        {



            ARBITRARY_TURRET_RECHARGED_CAPACITY++;

            i++;
            float recharge_delay = (i <= skips_amount) ? 0 : UpgradesManager.GetCurrentTurretRechargeValue();


            yield return new WaitForSeconds(recharge_delay);

            try
            {
                charge.GetComponent<Renderer>().enabled = true;
                AudioManager.PlayActivitySound(ID == 1 ? AudioManager.ActivityType.TURRET_CHARGE_SPAWN_1 : AudioManager.ActivityType.TURRET_CHARGE_SPAWN_2);

            }
            catch (Exception)
            {
            }
        }

        recharging = false;

        Action toExecute = ((ID == 1) ? LaserTurretCommunicationChannels.Channel1.Raise_TurretCapacityRecharged : LaserTurretCommunicationChannels.Channel2.Raise_TurretCapacityRecharged);
        toExecute();


        ARBITRARY_TURRET_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;
        ps.enableEmission = false;
    }

}
