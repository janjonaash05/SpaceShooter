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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    IEnumerator Change()
    {

        foreach (Material m in color_mats)
        {



            Material[] newMats = new Material[color_mats.Count];
            for (int i = 1; i <= color_degree; i++)
            {
                newMats[i] = m;
            }


            for (int i = color_degree+1; i < newMats.Length - color_degree; i++) 
            {
                newMats[i] = primary;
            }


            newMats[PRIMARY_INDEX] = primary;

            newMats[SECONDARY_INDEX] = secondary;




            yield return new WaitForSeconds(1f);




        }
    







    
    }













    const int SECONDARY_INDEX = 3;


    const int PRIMARY_INDEX = 1;


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
