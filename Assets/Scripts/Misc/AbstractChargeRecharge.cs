using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class AbstractChargeRecharge : MonoBehaviour
{
    protected List<GameObject> charges;

    protected int max_capacity;
    protected bool recharging = false;
    protected Coroutine current_recharge_coroutine;
    [SerializeField] protected GameObject charge_prefab;

    protected int ARBITRARY_CHARGES_RECHARGED_CAPACITY;
    protected abstract float POSITION_FOR_SIZE_DIVIDER { get; }

    /// <summary>
    /// If not recharging, sets the current_recharge_coroutine to the start of the Recharge coroutine with 0 skips.
    /// </summary>
    protected void RechargeOnDepletion()
    {
        if (!recharging) current_recharge_coroutine = StartCoroutine(Recharge(0));
    }


    /// <summary>
    /// <para>Stops the current_recharge_coroutine.</para>
    /// <para>Yields for GenerateCharges coroutine.</para>
    /// <para>Yields and assigns the current_recharge_coroutine to the start Recharge coroutine with ARBITRARY_SHIELD_RECHARGED_CAPACITY as skips amount. </para>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator UpgradeRecharging()
    {


        StopCoroutine(current_recharge_coroutine);

        yield return StartCoroutine(GenerateCharges());

        yield return current_recharge_coroutine = StartCoroutine(Recharge(ARBITRARY_CHARGES_RECHARGED_CAPACITY));
    }

    //protected abstract IEnumerator Recharge(int skips_amount);



    /// <summary>
    /// <para>Sets the ARBITRARY_CHARGES_RECHARGED_CAPACITY to 0, calls ExecuteBeforeRecharge() and sets recharging to true.  </para>
    /// <para>Attempts to disable all charge renderers.</para>
    /// <para>Enables particle system emission, yields RegenerateCharges coroutine with skips_amount, disables emission. </para>
    /// <para>Sets recharging to false, calls ExecuteAfterRecharge() and sets the ARBITRARY_CHARGES_RECHARGED_CAPACITY to GetMaxCapacityToSetOnGenerate() call.</para>
    /// </summary>
    /// <param name="skips_amount"></param>
    /// <returns></returns>
    protected IEnumerator Recharge(int skips_amount)
    {
        ARBITRARY_CHARGES_RECHARGED_CAPACITY = 0;


        ExecuteBeforeRecharge();
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



        GetParticleSystem.enableEmission = true;

        yield return StartCoroutine(RegenerateCharges(skips_amount));

        GetParticleSystem.enableEmission = false;


        recharging = false;

        ExecuteAfterRecharge();

        ARBITRARY_CHARGES_RECHARGED_CAPACITY = GetMaxCapacityToSetOnGenerate();

    }

    protected abstract ParticleSystem GetParticleSystem { get; }


    protected abstract void ExecuteBeforeRecharge();
    protected abstract void ExecuteAfterRecharge();













    /// <summary>
    /// <para>Yields for GenerateCharges coroutine.</para>
    /// <para>Goes from the MaxCapacity -1 downwards to CurrentCapacity, and disables renderers for charges at those indexes.</para>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Upgrade()
    {
        yield return StartCoroutine(GenerateCharges());


        (int MaxCapacity, int CurrentCapacity) = GetValuesForUpgrade();

        //int capacity = CoreCommunication.SHIELD_CAPACITY;


        for (int i = MaxCapacity - 1; i >= CurrentCapacity; i--)
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


    protected abstract (int MaxCapacity, int CurrentCapacity) GetValuesForUpgrade();



    /// <summary>
    /// <para>Yields DestroyCharges coroutine.</para>
    /// <para>Assigns max capacity, position unit and scale values.</para>
    /// <para>If max capacity is even, creates charges at locations calculated from both directions from 0, with no center charge.</para>
    /// <para>If max capacity is odd, creates charges at locations calculated from both directions from 0, with a center charge.</para>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator GenerateCharges()
    {
        yield return StartCoroutine(DestroyCharges());
        max_capacity = GetMaxCapacityToSetOnGenerate();

        (float position_unit, Vector3 scale) = GetPositionUnitAndScale();





        if (max_capacity % 2 == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 0; j < max_capacity / 2; j++)
                {

                    float pos = position_unit + position_unit * 2 * j;



                    //CreateCharge(new(0, pos * mult, 0), scale);

                    CreateCharge(FormPlacementVector(pos * mult), scale);
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
                    float pos = position_unit * 2 * j;
                    CreateCharge(FormPlacementVector(pos * mult), scale);


                }
            }



            CreateCharge(Vector3.zero, scale);

        }


        CreateChargeList();


    }

    protected abstract (float position_unit, Vector3 scale) GetPositionUnitAndScale();
    protected abstract int GetMaxCapacityToSetOnGenerate();

    /// <summary>
    /// Creates a charges list, adds all children with the acquiered tag, then assigns it as a GetChargesAsSortedList() call. 
    /// </summary>
    protected void CreateChargeList()
    {
        charges = new();



        string tag = GetTagForList();
        foreach (Transform child in transform)
        {
            if (child.CompareTag(tag))
            {
                charges.Add(child.gameObject);
            }
        }

        charges = GetChargesAsSortedList();







    }




    protected abstract List<GameObject> GetChargesAsSortedList();
    protected abstract string GetTagForList();

    /// <summary>
    /// Attempts to destroy all charge gameObjects in charges list.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DestroyCharges()
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

    }


    /// <summary>
    /// Creates a charge gameObject and sets its localPosition to arg pos and localScale to arg size.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    protected void CreateCharge(Vector3 pos, Vector3 size)
    {
        GameObject charge = Instantiate(charge_prefab, transform, false);

        charge.transform.localPosition = pos;
        charge.transform.localScale = size;
    }






   protected abstract Func<float, Vector3> FormPlacementVector { get; }



    protected abstract AudioManager.ActivityType CHARGE_SPAWN_SOUND_ACTIVITY_TYPE { get; }


    /// <summary>
    /// Goes through all charges, increases i and ARBITRARY_CHARGES_RECHARGED_CAPACITY.
    /// <para>For each charge, either skips delay, or calls CalculateRechargeDelay() (Skips if the current iteration is less or equal to the skips amount.) </para>
    /// <para>Skips iteration if the delay is infinite.</para>
    /// <para>Waits the delay,</para>
    /// <para>Plays the CHARGE_SPAWN_SOUND_ACTIVITY_TYPE sound and enables current charge's renderer.</para>
    /// </summary>
    /// <param name="skips_amount"></param>
    /// <returns></returns>
    protected IEnumerator RegenerateCharges(int skips_amount)
    {
        int i = 0;
        foreach (GameObject charge in charges)
        {
            ARBITRARY_CHARGES_RECHARGED_CAPACITY++;

            i++;
            float recharge_delay = (i <= skips_amount) ? 0 : CalculateRechargeDelay();


            if (float.IsPositiveInfinity(recharge_delay)) { continue; }

            yield return new WaitForSeconds(recharge_delay);

            try
            {
                AudioManager.PlayActivitySound(CHARGE_SPAWN_SOUND_ACTIVITY_TYPE);
                charge.GetComponent<Renderer>().enabled = true;

            }
            catch
            {
            }
        }

    }



    protected abstract float CalculateRechargeDelay();

}
