using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDisruptor : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject prefab;




    bool spawn_locked = false;
    bool perma_spawn_locked = false;

    void Start()
    {
        StartCoroutine(Spawn());

        HelperSpawnerManager.OnEMPSpawn += () => spawn_locked = true;
        SpinnerChargeUp.OnLaserShotPlayerDeath += () => perma_spawn_locked = true;

        HelperSpawnerManager.OnEMPDestroy += () => spawn_locked = false;




    }

    // Update is called once per frame


    IEnumerator Spawn()
    {

        while (true)
        {

            if (spawn_locked || perma_spawn_locked)
            {
                yield return null; continue;
            }



            if (GameObject.FindGameObjectsWithTag(Tags.DISRUPTOR).Length == 0)
            {
                int chance = 100 / DifficultyManager.DISRUPTOR_SPAWN_CHANCE;


                if (new System.Random().Next(0, chance) != 0) { yield return new WaitForSeconds(DifficultyManager.DISRUPTOR_SPAWN_DELAY); continue; }

                GameObject b = Instantiate(prefab, transform.position, prefab.transform.rotation);



                b.transform.GetChild(0).GetComponent<MoveDisruptorCharge>().SetTargets(1);
                b.transform.GetChild(1).GetComponent<MoveDisruptorCharge>().SetTargets(2);

                yield return new WaitForSeconds(DifficultyManager.DISRUPTOR_SPAWN_DELAY);

            }
        }

    }
}
