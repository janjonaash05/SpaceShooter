using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DisruptorColorChange : MonoBehaviour, IScoreEnumerable, IEMPDisruptable
{



    Material[] mats_storage;
    [SerializeField] Material white, primary, secondary;
    [SerializeField] float delay;
    [SerializeField] GameObject charge1, charge2;
    Dictionary<int, int> index_order_dict;

    Renderer rend;





    public static event Action<Material> OnColorChange;





    void Awake()
    {


        mats_storage = MaterialHolder.Instance().COLOR_SET_WHOLE();





        rend = GetComponent<Renderer>();
        index_order_dict = new Dictionary<int, int>() { { 1, 8 }, { 2, 7 }, { 3, 6 }, { 4, 5 }, { 5, 4 }, { 6, 3 }, { 7, 2 }, { 8, 9 } };

        InitialColorSetup();
        GetComponent<DisruptorStartEndMovement>().OnMoveUpFinish += StartColorChange;
    }

    /// <summary>
    /// Stops all Coroutines and destroys all children.
    /// </summary>
    public void OnEMP()
    {
        StopAllCoroutines();

        for (int i = 0; i < transform.childCount; i++)
        {

            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void StartColorChange()
    {
        StartCoroutine(ColorChange());
    }



    /// <summary>
    /// Assigns the materials to primary, secondary and the rest white.
    /// </summary>
    void InitialColorSetup()
    {

        Material[] mats = new Material[rend.materials.Length];
        mats[0] = rend.materials[0];
        mats[1] = rend.materials[1];



        for (int i = 2; i < mats.Length; i++)
        {
            mats[i] = white;
        }

        rend.materials = mats;
    }


    int i = 0;


    /// <summary>
    /// <para>Changes all white bars to colored ones over time, invoking OnColorChange for each color.</para>
    /// <para>Starts flashing the charges.</para>
    /// <para>Sets DisruptorMovement targeting and initiates the DisruptorTargetTurretHeads rotations. </para>
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorChange()
    {

        FlashDisruptorCharge charge1_flash_charge = charge1.GetComponent<FlashDisruptorCharge>();
        FlashDisruptorCharge charge2_flash_charge = charge2.GetComponent<FlashDisruptorCharge>();

        for (i = 1; i < 9; i++)
        {

            var color_mats = new Material[10];


            color_mats[0] = primary;
            color_mats[1] = secondary;

            for (int j = 2; j < 10; j++)
            {
                color_mats[j] = white;
            }

            for (int k = 0; k < i; k++)
            {

                color_mats[index_order_dict[k + 1]] = mats_storage[k];

                OnColorChange?.Invoke(mats_storage[k]);

            }

            rend.materials = color_mats;

            yield return new WaitForSeconds(delay);

        }


        charge1_flash_charge.FlashAllColors(mats_storage);
        charge2_flash_charge.FlashAllColors(mats_storage);



        GetComponent<DisruptorMovement>().SetTargeting(true);
        GetComponent<DisruptorTargetTurretHeads>().InitiateRotations();
    }


 
    public bool DisabledRewards { get; set; }
    public int CalculateScoreReward()
    {
        return (9 - i) * 5;
    }


}
