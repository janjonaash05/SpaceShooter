using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class CoreRingColorChange : MonoBehaviour
{
    // Start is called before the first frame update



    int color_degree = 16;
    [SerializeField] List<Material> mats_storage;

    [SerializeField] Material primary, secondary;


    Renderer rend;


    MaterialIndexHolder index_holder;
    void Start()
    {

        rend = GetComponent<Renderer>();
        InitColorUp();
        StartCoroutine(ColorChange());


        index_holder = CoreCommunication.CORE_INDEX_HOLDER;

        



    }

    // Update is called once per frame
    void Update()
    {

    }


    void InitColorUp()
    {

        Material[] start_current_mats = new Material[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
            start_current_mats[i] = secondary;
        }

        start_current_mats[PRIMARY_INDEX] = primary;

        start_current_mats[SECONDARY_INDEX] = secondary;


        rend.materials = start_current_mats;

    }






    void AssignBasicColors(Material[] newMats)
    {

        newMats[PRIMARY_INDEX] = primary;
        newMats[SECONDARY_INDEX] = secondary;
    }



    const int SECONDARY_INDEX = 0;


    const int PRIMARY_INDEX = 3;
    void ChangeMaterialArray()
    {

        if (index_holder.parent == 0) { return; }

        int size = rend.materials.Length;

        Material[] newMats = new Material[size];





        var copyHolder = new MaterialIndexHolder(index_holder.parent, index_holder.child, MaterialIndexHolder.Target.SPINNER);
        var colorlist = copyHolder.AllMatIndexesByHolder(true);

        copyHolder.ChangeIndex(0, 1);
        var offlist = copyHolder.AllMatIndexesByHolder(false);

        Debug.Log(colorlist.Count + " C " + offlist.Count + " O");

        Debug.Log(colorlist.ToCommaSeparatedString() + " C," + offlist.ToCommaSeparatedString() + " O");


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
                newMats[i] = primary;
            }
        }
        AssignBasicColors(newMats);


        rend.materials = newMats;



    }

    Material changing_mat;
    IEnumerator ColorChange()
    {

        while (true)
        {

            foreach (Material m in mats_storage)
            {


                changing_mat = m;

                ChangeMaterialArray();

                yield return new WaitForSeconds(CoreCommunication.CHANGE_TIME);



            }


        }

    }








  



















}
