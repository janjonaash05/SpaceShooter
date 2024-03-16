using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenColorChange : MonoBehaviour
{
    // Start is called before the first frame update




    Dictionary<int, int> HP_index_map = new() 
    {
        {1,3 },
        {2,2 },
        {3,5 },
        {4,4 },
    
    
    };


    Renderer rend;


    Material timer_color;
    Material color;
    Material off;
    void Start()
    {

        timer_color = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        
      

        rend = GetComponent<Renderer>();
        off = rend.materials[1];

        color = rend.materials[^1];


        GetComponent<TokenMovement>().OnHealthDecrease += (HP) => 
        {
            Debug.Log(HP + "TOKEN HP");

           Material[] mats = rend.materials;


            for (int i = 1; i<=4; i++)
            {
                mats[HP_index_map[i]] = off;
            }

            for (int i = 1; i <= HP; i++) 
            {
                mats[HP_index_map[i]] = timer_color;
            
            }

          

            rend.materials = mats;
        
        };
    }



    public void CoverInColor() 
    {
        var mats = new Material[rend.materials.Length];
        Array.Fill(mats, color);

        rend.materials = mats;
    
    }


    // Update is called once per frame
  


    




}
