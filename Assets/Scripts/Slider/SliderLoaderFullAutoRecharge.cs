using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SliderLoaderFullAutoRecharge : SliderLoaderRecharge
{

    [SerializeField] GameObject charge_prefab;





    GameObject[,] charge_grid;
    //  [SerializeField] int recharge_delay;






    //float margin = 0.25f; // for size 10x10x10





    float[] z_positions = { 0.375f, 0.125f, -0.125f, -0.375f }; // child
    float[] y_positions = { -1.5f, -1.25f, -1, -0.75f, -0.5f, -0.25f, 0, 0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f }; // parent

    IndexHolder holder;







    void Start()
    {
        PlayerInputCommunication.OnSliderFullAutoClick += (_) => OnActivationInvoke();
        PlayerInputCommunication.OnSliderBoltClick += (_) => OnDeactivationInvoke();


        charge_grid = new GameObject[y_positions.Length, z_positions.Length];


        holder = new IndexHolder(0, y_positions.Length - 1, 0, z_positions.Length - 1);

        holder.HardSetValues(-1, holder.maxChild);

        InitialChargeUp();



        OnDepletion += Recharge;





    }





    /// <summary>
    /// Fills the grid.
    /// </summary>
    void InitialChargeUp()
    {

        while (ChangeIndex(1) != 1) ;




    }



    /// <summary>
    /// <para>Waits a set amount of time.</para>
    /// <para>While calling ChangeIndex() with one doesn't result in -1, plays the SLIDER_FULL_AUTO_CHARGE_SPAWN sound and waits again. </para>
    /// </summary>
    async void Recharge()
    {

        await Task.Delay(UpgradesManager.GetCurrentSliderRechargeValue().full_auto);
        while (ChangeIndex(1) != 1)
        {
            AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_FULL_AUTO_CHARGE_SPAWN);

            await Task.Delay(UpgradesManager.GetCurrentSliderRechargeValue().full_auto);
        }






    }
    /// <summary>
    /// Calls either CreateChargeAtHolder() or DestroyChargeAtHolder(), based on if the childDelta is positive.
    /// <para>Gets the result of calling ChangeIndex on the holder with the childDelta.</para>
    /// <para>If the result is -1, calls OnDepletionInvoke(), and if it 1, calls OnFullRechargeInvoke().</para>
    /// <para></para>
    /// </summary>
    /// <param name="childDelta"></param>
    /// <returns></returns>
    public int ChangeIndex(int childDelta)
    {



        Action exec = (childDelta > 0) ? CreateChargeAtHolder : DestroyChargeAtHolder;
        exec();

        int result = holder.ChangeIndex(0, childDelta);

        
        if (result == -1)
        {
            OnDepletionInvoke();

        }
        else if (result == 1)
        {
            OnFullRechargeInvoke();
        }

        return result;

    }


    /// <summary>
    /// Creates the charge gameObject, sets its material to On/Off based on IsActive.
    /// <para>Assigns it as a grid value at holder Parent and Child indexes.</para>
    /// <para>Assigns localPosition to the values at y_positons with Parent and z_positions with Child as indexes.</para>
    /// </summary>
    void CreateChargeAtHolder()
    {
        GameObject charge = null;
        try
        {
            charge = Instantiate(charge_prefab, transform, false);


            var color_change = charge.GetComponent<SliderLoaderChargeColorChange>();
            Material mat = (IsActive) ? color_change.On : color_change .Off;


            charge.GetComponent<Renderer>().material = mat;
            charge_grid[holder.Parent, holder.Child] = charge;

            charge.transform.parent = transform;

            charge.transform.localPosition = new Vector3(0, y_positions[holder.Parent], z_positions[holder.Child]);


        }
        catch
        {
            Destroy(charge);
        }


    }

    /// <summary>
    /// If exists, destroys the charge gameObject at the current holder indexes in the grid.
    /// </summary>
    void DestroyChargeAtHolder()
    {
        Destroy(charge_grid?[holder.Parent, holder.Child]);
    }




}

