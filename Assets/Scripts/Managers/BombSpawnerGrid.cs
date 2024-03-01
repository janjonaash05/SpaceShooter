using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using Random = System.Random;

public class BombSpawnerGrid : MonoBehaviour
{





    [SerializeField] GameObject placeholder_prefab, spawner_prefab;


    Vector3[,] positions;

    int size_x = 1;
    int size_y = 1;



    
    float margin = 30;

    
    float chance = 100;


    void Start()
    {
        positions = new Vector3[size_x,size_y];

        GenerateGrid();
    }


    void GenerateGrid() 
    {

        bool x_odd = size_x % 2 != 0;
        bool y_odd = size_y % 2 != 0;



        Vector3 center_position = transform.position;


        (int size, int margin) x_adjust = (x_odd) ? (1, 0) : (0,1);

        (int size, int margin) y_adjust = (y_odd) ? (1, 0) : (0, 1);

        

        Vector3 left_corner = center_position + new Vector3(-margin *(size_x- x_adjust.size) /2 +margin*x_adjust.margin/2  , -margin * (size_y - y_adjust.size)/2 + margin * y_adjust.margin / 2, 0);
            
            for(int i = 0; i < size_x; i++) 
            {
                float x = left_corner.x + margin * i;
                for (int j = 0; j < size_y; j++)
                {


                    GameObject toSpawn = new Random()
                    .Next(Mathf.RoundToInt(100/chance)-1) == 0?  

                    Instantiate(spawner_prefab, transform, false) : 
                    Instantiate(placeholder_prefab, transform, false);

                    float y = left_corner.y + margin * j;


                    

                    positions[i, j] = new Vector3(x, y, 0);
                    toSpawn.transform.parent = transform;
                    toSpawn.transform.localPosition = positions[i, j]; 
                }
            
            }

    
    
        

            


    
    
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
