using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreRingColorChange : MonoBehaviour
{
    // Start is called before the first frame update



    int color_degree = 16;
    [SerializeField] List<Material> color_mats;

    [SerializeField] Material primary, secondary;


    Renderer r;
    void Start()
    {

        r = GetComponent<Renderer>();
        InitColorUp();
        StartCoroutine(Change());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void InitColorUp()
    {

        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = secondary;
        }

        start_current_mats[PRIMARY_INDEX] = primary;

        start_current_mats[SECONDARY_INDEX] = secondary;


        r.materials = start_current_mats;

    }


    IEnumerator Change()
    {


        while (true) 
        {
        
        
        
       

                foreach (Material m in color_mats)
                {



                    Material[] newMats = new Material[r.materials.Length];
                    for (int i = 1; i <= color_degree; i++)
                    {
                        newMats[color_order_index_dict[i]] = m;
                    }


                    for (int i = color_degree+1; i < newMats.Length - color_degree; i++) 
                    {
                    newMats[color_order_index_dict[i]] = secondary;
                    }


                    newMats[PRIMARY_INDEX] = primary;

                    newMats[SECONDARY_INDEX] = secondary;




                    yield return new WaitForSeconds(1f);

                r.materials = newMats;


                }


        }






    }







    public void DecreaseDegree() 
    {
        color_degree--;
      
    }





    const int SECONDARY_INDEX = 0;


    const int PRIMARY_INDEX = 3;


    static readonly Dictionary<int, int> color_order_index_dict = new()
    {
        {1,1 },
        {2,12 },
        {3,7 },
        {4,11 },
        {5,10 },
        {6,6 },
        {7,4 },
        {8,8 },


        {9,9 },
        {10,17 },
        {11,5 },
        {12,16 },
        {13,15 },
        {14,2 },
        {15,14 },
        {16,13 },










    };













}
