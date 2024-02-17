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


    Material color;
    Material off;
    void Start()
    {

        color = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        
      

        rend = GetComponent<Renderer>();
        off = rend.materials[1];
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
                mats[HP_index_map[i]] = color;
            
            }

          

            rend.materials = mats;
        
        };
    }



    Dictionary<int, int> hp_index = new();



    // Update is called once per frame
    void Update()
    {
        
    }


    void DecreaseHealth() 
    {
    
    
    }




}
