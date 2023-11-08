using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] float min_size, max_size, scale_up_increment;

    [SerializeField] float destoy_delay;

    [SerializeField] float spawn_x_offset;

    [SerializeField] Material[] Mats1;
    [SerializeField] Material[] Mats2;


    public Material backgroundMat;
    public GameObject prefab;
    void Start()
    {


        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame




    public IEnumerator SpawnTimer()
    {





        while (true)
        {

            
            Spawn(out GameObject bomb);
            Destroy(bomb, destoy_delay);
            yield return new WaitForSeconds(DifficultyManager.BOMB_SPAWN_DELAY);
        }


    }
    void Spawn(out GameObject bomb)
    {

        Vector3 spawn = new(Random.Range(transform.position.x - spawn_x_offset, transform.position.x + spawn_x_offset), transform.position.y, Random.Range(transform.position.z - 20, transform.position.z + 20));
        bomb = Instantiate(prefab, spawn, transform.rotation);
        bomb.transform.localScale = new(0.1f, 0.1f, 0.1f);




        float size = Random.Range(min_size, max_size);


        StartCoroutine(ScaleUp(bomb, size));


        string tag = Random.Range(0, 2) == 1 ? Tags.LASER_TARGET_1 : Tags.LASER_TARGET_2;
        bomb.tag = tag;

        Material colorMat = tag switch
        {


            Tags.LASER_TARGET_1 => Mats1[Random.Range(0, 4)],

            Tags.LASER_TARGET_2 => Mats2[Random.Range(0, 4)],
            _ => Mats1[0]
        };






        ParticleSystem destroy_ps = bomb.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem dissolve_ps = bomb.transform.GetChild(1).GetComponent<ParticleSystem>();



        var destroy_main = destroy_ps.main;
        var dissolve_main = dissolve_ps.main;



        destroy_main.startSize = size;
        dissolve_main.startSize = size;





        destroy_ps.GetComponent<ParticleSystemRenderer>().material = colorMat;
        dissolve_ps.GetComponent<ParticleSystemRenderer>().material = colorMat;
        bomb.GetComponent<BombColorChange>().InitialColorUp(colorMat);
       





    }


    IEnumerator ScaleUp(GameObject bomb, float target_size)
    {
        while (bomb.transform.localScale.x < target_size)
        {

            bomb.transform.localScale = new Vector3(bomb.transform.localScale.x + scale_up_increment, bomb.transform.localScale.y + scale_up_increment, bomb.transform.localScale.z + scale_up_increment);
            yield return null;
        }


    }
}








