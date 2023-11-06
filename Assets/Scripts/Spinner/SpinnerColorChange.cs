using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class SpinnerColorChange : MonoBehaviour
{


    Material changing_mat;
    public float change_time;


    public Material[] mats_storage;

    Material secondary, primary;




    public SpinnerIndexHolder index_holder;
    public bool charge_up_mode;

    public GameObject charge;
    Renderer rend;



    const int PRIMARY_INDEX = 1, SECONDARY_INDEX = 0, CHARGING_INDEX = 18;


    void Start()
    {
        LaserTurretCommunicationSO1.OnManualTargeting += (g) => ChangeIndexHolder(0, -1);
        LaserTurretCommunicationSO2.OnManualTargeting += (g) => ChangeIndexHolder(0, -1);







        rend = GetComponent<Renderer>();

        secondary = GetComponent<Renderer>().materials[0];
        primary = GetComponent<Renderer>().materials[1];

        index_holder = new SpinnerIndexHolder(0, 4);
        charge_up_mode = false;
        SpinnerIndexHolder.LoadMap();

        InitialColorSetup();


        StartCoroutine(colorChange());


    }

    public void ChangeIndexHolder(int parentDelta, int childDelta)
    {
        if (childDelta < 0) { EngageChargeUp(false); }


        int changeResult = index_holder.ChangeIndex(parentDelta, childDelta);


        if (changeResult == -1) { InitialColorSetup(); }
        if (changeResult == 1 && !charge_up_mode) { EngageChargeUp(true); }



    }

    void AssignBasicColors(Material[] newMats) 
    {

        newMats[PRIMARY_INDEX] = primary;
        newMats[SECONDARY_INDEX] = secondary;
        newMats[CHARGING_INDEX] = charge_up_mode ? changing_mat : secondary;
    }

    void InitialColorSetup()
    {
        // Renderer rend = GetComponent<Renderer>();
        Material[] newMats = new Material[rend.materials.Length];

        for (int i = 0; i < rend.materials.Length; i++)
        {

            newMats[i] = secondary;
        }

        AssignBasicColors(newMats);
        rend.materials = newMats;
    }




    void EngageChargeUp(bool start)
    {
        if (start)
        {
            Debug.Log("Engaging chargeup");
            charge_up_mode = true;
            GetComponent<SpinnerChargeUp>().StartCharging();
        }
        else
        {
            Debug.Log("Disengaging chargeup");
            charge_up_mode = false;
            GetComponent<SpinnerChargeUp>().EndCharging();
        }



    }




    // Update is called once per frame








    void ChangeMaterialArray()
    {

        if (index_holder.parent == 0) { return; }

        //Renderer rend = GetComponent<Renderer>();
        int size = rend.materials.Length;

        Material[] newMats = new Material[size];

        /*
        for (int i = 0; i < 2; i++)
        {
            newMats[i] = rend.materials[i];
        }
        newMats[CHARGING_INDEX] = (charge_up_mode) ? changing_mat : secondary;
        */

        AssignBasicColors(newMats);


        var copyHolder = new SpinnerIndexHolder(index_holder.parent, index_holder.child);
        var colorlist = SpinnerIndexHolder.AllMatIndexesByHolder(copyHolder, true);
        copyHolder.ChangeIndex(0, 1);
        var offlist = SpinnerIndexHolder.AllMatIndexesByHolder(copyHolder, false);



        if (colorlist.Count > 0)
        {
            foreach (int i in colorlist)
            {


                newMats[i] = changing_mat;

            }
        }
        if (offlist.Count > 0)
        {
            foreach (int i in offlist)
            {

                newMats[i] = secondary;
            }
        }




        rend.materials = newMats;
        charge.GetComponent<Renderer>().material = changing_mat;



    }


  
    IEnumerator colorChange()
    {

        while (true)
        {

            for (int i = 0; i < mats_storage.Length; i++)
            {


                
            

                changing_mat = mats_storage[i];

                ChangeMaterialArray();

                yield return new WaitForSeconds(change_time);



            }


        }

    }
}

