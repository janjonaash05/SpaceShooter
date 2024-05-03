using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TurretRecharge : AbstractChargeRecharge
{

    

    [SerializeField]
    int ID;



    //const float POSITION_FOR_SIZE_DIVIDER = 75;

    protected override Func<float, Vector3> FormPlacementVector => (x) => new(0, 0, x);

    protected override ParticleSystem GetParticleSystem => ps;
    protected override (int MaxCapacity, int CurrentCapacity) GetValuesForUpgrade()
    {
        return (LaserTurretChannel.MAX_TURRET_CAPACITY, (ID ==1)? LaserTurretCommunicationChannels.Channel1.TURRET_CAPACITY : LaserTurretCommunicationChannels.Channel2.TURRET_CAPACITY) ;
    }

    protected override List<GameObject> GetChargesAsSortedList() => charges.OrderBy(x => x.transform.localPosition.z).ToList();



    public event Action OnRechargeStart, OnRechargeEnd;

    ParticleSystem ps;


    LaserTurretChannel channel;

    protected override float POSITION_FOR_SIZE_DIVIDER => 75;

    void Start()
    {


        max_capacity =
             LaserTurretChannel.MAX_TURRET_CAPACITY;
        charges = new();
        StartCoroutine(GenerateCharges());

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();

        channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.OnTurretCapacityChanged += TurretCapacityValueChanged;
        channel.OnTurretCapacityDepleted += TurretCapacityDepleted;
        UpgradesManager.OnTurretCapacityValueChange += TurretCapacityUpgradeValueChange;

    }


    void TurretCapacityValueChanged()
    {
        if (recharging) return;
        charges[channel.TURRET_CAPACITY].GetComponent<Renderer>().enabled = false;
    }


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

        channel.OnTurretCapacityChanged -= TurretCapacityValueChanged;
        channel.OnTurretCapacityDepleted -= TurretCapacityDepleted;

        UpgradesManager.OnTurretCapacityValueChange -= TurretCapacityUpgradeValueChange;
    }






    protected override AudioManager.ActivityType CHARGE_SPAWN_SOUND_ACTIVITY_TYPE => ID ==1 ? AudioManager.ActivityType.TURRET_CHARGE_SPAWN_1 : AudioManager.ActivityType.TURRET_CHARGE_SPAWN_2;

    
    /// <summary>
    /// Empty
    /// </summary>
    protected override void ExecuteBeforeRecharge()
    {
    }

    protected override void ExecuteAfterRecharge()
    {
        Action toExecute = ((ID == 1) ? LaserTurretCommunicationChannels.Channel1.Raise_TurretCapacityRecharged : LaserTurretCommunicationChannels.Channel2.Raise_TurretCapacityRecharged);
        toExecute();

    }

   
    protected override (float position_unit, Vector3 scale) GetPositionUnitAndScale()
    {
        float start_size = charge_prefab.transform.localScale.z;
        float size = start_size / max_capacity;
        float position_unit = size / (POSITION_FOR_SIZE_DIVIDER);

        float scaled_size = start_size / (max_capacity + (position_unit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

        Vector3 scale = new(start_size, start_size, scaled_size);


        return (position_unit, scale);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>LaserTurretChannel.MAX_TURRET_CAPACITY</returns>
    protected override int GetMaxCapacityToSetOnGenerate()
    {
        return LaserTurretChannel.MAX_TURRET_CAPACITY;
    }

    protected override string GetTagForList()
    {
        return (ID == 1) ? Tags.TURRET_CHARGE_1 : Tags.TURRET_CHARGE_2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>UpgradesManager.GetCurrentTurretRechargeValue()</returns>
    protected override float CalculateRechargeDelay()
    {
        return UpgradesManager.GetCurrentTurretRechargeValue();
    }













    /*
    protected override IEnumerator Recharge(int skips_amount)
    {
        ARBITRARY_CHARGES_RECHARGED_CAPACITY = 0;
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

        yield return StartCoroutine(RegenerateCharges(skips_amount));

        ps.enableEmission = false;




        recharging = false;

        Action toExecute = ((ID == 1) ? LaserTurretCommunicationChannels.Channel1.Raise_TurretCapacityRecharged : LaserTurretCommunicationChannels.Channel2.Raise_TurretCapacityRecharged);
        toExecute();


        ARBITRARY_CHARGES_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;
        
    }
    */

    /*
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

public (float position_unit, Vector3 scale) GetCalculationValues()
{
    float start_size = charge_prefab.transform.localScale.z;
    float size = start_size / max_capacity;
    float position_unit = size / (POSITION_FOR_SIZE_DIVIDER);

    float scaled_size = start_size / (max_capacity + (position_unit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

    Vector3 scale = new(start_size, start_size, scaled_size);


    return (position_unit, scale);
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


    (float position_unit, Vector3 scale) = GetCalculationValues();



    if (max_capacity % 2 == 0)
    {
        for (int i = 0; i < 2; i++)
        {
            float mult = i == 0 ? 1 : -1;
            for (int j = 0; j < max_capacity / 2; j++)
            {
                GameObject charge = Instantiate(charge_prefab, transform, false);
                float pos = position_unit + position_unit * 2 * j;


                charge.transform.localPosition = new(0, 0, pos * mult);
                charge.transform.localScale = scale;
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
                float pos = position_unit * 2 * j;

                charge.transform.localPosition = new(0, 0, pos * mult);
                charge.transform.localScale = scale;

            }
        }

        GameObject charge_center = Instantiate(charge_prefab, transform, false);


        charge_center.transform.localPosition = Vector3.zero;
        charge_center.transform.localScale = scale;
    }


    yield return StartCoroutine(CreateChargesList());
}




IEnumerator CreateChargesList()
{

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

 protected override AudioManager.ActivityType CHARGE_SPAWN_SOUND_ACTIVITY_TYPE => ID ==1 ? AudioManager.ActivityType.TURRET_CHARGE_SPAWN_1 : AudioManager.ActivityType.TURRET_CHARGE_SPAWN_2;

protected override IEnumerator Recharge(int skips_amount)
{
    ARBITRARY_CHARGES_RECHARGED_CAPACITY = 0;
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



        ARBITRARY_CHARGES_RECHARGED_CAPACITY++;

        i++;
        float recharge_delay = (i <= skips_amount) ? 0 : CalculateRechargeDelay();


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


    ARBITRARY_CHARGES_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;
    ps.enableEmission = false;
}


*/
}
