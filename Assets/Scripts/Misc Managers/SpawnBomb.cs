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







    NameMaterialPair[] pairs1, pairs2;









    [SerializeField] GameObject bomb_normal_prefab, bomb_cluster_prefab;


    bool CanSpawn = true;


    public event Action<Material> OnBombSpawnStart;
    public event Action OnBombSpawnEnd;

    

    public void Start()
    {
        mats1 = MaterialHolder.Instance().COLOR_SET_1();
        mats2 = MaterialHolder.Instance().COLOR_SET_2();

        pairs1 = MaterialHolder.Instance().NAMED_COLOR_SET_1();
        pairs2 = MaterialHolder.Instance().NAMED_COLOR_SET_2();
    }



    private void OnDestroy()
    {
        BombSpawnerGrid.OnClusterEventStart -= ClusterEventStart;
        BombSpawnerGrid.OnClusterEventSpawn -= ClusterEventSpawn;
        BombSpawnerGrid.OnClusterEventEnd -= ClusterEventEnd;
    }




    void ClusterEventStart() => CanSpawn = false; 
    void ClusterEventEnd() => CanSpawn = true;
    void ClusterEventSpawn(string tag, NameMaterialPair pair) => ConstructBomb(bomb_cluster_prefab,tag, pair, min_size/5f);

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

    /// <summary>
    /// Sets the gameObject's parent's materials to arg m at the TILE_COLOR_INDEX.
    /// </summary>
    /// <param name="m"></param>
    void ChangeTileToColor(Material m)
    {

        GameObject tile = transform.parent.gameObject;
        Material[] mats = tile.GetComponent<Renderer>().materials;
        mats[TILE_COLOR_INDEX] = m;
        tile.GetComponent<Renderer>().materials = mats;

    }


    /// <summary>
    /// Sets the gameObject's parent's materials to arg m at the TILE_SECONDARY_INDEX.
    /// </summary>
    /// <param name="m"></param>
    void ChangeTileToOff()
    {
        GameObject tile = transform.parent.gameObject;
        Material[] mats = tile.GetComponent<Renderer>().materials;
        mats[TILE_COLOR_INDEX] = mats[TILE_SECONDARY_INDEX];
        tile.GetComponent<Renderer>().materials = mats;

    }

    /// <summary>
    /// Sets the spawner particle system renderer material to arg m, enables its emission.
    /// </summary>
    /// <param name="m"></param>
    void StartTileEmission(Material m)
    {

        spawner_ps.GetComponent<ParticleSystemRenderer>().material = m;
        var emission = spawner_ps.emission;
        emission.enabled = true;
    }
    /// <summary>
    /// Disables the spawner particle system emission.
    /// </summary>
    /// <param name="m"></param>
    void EndTileEmission()
    {

        var emission = spawner_ps.emission;
        emission.enabled = false;
    }



    /// <summary>
    /// <para>If unable to spawn, returns.</para>
    /// <para>Generates a random tag from 2 options.</para>
    /// <para>Based on the tag, chooses a random NameMaterialPair from the appropriate array.</para>
    /// <para>Gets a random size based on an interval.</para>
    /// <para>Calls ConstructBomb() with the normal bomb prefab, the attained tag, pair and size.</para>
    /// </summary>
    void SpawnRegular()
    {


        if (!CanSpawn) return;

        string tag = Random.Range(0, 2) == 1 ? Tags.LASER_TARGET_1 : Tags.LASER_TARGET_2;


        NameMaterialPair pair = tag switch
        {


            Tags.LASER_TARGET_1 => pairs1[Random.Range(0, 4)],

            Tags.LASER_TARGET_2 => pairs2[Random.Range(0, 4)],
            _ => null
        } ;


        float size = Random.Range(min_size, max_size);


        ConstructBomb(bomb_normal_prefab, tag, pair, size);




    }



    /// <summary>
    /// Gets the material from the arg pair, invokes OnBombSpawnStart with it.
    /// <para>Instantiates the arg prefab on this gameObjects position and rotation, sets its localScale to a set small scale.</para>
    /// <para>Assigns the arg tag to the created bomb.</para>
    /// <para>Starts the ScaleUp coroutine with the bomb and arg scale.</para>
    /// <para>Gets the bomb's particle systems and assigns scale relative to the arg scale, and renderer material as the material from the arg pair.</para>
    /// <para>Calls Init on the BombColorChange component with the arg pair.</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="tag"></param>
    /// <param name="pair"></param>
    /// <param name="size"></param>
    void ConstructBomb(GameObject prefab,string tag, NameMaterialPair pair, float size)
    {

        Material mat = pair.Material;
        OnBombSpawnStart?.Invoke(mat);


        
        GameObject bomb = Instantiate(prefab, transform.position, transform.rotation);
        bomb.transform.localScale = new(0.1f, 0.1f, 0.1f);


        bomb.tag = tag;

        StartCoroutine(ScaleUp(bomb, size));


        ParticleSystem destroy_ps = bomb.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem dissolve_ps = bomb.transform.GetChild(1).GetComponent<ParticleSystem>();



        var destroy_main = destroy_ps.main;
        var dissolve_main = dissolve_ps.main;



        destroy_main.startSize = size;
        dissolve_main.startSize = size / 2;





        destroy_ps.GetComponent<ParticleSystemRenderer>().material = mat;
        dissolve_ps.GetComponent<ParticleSystemRenderer>().material = mat;


        bomb.GetComponent<BombColorChange>().Init(pair);


    }


    Vector3 target_scale;

    /// <summary>
    /// <para>Calculates the target scale.</para>
    /// <para>if the bomb is null, invokes OnBombSpawnEnd and returns. </para>
    /// <para>LERPs the bomb's localScale from zero to target over a druation, if anything fails invokes OnBombSpawnEnd and returns. </para>
    /// <para>If the bomb is null or its BombFall component is null, invokes OnBombSpawnEnd and returns.</para>
    /// <para>Else, sets it scaled up, invokes OnBombSpawnEnd.</para>
    /// </summary>
    /// <param name="bomb"></param>
    /// <param name="target_size"></param>
    /// <returns></returns>
    IEnumerator ScaleUp(GameObject bomb, float target_size)
    {

        target_scale = target_size * Vector3.one;
        if (bomb == null) { OnBombSpawnEnd?.Invoke(); yield break; }

        float duration = 0.5f;
        float lerp = 0f;




        while (lerp < duration)
        {
            try
            {
                lerp += Time.deltaTime;
                bomb.transform.localScale = Vector3.Lerp(Vector3.zero, target_scale, lerp / duration) ; //new Vector3(bomb.transform.localScale.x + scale_up_increment, bomb.transform.localScale.y + scale_up_increment, bomb.transform.localScale.z + scale_up_increment);
            }
            catch
            {
                OnBombSpawnEnd?.Invoke();
                yield break;
            }

            yield return null;
        }




        if (bomb == null || bomb.GetComponent<BombFall>() == null) { OnBombSpawnEnd?.Invoke(); yield break; }
        bomb.GetComponent<BombFall>().SetScaledUp(); 
        OnBombSpawnEnd?.Invoke();


    }
}








