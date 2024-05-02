using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FormConstellation : MonoBehaviour
{
    


    [SerializeField] GameObject star_prefab, supernova_prefab;

    Material[] mats;
    [SerializeField] float random_distance_factor;
    [SerializeField]

    Vector3[] constellation_star_offsets = {
        new(1, 0, 0),
        new(Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2,0 ),
        new(0, 1,0),
        new(-Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2,0 ),
        new(-1, 0, 0),
        new(-Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2, 0),
        new(0, -1, 0),
        new(Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2, 0),
    };



    List<GameObject> star_list;

    int STAR_AMOUNT = 8;

    [SerializeField][Tooltip("in ms")] int star_spawn_delay;


    bool spawn_locked = false;
    bool perma_spawn_locked = false;



    void Start()
    {

        

        mats = MaterialHolder.Instance().COLOR_SET_WHOLE();

        InvokeRepeating(nameof(Form), DifficultyManager.GetCurrentConstellationSpawnRateValue(), DifficultyManager.GetCurrentConstellationSpawnRateValue());


        HelperSpawnerManager.OnEMPSpawn += () => spawn_locked = true;
        SpinnerChargeUp.OnLaserShotPlayerDeath += () => perma_spawn_locked = true;
        HelperSpawnerManager.OnEMPDestroy += () => spawn_locked = false;






    }




    System.Random rand;


    /// <summary>
    /// <para>Creates the supernova, star list and pools of color and position indexes.</para>
    /// <para>For STAR_AMOUNT of times, gets a random entry from the pools, calls CreateStar() and removes the entries from the pools. Waits for a set amount of time.</para>
    /// <para>Calls CheckForCompletedFalls().</para>
    /// </summary>
    /// <returns></returns>
    async Task Form()
    {

        rand = new();


        if (spawn_locked || perma_spawn_locked) return;

        if (GameObject.FindGameObjectsWithTag(Tags.SUPERNOVA).Length != 0) return;
        

        STAR_AMOUNT = DifficultyManager.GetCurrentConstellationMaxStarsValue();



        var supenova = Instantiate(supernova_prefab, transform, false);
        supenova.transform.parent = transform;
        supenova.transform.localPosition = new Vector3(-86.5f, 0, 0);



        star_list = new();
        List<int> color_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();
        List<int> pos_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();



        for (int i = 0; i < STAR_AMOUNT; i++)
        {

            if(spawn_locked || perma_spawn_locked) return ;


            int color_index = color_index_pool[rand.Next(0, STAR_AMOUNT - i)];
            int pos_index = pos_index_pool[rand.Next(0, STAR_AMOUNT - i)];

           
            CreateStar(color_index, pos_index);


            color_index_pool.Remove(color_index);
            pos_index_pool.Remove(pos_index);



            await Task.Delay(star_spawn_delay);

        }



        CheckForCompletedFalls();




    }


    /// <summary>
    /// <para>Creates a star and adds it to the star list.</para>
    /// <para>Offsets it's position up and left using the pos_index.</para>
    /// <para>Sets up the StarChargeUp using the color_index.</para>
    /// <para>Calls RotateTowardsPlayer() and begins ChargeUp().</para>
    /// <para>Sets the star's particle system material to color_index material.</para>
    /// </summary>
    /// <param name="color_index"></param>
    /// <param name="pos_index"></param>
    void CreateStar(int color_index, int pos_index) 
    {
        var star = Instantiate(star_prefab, transform, false);

        star.transform.parent = transform;
        star.transform.localPosition = Vector3.zero;


        star_list.Add(star);





        float rand_multiplier = (float)(rand.NextDouble() * random_distance_factor) + 1;


        star.transform.Translate(rand_multiplier * 0.5f * constellation_star_offsets[pos_index].y * Vector3.up);
        star.transform.Translate(rand_multiplier * constellation_star_offsets[pos_index].x * Vector3.forward);

        star.GetComponent<StarChargeUp>().Setup(mats[color_index]);
        star.GetComponent<StarEmergence>().InitAndRotateTowardsPlayer();


        _ = star.GetComponent<StarChargeUp>().ChargeUp();


        star.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = mats[color_index];


    }





    /// <summary>
    /// Endlessly checks if all stars in the star list are null, if yes, then breaks and invokes OnAllStarsGone.
    /// </summary>
    void CheckForCompletedFalls()
    {
        IEnumerator checkForCompletedFalls()
        {
            while (true)
            {
                if (spawn_locked || perma_spawn_locked) yield break;


                bool ready = true;
                foreach (GameObject star in star_list)
                {
                    if (star != null) { ready = false; }
                }

                if (ready)
                {
                    OnAllStarsGone?.Invoke();
                    break;
                }



                yield return null;
            }



        }

        StartCoroutine(checkForCompletedFalls());

    }


    public static event Action OnAllStarsGone;

}
