using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class CoreRingColorChange : MonoBehaviour
{




    List<Material> mats_storage;

    [SerializeField] Material primary, secondary;


    Renderer rend;


    MaterialIndexHolder index_holder;




    public static event Action<Material> OnMaterialChange;



    /// <summary>
    /// <para>Gets the material storage, assigns a function to the OnParentValueChangedCore which sets the changing mat based on the core index holder parent value.</para>
    /// </summary>
    void Awake()
    {
        mats_storage = MaterialHolder.Instance().PLAYER_HEALTH_SET().ToList();
        CoreCommunication.OnCommunicationInit += Init;

        changing_mat = mats_storage[0];


        CoreCommunication.OnParentValueChangedCore += () =>
        {

            changing_mat = (CoreCommunication.CORE_INDEX_HOLDER.Parent) switch
            {
                5 => mats_storage[0],
                4 => mats_storage[1],
                3 => mats_storage[2],
                2 => mats_storage[3],
                1 => mats_storage[4],
                0 => mats_storage[4],
                _ => mats_storage[4]

            };

        };




    }



    private void OnDestroy()
    {
        CoreCommunication.OnCommunicationInit -= Init;
    }


    private void Init()
    {
        index_holder = CoreCommunication.CORE_INDEX_HOLDER;
        rend = GetComponent<Renderer>();
        ColorUp(mats_storage[0]);
        StartCoroutine(ColorChange());
    }


    /// <summary>
    /// Fills the materials array with the mat, then AssignBasicColors.
    /// </summary>
    void ColorUp(Material mat)
    {

        Material[] mats = new Material[rend.materials.Length];

        Array.Fill(mats, mat);

        AssignBasicColors(mats); ;


        rend.materials = mats;

    }


    /// <summary>
    /// Fills the materials array with primary color, then AssignBasicColors.
    /// </summary>
    void ColorUpOff()
    {

        Material[] mats = new Material[rend.materials.Length];
        Array.Fill(mats, primary);

        AssignBasicColors(mats);


        rend.materials = mats;

    }





    void AssignBasicColors(Material[] newMats)
    {

        newMats[PRIMARY_INDEX] = primary;
        newMats[SECONDARY_INDEX] = secondary;
    }



    const int SECONDARY_INDEX = 1;
    const int PRIMARY_INDEX = 0;


    /// <summary>
    /// <para>If the index holder is on an edge, calls either ColorUp or ColorUpOff based on the type of edge and returns early.</para>
    /// <para>Grabs the offlist and the colorlist based on a copy index holder, assigns materials to a new array based on the lists, calls AssignBasicColors and reassigns the renderer materials. </para>
    /// </summary>
    void ChangeMaterialArray()
    {

        if (index_holder.edge == MaterialIndexHolder.Edge.UPPER) { ColorUp(changing_mat); return; }
        else if (index_holder.edge == MaterialIndexHolder.Edge.LOWER) { ColorUpOff(); return; }
        int size = rend.materials.Length;

        Material[] newMats = new Material[size];

        var copyHolder = new MaterialIndexHolder(index_holder.Parent, index_holder.Child, MaterialIndexHolder.Target.CORE, index_holder.edge);
        var colorlist = copyHolder.AllMatIndexesByHolder(true);


        copyHolder.ChangeIndex(0, 1);
        var offlist = copyHolder.AllMatIndexesByHolder(false);

        if (offlist.Count > 0)
        {
            foreach (int i in offlist)
            {
                newMats[i] = primary;
            }
        }

        if (colorlist.Count > 0)
        {
            foreach (int i in colorlist)
            {


                newMats[i] = changing_mat;

            }
        }

        AssignBasicColors(newMats);
        rend.materials = newMats;

    }

    public Material changing_mat { get; private set; }
    /// <summary>
    /// Invokes OnMaterialChange with the changing material, calls ChangeMaterialArray() and waits a set amount of time. (repeated forever)
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorChange()
    {

        while (true)
        {
            OnMaterialChange?.Invoke(changing_mat);
            ChangeMaterialArray();
            yield return new WaitForSeconds(CoreCommunication.CHANGE_TIME);

        }

    }




























}
