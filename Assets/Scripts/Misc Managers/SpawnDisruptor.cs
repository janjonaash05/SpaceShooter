using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDisruptor : MonoBehaviour
{
    

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

    

    /// <summary>
    /// In an endless loop, unless spawn_locked or perma_spawn_locked:
    /// <para>If there are no gameObjects with the DISRUPTOR tag:</para>
    /// <para>Calculates the spawn chance and attempts to roll 0, if unsuccessful, skips the iteration.</para>
    /// <para>Instantiates the disruptor prefab at this gameObjects position with the prefab's rotation.</para>
    /// <para>Assigns targets to its charges.</para>
    /// <para>Waits a spawn delay.</para>
    ///
    /// </summary>
    /// <returns></returns>
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
                int chance = 100 / DifficultyManager.GetCurrentDisruptorSpawnChanceValue();


                if (new System.Random().Next(0, chance) != 0) { yield return new WaitForSeconds(DifficultyManager.DISRUPTOR_SPAWN_DELAY); continue; }

                GameObject b = Instantiate(prefab, transform.position, prefab.transform.rotation);



                b.transform.GetChild(0).GetComponent<MoveDisruptorCharge>().SetTargets(1);
                b.transform.GetChild(1).GetComponent<MoveDisruptorCharge>().SetTargets(2);

                yield return new WaitForSeconds(DifficultyManager.DISRUPTOR_SPAWN_DELAY);

            }
        }

    }
}
