using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnBomb : MonoBehaviour
{
    

    ParticleSystem spawner_ps;

    [SerializeField] float min_size, max_size, scale_up_increment;

    [SerializeField] float destoy_delay;

    [SerializeField] float spawn_x_offset;

    Material[] mats1;
    Material[] mats2;

















    [SerializeField] GameObject bomb_normal_prefab, bomb_cluster_prefab;


    bool CanSpawn = true;


    public event Action<Material> OnBombSpawnStart;
    public event Action OnBombSpawnEnd;

    

    public void Start()
    {
        mats1 = MaterialHolder.Instance().COLOR_SET_1();
        mats2 = MaterialHolder.Instance().COLOR_SET_2();
    }



    private void OnDestroy()
    {
        BombSpawnerGrid.OnClusterEventStart -= ClusterEventStart;
        BombSpawnerGrid.OnClusterEventSpawn -= ClusterEventSpawn;
        BombSpawnerGrid.OnClusterEventEnd -= ClusterEventEnd;
    }




    void ClusterEventStart() => CanSpawn = false; 
    void ClusterEventEnd() => CanSpawn = true;
    void ClusterEventSpawn(string tag, Material mat) => ConstructBomb(bomb_cluster_prefab,tag, mat, min_size/5f);

    private void Awake()
    {


        BombSpawnerGrid.OnClusterEventStart += ClusterEventStart;
        BombSpawnerGrid.OnClusterEventSpawn += ClusterEventSpawn;
        BombSpawnerGrid.OnClusterEventEnd += ClusterEventEnd;



        spawner_ps = transform.parent.GetChild(0).GetComponent<ParticleSystem>();


        InvokeRepeating(nameof(SpawnRegular), DifficultyManager.BOMB_SPAWN_DELAY * (float)new System.Random().NextDouble(), DifficultyManager.BOMB_SPAWN_DELAY);


        HelperSpawnerManager.OnBlackHoleSpawn += () => CanSpawn = false;
        HelperSpawnerManager.OnBlackHoleDestroy += () => CanSpawn = true;




        OnBombSpawnStart += ChangeTileToColor;
        OnBombSpawnStart += StartTileEmission;
        OnBombSpawnEnd += ChangeTileToOff;
        OnBombSpawnEnd += EndTileEmission;





    }


    const int TILE_COLOR_INDEX = 2;
    const int TILE_SECONDARY_INDEX = 1;
    void ChangeTileToColor(Material m)
    {

        GameObject tile = transform.parent.gameObject;
        Material[] mats = tile.GetComponent<Renderer>().materials;
        mats[TILE_COLOR_INDEX] = m;
        tile.GetComponent<Renderer>().materials = mats;

    }



    void ChangeTileToOff()
    {
        GameObject tile = transform.parent.gameObject;
        Material[] mats = tile.GetComponent<Renderer>().materials;
        mats[TILE_COLOR_INDEX] = mats[TILE_SECONDARY_INDEX];
        tile.GetComponent<Renderer>().materials = mats;

    }

    void StartTileEmission(Material m)
    {

        spawner_ps.GetComponent<ParticleSystemRenderer>().material = m;
        var emission = spawner_ps.emission;
        emission.enabled = true;
    }

    void EndTileEmission()
    {

        var emission = spawner_ps.emission;
        emission.enabled = false;
    }




    void SpawnRegular()
    {


        if (!CanSpawn) return;

        string tag = Random.Range(0, 2) == 1 ? Tags.LASER_TARGET_1 : Tags.LASER_TARGET_2;


        Material colorMat = tag switch
        {


            Tags.LASER_TARGET_1 => mats1[Random.Range(0, 4)],

            Tags.LASER_TARGET_2 => mats2[Random.Range(0, 4)],
            _ => mats1[0]
        };


        float size = Random.Range(min_size, max_size);


        ConstructBomb(bomb_normal_prefab, tag, colorMat, size);


        /*
        OnBombSpawnStart?.Invoke(colorMat);


        Vector3 spawn = transform.position;
        GameObject bomb = Instantiate(prefab, spawn, transform.rotation);
        bomb.transform.localScale = new(0.1f, 0.1f, 0.1f);


        bomb.tag = tag;
        


        StartCoroutine(ScaleUp(bomb, size));


        ParticleSystem destroy_ps = bomb.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem dissolve_ps = bomb.transform.GetChild(1).GetComponent<ParticleSystem>();



        var destroy_main = destroy_ps.main;
        var dissolve_main = dissolve_ps.main;



        destroy_main.startSize = size;
        dissolve_main.startSize = size / 2;





        destroy_ps.GetComponent<ParticleSystemRenderer>().material = colorMat;
        dissolve_ps.GetComponent<ParticleSystemRenderer>().material = colorMat;


        bomb.GetComponent<BombColorChange>().Init(colorMat);

        */




    }



    void ConstructBomb(GameObject prefab,string tag, Material mat, float size)
    {
        OnBombSpawnStart?.Invoke(mat);


        Vector3 spawn = transform.position;
        GameObject bomb = Instantiate(prefab, spawn, transform.rotation);
        bomb.transform.localScale = new(0.1f, 0.1f, 0.1f);


        bomb.tag = tag;
      //  float size = Random.Range(min_size, max_size);


        StartCoroutine(ScaleUp(bomb, size));


        ParticleSystem destroy_ps = bomb.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem dissolve_ps = bomb.transform.GetChild(1).GetComponent<ParticleSystem>();



        var destroy_main = destroy_ps.main;
        var dissolve_main = dissolve_ps.main;



        destroy_main.startSize = size;
        dissolve_main.startSize = size / 2;





        destroy_ps.GetComponent<ParticleSystemRenderer>().material = mat;
        dissolve_ps.GetComponent<ParticleSystemRenderer>().material = mat;


        bomb.GetComponent<BombColorChange>().Init(mat);


    }


    Vector3 target_scale;


    IEnumerator ScaleUp(GameObject bomb, float target_size)
    {

        target_scale = target_size * Vector3.one;

        if (bomb == null) { OnBombSpawnEnd?.Invoke(); }



        float duration = 0.5f;
        float lerp = 0f;




        while (lerp < duration)
        {
            try
            {
                lerp += Time.deltaTime;
                bomb.transform.localScale = Vector3.Lerp(Vector3.zero, target_scale, lerp / duration) ; //new Vector3(bomb.transform.localScale.x + scale_up_increment, bomb.transform.localScale.y + scale_up_increment, bomb.transform.localScale.z + scale_up_increment);


 

            }
            catch (Exception)
            {

                OnBombSpawnEnd?.Invoke();

                yield break;
            }

            yield return null;
        }




        if (bomb == null || bomb.GetComponent<BombFall>() == null) { OnBombSpawnEnd?.Invoke(); yield break; }
        bomb.GetComponent<BombFall>().SetScaledUp(); OnBombSpawnEnd?.Invoke();


    }
}








