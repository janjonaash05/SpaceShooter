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

   protected abstract IEnumerator Recharge(int skips_amount);

    /// <summary>
    /// <para>Yields for GenerateCharges coroutine.</para>
    /// <para>Goes from the current capacity value minus 1 to the current SHIELD_CAPACITY, and disables renderers for charges at those indexes.</para>
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Upgrade()
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
        max_capacity = GetMaxCapacityToSet();

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
    protected abstract int GetMaxCapacityToSet();

    /// <summary>
    /// Creates a charges list, adds all children with the SHIELD_CHARGE tag, then orders it descending based on the localPosition y value.
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
    /// Creates a charge gameObject and sets its localPosition to arg pos and localScale to arg scale.
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
    /// Goes through all charges, increases i and ARBITRARY_SHIELD_RECHARGED_CAPACITY.
    /// <para>For each charge, calculates the recharge delay based on ORE_INDEX_HOLDER Parent value. </para>
    /// <para>Waits the calculated time.</para>
    /// <para>Plays the SHIELD_CHARGE_SPAWN and enables current charge's renderer.</para>
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
                AudioManager.PlayActivitySound(AudioManager.ActivityType.SHIELD_CHARGE_SPAWN);
                charge.GetComponent<Renderer>().enabled = true;

            }
            catch
            {
            }
        }

    }



    protected abstract float CalculateRechargeDelay();

}
