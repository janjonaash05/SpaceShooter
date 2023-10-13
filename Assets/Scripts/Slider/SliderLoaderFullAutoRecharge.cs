using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SliderLoaderFullAutoRecharge : SliderLoaderRecharge
{

    [SerializeField] GameObject charge_prefab;





    GameObject[,] charge_grid;
    [SerializeField] int recharge_delay;






    //float margin = 0.25f; // for size 10x10x10





    float[] z_positions = { 0.375f, 0.125f, -0.125f, -0.375f }; // child
    float[] y_positions = { -1.5f, -1.25f, -1, -0.75f, -0.5f, -0.25f, 0, 0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f }; // parent

    IndexHolder holder;



   



    void Start()
    {
        PlayerInputSO.OnSliderFullAutoClick += (_) => OnActivationInvoke();
        PlayerInputSO.OnSliderBoltClick += (_) => OnDeactivationInvoke();


        charge_grid = new GameObject[y_positions.Length, z_positions.Length];


        holder = new IndexHolder(0, y_positions.Length - 1, 0, z_positions.Length - 1);

        holder.HardSetValues(-1, holder.maxChild);

        InitialChargeUp();



        OnDepletion += Recharge;





    }






    void InitialChargeUp()
    {

        while (ChangeIndex(1) != 1) ;




    }



    async void Recharge()
    {
        
        
            while (ChangeIndex(1) != 1)
            {
            await Task.Delay(recharge_delay);
            }


        


        
    }

    delegate void Executable();
    public int ChangeIndex(int childDelta)
    {



        Executable exec = (childDelta > 0) ? CreateChargeAtHolder : DestroyChargeAtHolder;
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

    void CreateChargeAtHolder()
    {
        GameObject charge = null;
        try
        {
            charge = Instantiate(charge_prefab, transform, false);

            Material mat = (IsActive) ? charge.GetComponent<SliderLoaderChargeColorChange>().On : charge.GetComponent<SliderLoaderChargeColorChange>().Off;


            charge.GetComponent<Renderer>().material = mat;
            charge_grid[holder.parent, holder.child] = charge;

            charge.transform.parent = transform;

            charge.transform.localPosition = new Vector3(0, y_positions[holder.parent], z_positions[holder.child]);


        }
        catch (Exception)
        {
            DestroyImmediate(charge);
        }


    }


    void DestroyChargeAtHolder()
    {
        Destroy(charge_grid?[holder.parent, holder.child]);
    }




}

