using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretRecharge : MonoBehaviour
{
    // Start is called before the first frame update
    int max_capacity;


    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;


    [SerializeField]
    int ID;








    const float POSITION_FOR_SIZE_DIVIDER = 75;







    bool paused = false, recharging = false;





    public event Action OnRechargeStart, OnRechargeEnd;







    ParticleSystem ps;

    void Start()
    {


        max_capacity = UpgradesManager.SHIELD_MAX_CAPACITY;


        charges = new();
        GenerateCharges();





        ps = transform.GetChild(0).GetComponent<ParticleSystem>();





        switch (ID)
        {
            case 1:

                OnRechargeStart += LaserTurretCommunicationChannels.Channel1.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunicationChannels.Channel1.Raise_EnableAutoTargeting;

                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += () => paused = true;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += () => paused = false;


                LaserTurretCommunicationChannels.Channel1.OnTurretCapacityDepleted += Recharge;


                LaserTurretCommunicationChannels.Channel1.OnTurretCapacityChanged += () =>
                {
                    if (recharging) return;
                    Destroy(charges[LaserTurretCommunicationChannels.Channel1.TURRET_CAPACITY]);

                };


                LaserTurretCommunicationChannels.Channel1.OnTurretCapacityDepleted += () => { if (!recharging) Recharge(); };




                break;

            case 2:

                OnRechargeStart += LaserTurretCommunicationChannels.Channel2.Raise_DisableAutoTargeting;
                OnRechargeEnd += LaserTurretCommunicationChannels.Channel2.Raise_EnableAutoTargeting;

                LaserTurretCommunicationChannels.Channel2.OnControlDisabled += () => paused = true;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += () => paused = false;

                LaserTurretCommunicationChannels.Channel2.OnTurretCapacityDepleted += Recharge;

                LaserTurretCommunicationChannels.Channel2.OnTurretCapacityChanged += () =>
                {
                    if (recharging) return;
                    Destroy(charges[LaserTurretCommunicationChannels.Channel2.TURRET_CAPACITY]);

                };


                LaserTurretCommunicationChannels.Channel2.OnTurretCapacityDepleted += () => { if (!recharging) Recharge(); };
                break;


        }

    }


    void GenerateCharges()
    {

        max_capacity = (ID == 1) ? LaserTurretCommunicationChannels.Channel1.MAX_TURRET_CAPACITY : LaserTurretCommunicationChannels.Channel2.MAX_TURRET_CAPACITY;


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


                    //   charge.transform.localPosition = new(0, pos * mult, 0);
                    charge.transform.localPosition = new(0, 0, pos * mult);
                    //   charge.transform.localScale = new(start_size, scaled_size, scaled_size);
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

                    charge.transform.localPosition = new(0, pos * mult, 0);
                    charge.transform.localScale = new(start_size, scaled_size, scaled_size);

                }
            }

            GameObject charge_center = Instantiate(charge_prefab, transform, false);


            charge_center.transform.localPosition = Vector3.zero;
            charge_center.transform.localScale = new(start_size, scaled_size, scaled_size);
        }


        charges = new();


        string charge_tag = (ID==1)? Tags.TURRET_CHARGE_1 : Tags.TURRET_CHARGE_2;

        foreach (Transform child in transform)
        {
            if (child.CompareTag(charge_tag))
            {
                charges.Add(child.gameObject);

            }

        }

        charges = charges.OrderBy(x => x.transform.localPosition.z).ToList();


    }



    void Recharge()
    {
        recharging = true;


        GenerateCharges();

        foreach (GameObject charge in charges)
        {
            charge.SetActive(false);
        }


        charges.RemoveAt(0); //removes a wrongly delayed deleted charge



        ps.enableEmission = true;


        StartCoroutine(recharge());


        IEnumerator recharge()
        {

            foreach (GameObject charge in charges)
            {
                float recharge_delay = UpgradesManager.TURRET_RECHARGE_DEGREE_VALUE_DICT[UpgradesManager.UPGRADE_VALUE_DICT[UpgradesManager.UpgradeType.TURRET_RECHARGE]];


                if (recharge_delay == float.NaN) { yield return null; }
                yield return new WaitForSeconds(recharge_delay);
                charge.SetActive(true);




            }

            recharging = false;

            Action toExecute = ((ID == 1) ? LaserTurretCommunicationChannels.Channel1.Raise_TurretCapacityRecharged : LaserTurretCommunicationChannels.Channel2.Raise_TurretCapacityRecharged);
            toExecute();


            ps.enableEmission = false;
        }

    }



    // Update is called once per frame
    void Update()
    {

    }



}
