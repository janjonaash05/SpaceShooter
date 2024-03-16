using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using Random = System.Random;

public class BombSpawnerGrid : MonoBehaviour
{





    [SerializeField] GameObject placeholder_prefab, spawner_prefab;


    Vector3[,] positions;

    [SerializeField] int size_x;
    [SerializeField] int size_y;




    const float MARGIN = 30;


    int start_amount;


    void Start()
    {


        start_amount = (size_x * size_y) / 3;
        positions = new Vector3[size_x, size_y];

        GenerateGrid();
    }


    void GenerateGrid()
    {

        bool x_odd = size_x % 2 != 0;
        bool y_odd = size_y % 2 != 0;



        HashSet<(int i, int j)> loop_coordinates_for_spawns = new();


        while (loop_coordinates_for_spawns.Count <= start_amount) 
        {
            loop_coordinates_for_spawns.Add((new Random().Next(0,size_x), new Random().Next(0,size_y)));
        
        }



       




        Vector3 center_position = transform.position;


        (int size, int margin) x_adjust = (x_odd) ? (1, 0) : (0, 1);

        (int size, int margin) y_adjust = (y_odd) ? (1, 0) : (0, 1);



        Vector3 left_corner = center_position + new Vector3(-MARGIN * (size_x - x_adjust.size) / 2 + MARGIN * x_adjust.margin / 2, -MARGIN * (size_y - y_adjust.size) / 2 + MARGIN * y_adjust.margin / 2, 0);

        for (int i = 0; i < size_x; i++)
        {
            float x = left_corner.x + MARGIN * i;
            for (int j = 0; j < size_y; j++)
            {
                GameObject toSpawn;
                if (loop_coordinates_for_spawns.Contains((i, j)))
                {
                    toSpawn = Instantiate(spawner_prefab, transform, false);


                    loop_coordinates_for_spawns.Remove((i, j));

                }
                else 
                {
                    toSpawn=  Instantiate(placeholder_prefab, transform, false);
                }

                /*
                GameObject toSpawn = new Random()
                .Next(Mathf.RoundToInt(100 / chance) - 1) == 0 ?

                Instantiate(spawner_prefab, transform, false) :
                Instantiate(placeholder_prefab, transform, false);
                */
                float y = left_corner.y + MARGIN * j;




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
