using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaColorChange : MonoBehaviour, IEMPDisruptable
{




    [SerializeField] List<Material> color_mats;

    [SerializeField] Material primary, secondary;


    [SerializeField] float color_change_delay;

    [SerializeField] Material white;


    Renderer r;




    public event Action OnColorUpFinished;

    public event Action OnDeathColorUpFinished;






    /// <summary>
    /// Sets all materials to secondary except for the one at PRIMARY_INDEX, which is set to primary.
    /// </summary>
    void InitColorUp()
    {

        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = secondary;
        }

        start_current_mats[PRIMARY_INDEX] = primary;
        


        r.materials = start_current_mats;

    }



    /// <summary>
    /// Copies the materials and changes the one at CENTER_INDEX to arg m, reassigns them back to the renderer.
    /// </summary>
    /// <param name="m"></param>
    void OnlyCenterColorUp(Material m)
    {
        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = r.materials[i];
        }

        start_current_mats[CENTER_INDEX] = m;

        r.materials = start_current_mats;

    }



    /// <summary>
    /// Adds arg m to the color_mats, calls OnlyCenterColorUp() with arg m.
    /// </summary>
    /// <param name="m"></param>
    void SetCentertAndAddColorToList(Material m)
    {
        color_mats.Add(m);
        OnlyCenterColorUp(m);
    }





    void Start()
    {



        StarFall.OnStarFallen += SetCentertAndAddColorToList;
        FormConstellation.OnAllStarsGone += AllColorUp;

        color_mats = new();


        r = GetComponent<Renderer>();


        InitColorUp();



    }

    private void OnDestroy()
    {
        StarFall.OnStarFallen -= SetCentertAndAddColorToList;
        FormConstellation.OnAllStarsGone -= AllColorUp;
    }











    /// <summary>
    /// Calls OnlyCenterColorUp with white.
    /// <para>Over time calls AddColor with all colors from color_mats. </para>
    /// <para>Invokes OnColorUpFinished. </para>
    /// </summary>
    void AllColorUp()
    {
        OnlyCenterColorUp(white);

        IEnumerator colorUp()
        {

            for (int i = 0; i < color_mats.Count; i++)
            {
                AddColor(color_mats[i]);
                yield return new WaitForSeconds(color_change_delay);
            }


            OnColorUpFinished?.Invoke();

        }

        StartCoroutine(colorUp());

    }








    int current_color_index = 1;


    /// <summary>
    /// Plays the SUPERNOVA_CHARGE_UP sound.
    /// <para>Gets the index to change based on the current_color_index.</para>
    /// <para>Goes through all materials, if the iteration is equal to the index to change, changes the material to arg color.</para></para>
    /// <para>Increases the current_color_index by 1.</para>
    /// 
    /// </summary>
    /// <param name="color"></param>
    void AddColor(Material color)
    {

        AudioManager.PlayActivitySound(AudioManager.ActivityType.SUPERNOVA_CHARGE_UP);

        Material[] new_mats = new Material[r.materials.Length];



        int index_to_change = color_order_index_dict[current_color_index];
        for (int j = 0; j < r.materials.Length; j++)
        {

            new_mats[j] = j == index_to_change ? color : r.materials[j];

        }




        r.materials = new_mats;
        current_color_index++;

    }

    /// <summary>
    /// Decreases the current_color_index by 1, gets the index to change based on it.
    /// <para>Goes through all materials, if the iteration is equal to the index to change, changes the material to secondary.</para>
    /// </summary>
    public void RemoveColor()
    {
        current_color_index--;

        Material[] new_mats = new Material[r.materials.Length];



        int index_to_change = color_order_index_dict[current_color_index];
        for (int i = 0; i < r.materials.Length; i++)
        {

            new_mats[i] = i == index_to_change ? secondary : r.materials[i];

        }




        r.materials = new_mats;


    }

    public void OnEMP()
    {
        StopAllCoroutines();
    }

    const int CENTER_INDEX = 0;


    const int PRIMARY_INDEX = 1;


    static readonly Dictionary<int, int> color_order_index_dict = new()
    {
        {1,9 },
        {2,2 },
        {3,4 },
        {4,6 },
        {5,7 },
        {6,3 },
        {7,8 },
        {8,5 },
    };









}
