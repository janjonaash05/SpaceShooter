using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SupernovaColorChange : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] Material[] color_mats;

    [SerializeField] Material primary, secondary;


    Renderer r;



    List<Material> colors;
    




    void InitColorUp()
    {

        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = secondary;
        }

        start_current_mats[PRIMARY_INDEX] = primary;

        start_current_mats[CENTER_INDEX] = secondary;


        r.materials = start_current_mats;

    }



    void Start()
    {

        colors = new();


        r = GetComponent<Renderer>();


        InitColorUp();











        IEnumerator change()
        {

            for (int i = 0; i < color_mats.Length; i++)
            {



                AddColor(color_mats[i]);
                current_color_index++;
                    


                yield return new WaitForSeconds(0.1f);
            }





        }

        StartCoroutine(change());



    }

    // Update is called once per frame
    void Update()
    {

    }





    int current_color_index = 1 ;

    void AddColor(Material color) 
    {
        Material[] new_mats = new Material[r.materials.Length];



        int index_to_change = color_order_index_dict[current_color_index];
        for (int j = 0; j < r.materials.Length; j++)
        {

            new_mats[j] = j == index_to_change ? color: r.materials[j];

        }




        r.materials = new_mats;

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
