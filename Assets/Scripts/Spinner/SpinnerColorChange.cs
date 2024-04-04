using System;
using System.Collections;
using System.Runtime.Serialization;

using UnityEngine;
using UnityEngine.Animations;

public class SpinnerColorChange : MonoBehaviour
{


    public static Material CHANGING_MAT { get; private set; }



    Material[] mats_storage;

    Material secondary, primary;




    public MaterialIndexHolder index_holder;
    public bool charge_up_mode;

    public GameObject charge;
    Renderer rend;

    public static event Action<Material> OnMaterialChange;

    const int PRIMARY_INDEX = 1, SECONDARY_INDEX = 0, CHARGING_INDEX = 2;


    // void SpinnerChargeUpStart() => EngageChargeUp(true);
    // void SpinnerChargeUpEnd() => EngageChargeUp(false);


    void SpinnerChargeUpStart() => charge_up_mode = true;
    void SpinnerChargeUpEnd() => charge_up_mode = false;





    private void OnDestroy()
    {
        CoreCommunication.OnSpinnerChargeUpStart -= SpinnerChargeUpStart;
        CoreCommunication.OnSpinnerChargeUpEnd -= SpinnerChargeUpEnd;
        CoreCommunication.OnSpinnerInitialColorUp -= InitialColorSetup;

    }


    void Start()
    {



        mats_storage = MaterialHolder.Instance().COLOR_SET_WHOLE();

        CoreCommunication.OnSpinnerChargeUpStart += SpinnerChargeUpStart;
        CoreCommunication.OnSpinnerChargeUpEnd += SpinnerChargeUpEnd;
        CoreCommunication.OnSpinnerInitialColorUp += InitialColorSetup;



        rend = GetComponent<Renderer>();

        secondary = GetComponent<Renderer>().materials[0];
        primary = GetComponent<Renderer>().materials[1];

        index_holder = CoreCommunication.SPINNER_INDEX_HOLDER;
        charge_up_mode = false;
        // SpinnerIndexHolder.LoadMap();

        InitialColorSetup();


        StartCoroutine(ColorChange());


    }







    void AssignBasicColors(Material[] newMats)
    {

        newMats[PRIMARY_INDEX] = primary;
        newMats[SECONDARY_INDEX] = secondary;
        newMats[CHARGING_INDEX] = charge_up_mode ? CHANGING_MAT : secondary;
    }

    void InitialColorSetup()
    {
        // Renderer rend = GetComponent<Renderer>();
        Material[] newMats = new Material[rend.materials.Length];

        for (int i = 0; i < rend.materials.Length; i++)
        {

            newMats[i] = primary;
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














    public void ChangeMaterialArray()
    {

        if (index_holder.Parent == 0) { return; }

        int size = rend.materials.Length;

        Material[] newMats = new Material[size];





        var copyHolder = new MaterialIndexHolder(index_holder.Parent, index_holder.Child, MaterialIndexHolder.Target.SPINNER, index_holder.edge);



        var colorlist = copyHolder.AllMatIndexesByHolder(true);

        copyHolder.ChangeIndex(0, 1);
        var offlist = copyHolder.AllMatIndexesByHolder(false);

        Debug.Log(colorlist.Count + " C " + offlist.Count + " O");




        if (colorlist.Count > 0)
        {
            foreach (int i in colorlist)
            {


                newMats[i] = CHANGING_MAT;

            }
        }
        if (offlist.Count > 0)
        {
            foreach (int i in offlist)
            {


                newMats[i] = primary;
            }
        }


        AssignBasicColors(newMats);


        rend.materials = newMats;
        charge.GetComponent<Renderer>().materials = new[] { CHANGING_MAT, CHANGING_MAT };



    }


    IEnumerator ColorChange()
    {

        while (true)
        {

            foreach (Material m in mats_storage)
            {

                CHANGING_MAT = m;
                OnMaterialChange?.Invoke(m);
                ChangeMaterialArray();

                yield return new WaitForSeconds(CoreCommunication.CHANGE_TIME);



            }


        }

    }
}

