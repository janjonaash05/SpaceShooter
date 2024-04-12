using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldRecharge : MonoBehaviour
{

    int max_capacity;


    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;


    const float POSITION_FOR_SIZE_DIVIDER = 100;


    bool recharging;





    Coroutine current_recharge_coroutine;







    ParticleSystem ps;

    [SerializeField] GameObject shield_emitter, shield_emitter_antenna;
    Renderer emitter_rend, antenna_rend;

    Material on_mat, off_mat;





    private void OnDestroy()
    {

        CoreCommunication.OnBombFallen -= BombFallen;
        CoreCommunication.OnShieldDepleted -= RechargeOnDepletion;
        UpgradesManager.OnShieldMaxCapacityValueChange -= ShieldMaxCapacityValueChange;
    }





    void Start()
    {


        max_capacity = UpgradesManager.SHIELD_MAX_CAPACITY;
        Debug.LogError(max_capacity +" shield max capacity");

        charges = new();
        StartCoroutine(GenerateCharges());
        emitter_rend = shield_emitter.GetComponent<Renderer>();

        on_mat = emitter_rend.materials[2];
        off_mat = emitter_rend.materials[1];


        antenna_rend = shield_emitter_antenna.GetComponent<Renderer>();


        ps = transform.GetComponentInChildren<ParticleSystem>();





        CoreCommunication.OnBombFallen += BombFallen;
        CoreCommunication.OnShieldDepleted += RechargeOnDepletion;
        UpgradesManager.OnShieldMaxCapacityValueChange += ShieldMaxCapacityValueChange;











    }





    void BombFallen(Material m)
    {
        if (recharging) return;
        charges[CoreCommunication.SHIELD_CAPACITY].GetComponent<Renderer>().enabled = false;

    }

    void ShieldMaxCapacityValueChange()
    {
        if (recharging)
        {
            StartCoroutine(UpgradeRecharging());
            return;
        }


        StartCoroutine(Upgrade());
    }







    void RechargeOnDepletion()
    {
        if (!recharging) current_recharge_coroutine = StartCoroutine(Recharge(0));
    }

    IEnumerator UpgradeRecharging()
    {


        StopCoroutine(current_recharge_coroutine);

        yield return StartCoroutine(GenerateCharges());

        yield return current_recharge_coroutine = StartCoroutine(Recharge(ARBITRARY_SHIELD_RECHARGED_CAPACITY));
    }


    IEnumerator Upgrade()
    {
        yield return StartCoroutine(GenerateCharges());
        int capacity = CoreCommunication.SHIELD_CAPACITY;


        for (int i = UpgradesManager.GetCurrentTurretCapacityValue() - 1; i >= capacity; i--)
        {
            try
            {
                charges[i].GetComponent<Renderer>().enabled = false;
            }
            catch
            {

            }

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
        max_capacity = UpgradesManager.SHIELD_MAX_CAPACITY;


        float startSize = charge_prefab.transform.localScale.x;
        float size = startSize / max_capacity;
        float positionUnit = size / (POSITION_FOR_SIZE_DIVIDER);

        float scaled_size = startSize / (max_capacity + (positionUnit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

        if (max_capacity % 2 == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 0; j < max_capacity / 2; j++)
                {
                    GameObject charge = Instantiate(charge_prefab, transform, false);
                    float pos = positionUnit + positionUnit * 2 * j;


                    charge.transform.localPosition = new(0, pos * mult, 0);
                    charge.transform.localScale = new(startSize, scaled_size, scaled_size);
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
                    charge.transform.localScale = new(startSize, scaled_size, scaled_size);

                }
            }

            GameObject charge_center = Instantiate(charge_prefab, transform, false);


            charge_center.transform.localPosition = Vector3.zero;
            charge_center.transform.localScale = new(startSize, scaled_size, scaled_size);
        }


        charges = new();

        foreach (Transform child in transform)
        {
            if (child.CompareTag(Tags.SHIELD_CHARGE))
            {
                charges.Add(child.gameObject);

            }

        }

        charges = charges.OrderByDescending(x => x.transform.localPosition.y).ToList();


    }


    /*
    void Recharge()
    {
        recharging = true;


        GenerateCharges();

        foreach (GameObject charge in charges)
        {
            charge.SetActive(false);
        }


        



        ps.enableEmission = true;
        ChangeEmitterAndAntennaColor(off_mat);
        

        StartCoroutine(recharge());


        IEnumerator recharge()
        {
            
            foreach (GameObject charge in charges)
            {
                float recharge_delay = CoreCommunication.CORE_INDEX_HOLDER.Parent switch
                {
                    5 => 0.1f,
                    4 => 0.5f,
                    3 => 1f,
                    2 => 1.5f,
                    1 => 2f,
                    0 => float.NaN
                }; ;
                

                if (recharge_delay == float.NaN) { continue; }
                yield return new WaitForSeconds(recharge_delay);
                charge.SetActive(true); 



               
            }

            recharging = false;
            CoreCommunication.Raise_ShieldRecharged();


            ps.enableEmission = false;
            ChangeEmitterAndAntennaColor(on_mat);
        }

    }

    */



    int ARBITRARY_SHIELD_RECHARGED_CAPACITY;

    IEnumerator Recharge(int skips_amount)
    {
        ARBITRARY_SHIELD_RECHARGED_CAPACITY = 0;
        recharging = true;
        ChangeEmitterAndAntennaColor(off_mat);

        foreach (GameObject charge in charges)
        {
            try
            {
                charge.GetComponent<Renderer>().enabled = false;


            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }



        ps.enableEmission = true;

        int i = 0;
        foreach (GameObject charge in charges)
        {



            ARBITRARY_SHIELD_RECHARGED_CAPACITY++;

            i++;
            float recharge_delay = (i <= skips_amount) ? 0 : CoreCommunication.CORE_INDEX_HOLDER.Parent switch
            {
                5 => 0.75f,
                4 => 1f,
                3 => 1.25f,
                2 => 1.5f,
                1 => 1.75f,
                0 => float.PositiveInfinity

            };


            if (float.IsPositiveInfinity(recharge_delay)) { continue; }

            yield return new WaitForSeconds(recharge_delay);

            try
            {
                charge.GetComponent<Renderer>().enabled = true;

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        recharging = false;

        CoreCommunication.Raise_ShieldRecharged();




        ChangeEmitterAndAntennaColor(on_mat);

        ARBITRARY_SHIELD_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;
        ps.enableEmission = false;
    }




    void ChangeEmitterAndAntennaColor(Material m)
    {
        var e_mats = emitter_rend.materials;
        e_mats[^1] = m;

        var a_mats = antenna_rend.materials;
        a_mats[^1] = m;

        emitter_rend.materials = e_mats;
        antenna_rend.materials = a_mats;

    }
















    /*
    int max_capacity;


    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;

    [SerializeField]
    int ID;

    int ARBITRARY_SHIELD_RECHARGED_CAPACITY;

    Coroutine current_recharge_coroutine;

    const float POSITION_FOR_SIZE_DIVIDER = 75;

    bool recharging = false;

    public event Action OnRechargeStart, OnRechargeEnd;

    ParticleSystem ps;

    void Start()
    {


        max_capacity =
             LaserTurretChannel.MAX_TURRET_CAPACITY;
        charges = new();
        StartCoroutine(GenerateCharges());

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();


        switch (ID)
        {
            case 1:

                // OnRechargeStart += LaserTurretCommunicationChannels.Channel1.Raise_DisableAutoTargeting;
                // OnRechargeEnd += LaserTurretCommunicationChannels.Channel1.Raise_EnableAutoTargeting;





                LaserTurretCommunicationChannels.Channel1.OnTurretCapacityChanged += () =>
                {
                    if (recharging) return;
                    charges[LaserTurretCommunicationChannels.Channel1.TURRET_CAPACITY].GetComponent<Renderer>().enabled = false;


                };


                LaserTurretCommunicationChannels.Channel1.OnTurretCapacityDepleted += () => RechargeOnDepletion();

                break;

            case 2:


                LaserTurretCommunicationChannels.Channel2.OnTurretCapacityChanged += () =>
                {
                    if (recharging) return;
                    charges[LaserTurretCommunicationChannels.Channel2.TURRET_CAPACITY].GetComponent<Renderer>().enabled = false;

                };


                LaserTurretCommunicationChannels.Channel2.OnTurretCapacityDepleted += () => RechargeOnDepletion();
                break;
        }



        UpgradesManager.OnTurretCapacityValueChange += () =>
        {


            if (recharging)
            {
                StartCoroutine(UpgradeRecharging());
                return;
            }


            StartCoroutine(Upgrade());



        };

    }


    void RechargeOnDepletion()
    {
        if (!recharging) current_recharge_coroutine = StartCoroutine(Recharge(0));
    }



    IEnumerator UpgradeRecharging()
    {


        StopCoroutine(current_recharge_coroutine);

        yield return StartCoroutine(GenerateCharges());

        yield return current_recharge_coroutine = StartCoroutine(Recharge(ARBITRARY_SHIELD_RECHARGED_CAPACITY));
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
        ARBITRARY_SHIELD_RECHARGED_CAPACITY = 0;
        recharging = true;

        foreach (GameObject charge in charges)
        {
            try
            {
                charge.GetComponent<Renderer>().enabled = false;


            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }



        ps.enableEmission = true;

        int i = 0;
        foreach (GameObject charge in charges)
        {



            ARBITRARY_SHIELD_RECHARGED_CAPACITY++;

            i++;
            float recharge_delay = (i <= skips_amount) ? 0 : UpgradesManager.GetCurrentTurretRechargeValue();


            yield return new WaitForSeconds(recharge_delay);

            try
            {
                charge.GetComponent<Renderer>().enabled = true;

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        recharging = false;

        Action toExecute = ((ID == 1) ? LaserTurretCommunicationChannels.Channel1.Raise_TurretCapacityRecharged : LaserTurretCommunicationChannels.Channel2.Raise_TurretCapacityRecharged);
        toExecute();


        ARBITRARY_SHIELD_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;
        ps.enableEmission = false;
        OnRechargeEnd?.Invoke();
    }

    */

}
