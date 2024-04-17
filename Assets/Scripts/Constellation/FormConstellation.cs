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
        new Vector3(1, 0, 0),
        new Vector3(Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2,0 ),
        new Vector3(0, 1,0),
        new Vector3(-Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2,0 ),
        new Vector3(-1, 0, 0),
        new Vector3(-Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2, 0),
        new Vector3(0, -1, 0),
        new Vector3(Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2, 0),
    };



    

    /*
    Vector3[] constellation_star_offsets = {
        new Vector3(0, 0, 1),
        new Vector3(0, Mathf.Sqrt(2) / 2,Mathf.Sqrt(2) / 2 ),
        new Vector3(0, 1,0),
        new Vector3(0, Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2),
        new Vector3(0, 0, -1),
        new Vector3(0, -Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2),
        new Vector3(0, -1, 0),
        new Vector3(0, -Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2),
    };
    */

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





    async Task Form()
    {


        if (spawn_locked || perma_spawn_locked) return;

        if (GameObject.FindGameObjectsWithTag(Tags.SUPERNOVA).Length != 0) return;
        

        STAR_AMOUNT = DifficultyManager.GetCurrentConstellationMaxStarsValue();



        var nova = Instantiate(supernova_prefab, transform, false);
        nova.transform.parent = transform;
        nova.transform.localPosition = new Vector3(-86.5f, 0, 0);





        star_list = new();
        List<int> color_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();
        List<int> pos_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();



        for (int i = 0; i < STAR_AMOUNT; i++)
        {

            if(spawn_locked || perma_spawn_locked) return ;



            var star = Instantiate(star_prefab, transform, false);

            star.transform.parent = transform;
            star.transform.localPosition = Vector3.zero;


            star_list.Add(star);

            System.Random r = new();

            int color_index = color_index_pool[r.Next(0, STAR_AMOUNT - i)];
            int pos_index = pos_index_pool[r.Next(0, STAR_AMOUNT - i)];



            float rand_multiplier = (float)(r.NextDouble() * random_distance_factor) + 1;


            star.transform.Translate(rand_multiplier *0.5f * constellation_star_offsets[pos_index].y * Vector3.up);
            star.transform.Translate(rand_multiplier * constellation_star_offsets[pos_index].x * Vector3.forward);

            star.GetComponent<StarChargeUp>().Setup(mats[color_index]);
            star.GetComponent<StarEmergence>().RotateTowardsPlayer();


            _ = star.GetComponent<StarChargeUp>().ChargeUp();


            star.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = mats[color_index];



            color_index_pool.Remove(color_index);
            pos_index_pool.Remove(pos_index);



            await Task.Delay(star_spawn_delay);

        }



        CheckForCompletedFalls();




    }








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
