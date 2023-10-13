using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FormConstellation : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] GameObject star_prefab;
    [SerializeField] Material[] mats;
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

    const int STAR_AMOUNT = 8;

    [SerializeField][Tooltip("in ms")] int star_spawn_delay;

    void Start()
    {


        Form();






    }





    async void Form()
    {
        star_list = new();
        List<int> color_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();
        List<int> pos_index_pool = Enumerable.Range(0, STAR_AMOUNT).ToList();



        for (int i = 0; i < STAR_AMOUNT; i++)
        {
            var star = Instantiate(star_prefab, transform, false);

            star.transform.parent = transform;
            star.transform.localPosition = Vector3.zero;


            star_list.Add(star);

            System.Random r = new();

            int color_index = color_index_pool[r.Next(0, STAR_AMOUNT - i)];
            int pos_index = pos_index_pool[r.Next(0, STAR_AMOUNT - i)];



            float rand_multiplier = (float)(r.NextDouble() * random_distance_factor) + 1;


            star.transform.Translate(rand_multiplier * constellation_star_offsets[pos_index].y * Vector3.up);
            star.transform.Translate(rand_multiplier * constellation_star_offsets[pos_index].x * Vector3.forward);

            star.GetComponent<StarChargeUp>().Setup(mats[color_index]);
            _ = star.GetComponent<StarChargeUp>().ChargeUp();

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

              //  Debug.Log("stars: " + star_list.Count);

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
    // Update is called once per frame
    void Update()
    {

    }
}
