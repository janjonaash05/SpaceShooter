using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class BombSpawnerGrid : MonoBehaviour
{





    [SerializeField] GameObject placeholder_prefab, spawner_prefab;


    Vector3[,] positions;

    [SerializeField] int size_x;
    [SerializeField] int size_y;




    const float MARGIN = 30;



    List<GameObject> placeholders = new();

    int start_amount;





    void BombSpawnerForm() => StartCoroutine(SwitchPlaceholderForSpawner());


    private void OnDestroy()
    {
        DifficultyManager.OnBombSpawnerForm -= BombSpawnerForm;

    }

    void Start()
    {
        DifficultyManager.OnBombSpawnerForm += BombSpawnerForm;

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
            loop_coordinates_for_spawns.Add((new Random().Next(0, size_x), new Random().Next(0, size_y)));

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
                    toSpawn = Instantiate(placeholder_prefab, transform, false);

                    placeholders.Add(toSpawn);
                }

                float y = left_corner.y + MARGIN * j;




                positions[i, j] = new Vector3(x, y, 0);
                toSpawn.transform.parent = transform;
                toSpawn.transform.localPosition = positions[i, j];
            }

        }










    }


    IEnumerator SwitchPlaceholderForSpawner()
    {
        GameObject placeholder = placeholders[new System.Random().Next(placeholders.Count)];
        placeholders.Remove(placeholder);


        ParticleSystem ps = placeholder.transform.GetChild(0).GetComponent<ParticleSystem>();
        ps.enableEmission = true;
        ps.Play();

        placeholder.GetComponent<Renderer>().enabled = false;

        GameObject spawner = Instantiate(spawner_prefab, transform, false);
        spawner.transform.parent = transform;
        spawner.transform.localPosition = placeholder.transform.localPosition;
        Vector3 target_scale = spawner.transform.localScale;

        spawner.transform.localScale = placeholder.transform.localScale;

        Material[] oldmats = new Material[spawner.GetComponent<Renderer>().materials.Length];
        Material[] newmats = new Material[spawner.GetComponent<Renderer>().materials.Length];


        Array.Copy(spawner.GetComponent<Renderer>().materials, oldmats, spawner.GetComponent<Renderer>().materials.Length);
        Array.Fill(newmats, MaterialHolder.Instance().ENEMY_UPGRADE());



        spawner.GetComponent<Renderer>().materials = newmats;

        float lerp = 0;
        while (spawner.transform.localScale.x < target_scale.x)
        {
            lerp += Time.deltaTime;

            spawner.transform.localScale = new Vector3(Mathf.Lerp(0, target_scale.x, lerp), Mathf.Lerp(0, target_scale.y, lerp), Mathf.Lerp(0, target_scale.z, lerp));

        }

        spawner.GetComponent<Renderer>().materials = oldmats;

        spawner.transform.localScale = target_scale;

        yield return new WaitForSeconds(ps.duration);

        Destroy(placeholder);




    }









    // Update is called once per frame
    void Update()
    {

    }
}
