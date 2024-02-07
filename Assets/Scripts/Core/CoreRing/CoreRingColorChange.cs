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



     List<Material> mats_storage;

    [SerializeField] Material primary, secondary;


    Renderer rend;


    MaterialIndexHolder index_holder;




    public static event Action<Material> OnMaterialChange;



    
    void Start()
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


    private void Init()
    {
        index_holder = CoreCommunication.CORE_INDEX_HOLDER;
        
        rend = GetComponent<Renderer>();
        ColorUp(mats_storage[0]);
        StartCoroutine(ColorChange());
    }

    // Update is called once per frame
    void Update()
    {

    }


    void ColorUp(Material mat)
    {

        Material[] mats = new Material[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
            mats[i] = mat;
        }

        mats[PRIMARY_INDEX] = primary;

        mats[SECONDARY_INDEX] = secondary;


        rend.materials = mats;

    }



    void ColorUpOff() 
    {

        Material[] mats = new Material[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
            mats[i] = primary;
        }

        mats[PRIMARY_INDEX] = primary;

        mats[SECONDARY_INDEX] = secondary;


        rend.materials = mats;

    }





    void AssignBasicColors(Material[] newMats)
    {

        newMats[PRIMARY_INDEX] = primary;
        newMats[SECONDARY_INDEX] = secondary;
    }



    const int SECONDARY_INDEX = 1;


    const int PRIMARY_INDEX = 0;
    void ChangeMaterialArray()
    {

        if (index_holder.edge ==MaterialIndexHolder.Edge.UPPER) { ColorUp(changing_mat); return; }
        else if (index_holder.edge == MaterialIndexHolder.Edge.LOWER) { ColorUpOff(); return; }
        int size = rend.materials.Length;

        Material[] newMats = new Material[size];



        

        var copyHolder = new MaterialIndexHolder(index_holder.Parent, index_holder.Child, MaterialIndexHolder.Target.CORE, index_holder.edge);
        var colorlist = copyHolder.AllMatIndexesByHolder(true);




        //Debug.Log(copyHolder + " pre");

        copyHolder.ChangeIndex(0, 1);




        //  Debug.Log(copyHolder + " post");


        var offlist = copyHolder.AllMatIndexesByHolder(false);

        Debug.Log(colorlist.Count + " C " + offlist.Count + " O");

        Debug.Log(colorlist.ToCommaSeparatedString() + " C," + offlist.ToCommaSeparatedString() + " O");



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
