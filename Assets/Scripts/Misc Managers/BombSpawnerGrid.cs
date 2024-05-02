using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class BombSpawnerGrid : MonoBehaviour
{






    public static event Action<string, NameMaterialPair> OnClusterEventSpawn;
    public static event Action OnClusterEventStart, OnClusterEventEnd;


    [SerializeField] GameObject placeholder_prefab, spawner_prefab;


    Vector3[,] positions;

    int size;
    




    const float MARGIN = 30;



    List<GameObject> placeholders = new();

    int start_amount;


    /// <summary>
    /// <para>Periodically starts, spawns and ends the cluster event.</para>
    /// <para>The spawning repeats based on the burst amount value</para>
    /// <para>Each spawn randomly chooses a tag and the color-name material pair from 2 sets</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator ClusterEventTimer()
    {

        WaitForSeconds one_second_wait = new(1f);
        WaitForSeconds five_second_wait = new(5f);

        while (true)
        {
            yield return new WaitForSeconds(DifficultyManager.GetCurrentBombClusterFrequencyValue());



            OnClusterEventStart?.Invoke();




            for (int i = 0; i < DifficultyManager.GetCurrentBombClusterBurstAmountValue(); i++)
            {
                string tag = UnityEngine.Random.Range(0, 2) == 0 ? Tags.LASER_TARGET_1 : Tags.LASER_TARGET_2;

                var pairs = tag.Equals(Tags.LASER_TARGET_1) ? MaterialHolder.Instance().NAMED_COLOR_SET_1() : MaterialHolder.Instance().NAMED_COLOR_SET_2();
                var pair = pairs[UnityEngine.Random.Range(0, pairs.Length)];

                
                OnClusterEventSpawn?.Invoke(tag, pair);

                yield return one_second_wait;


            }





            OnClusterEventEnd?.Invoke();
            yield return five_second_wait;










        }





    }






    void BombSpawnerForm() => StartCoroutine(SwitchPlaceholderForSpawner());


    private void OnDestroy()
    {
        DifficultyManager.OnBombSpawnerForm -= BombSpawnerForm;

    }

    void Start()
    {
        DifficultyManager.OnBombSpawnerForm += BombSpawnerForm;


        size = DifficultyManager.BOMB_GRIDxSIZE_DIFFICULTY_DICT[DifficultyManager.DIFFICULTY];



        float y_pos_offset = DifficultyManager.DIFFICULTY switch
        {
            DifficultyManager.Difficulty.EASY => 0,
            DifficultyManager.Difficulty.NORMAL => 20,
            DifficultyManager.Difficulty.HARD => 25,


        };

        transform.Translate(new(0, y_pos_offset, 0));


        start_amount = (size * size) / 3;
        positions = new Vector3[size, size];

        GenerateGrid();
        StartCoroutine(ClusterEventTimer());


    }

    /// <summary>
    /// <para>Creates a set of random pairs of array coordinates.</para>
    /// <para>Gets the center position, adjust based on parity, and calculates the top left corner.</para>
    /// <para>Iterates through all array coordinates, calculates x and y coordinates</para>
    /// <para>If the array coordinates are present in the set, spawns a spawner object and removes the pair from the set.</para>
    /// <para>If not, spawns spawns the placeholder object and adds it to a list.</para>
    /// <para>Finally, sets the spawned object parent as this object and its localPosition to (x,y,0) coordinates.</para>
    /// </summary>
    void GenerateGrid()
    {


        bool odd = size % 2 != 0;

        HashSet<(int i, int j)> loop_coordinates_for_spawns = new();


        while (loop_coordinates_for_spawns.Count < start_amount)
        {
            loop_coordinates_for_spawns.Add((new Random().Next(0, size), new Random().Next(0, size)));

        }

        Vector3 center_position = transform.position;
        (int size, int margin) adjust = (odd) ? (1, 0) : (0, 1);

        Vector3 top_left_corner = center_position + new Vector3(-MARGIN * (size - adjust.size) / 2 + MARGIN * adjust.margin / 2, -MARGIN * (size - adjust.size) / 2 + MARGIN * adjust.margin / 2, 0);

        for (int i = 0; i < size; i++)
        {
            float x = top_left_corner.x + MARGIN * i;
            for (int j = 0; j < size; j++)
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

                float y = top_left_corner.y + MARGIN * j;

                positions[i, j] = new Vector3(x, y, 0);
                toSpawn.transform.parent = transform;
                toSpawn.transform.localPosition = positions[i, j];
            }

        }










    }

    /// <summary>
    /// <para>Gets a random placeholder and removes it from the list.</para>
    /// <para>Starts playing the placeholder's particle system and disables its renderer.</para>
    /// <para>Instantiates a spawner object.</para>
    /// <para>Creates a copy of the spawner materials and an array filled with enemy upgrade color.</para>
    /// <para>Assigns the colors to the spawner, LERPs its scale from zero to target, sets its materials back to the copy.</para>
    /// <para>Waits until the particle system finished and destroys the placeholder.</para>
    /// </summary>
    /// <returns></returns>
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
        /*while (spawner.transform.localScale.x < target_scale.x)
        {
            lerp += Time.deltaTime;

            spawner.transform.localScale = new Vector3(Mathf.Lerp(0, target_scale.x, lerp), Mathf.Lerp(0, target_scale.y, lerp), Mathf.Lerp(0, target_scale.z, lerp));

        }
        */

        float duration = 0.75f;

        while (lerp < duration)
        {
            lerp += Time.deltaTime;
            spawner.transform.localScale = Vector3.Lerp(Vector3.zero, target_scale, lerp / duration);
        }


        spawner.GetComponent<Renderer>().materials = oldmats;



        yield return new WaitForSeconds(ps.duration);

        Destroy(placeholder);




    }











}
