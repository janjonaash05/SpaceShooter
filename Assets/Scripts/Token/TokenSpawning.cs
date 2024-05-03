using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokenSpawning : MonoBehaviour
{

    public static Transform[] TRANSPORTER_COLLIDER_TRANSFORMS { get; private set; }
    public static Transform[] TRANSPORTER_TRANSFORMS { get; private set; }
    public static Transform CENTER_TRANSFORM { get; private set; }
    public static Transform HARPOON_STATION_TRANSFORM { get; private set; }

    [SerializeField] GameObject friendly_prefab, enemy_prefab;

    bool perma_stopped = false;



    Material friendly_mat, enemy_mat;




    void OnLaserShotPlayerDeath() => perma_stopped = true;


    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= OnLaserShotPlayerDeath;
    }

    void Start()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath += OnLaserShotPlayerDeath;

        friendly_mat = friendly_prefab.GetComponent<Renderer>().sharedMaterials[^1];

        enemy_mat = enemy_prefab.GetComponent<Renderer>().sharedMaterials[^1];




        CENTER_TRANSFORM = transform;
        TRANSPORTER_COLLIDER_TRANSFORMS = GameObject.FindGameObjectsWithTag(Tags.TOKEN_TRANSPORT_COLLIDER).Select(x => x.transform).ToArray();
        TRANSPORTER_TRANSFORMS = GameObject.FindGameObjectsWithTag(Tags.TOKEN_TRANSPORT).Select(x => x.transform).ToArray();
        HARPOON_STATION_TRANSFORM = GameObject.FindWithTag(Tags.HARPOON_STATION).transform;


        StartCoroutine(Spawning());




    }



    /// <summary>
    /// In an endless loop:
    /// <para>- If perma_stopped, breaks and returns.</para>
    /// <para>- Calculates a random wait time and waits it.</para>
    /// <para>- If there are no TOKEN gameObjects, yields the Spawn coroutine.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawning()
    {
        while (true)
        {
            if (perma_stopped)
            {
                yield break;
            }


            float waitTime = UnityEngine.Random.Range(2, 10);
            yield return new WaitForSeconds(waitTime);
            if (GameObject.FindGameObjectWithTag(Tags.TOKEN) == null)
                yield return StartCoroutine(Spawn());
        }





    }







    /// <summary>
    /// If perma_stopped, returns.
    /// <para>Gets the prefab and material based on a 50/50 chance (Enemy/Friendly).</para>
    /// <para>Gets a random index, based on it assigns the transporter_transform and transporter_collider_transform.  </para>
    /// <para>Yields the transporter_transform's Flash coroutine with the material.</para>
    /// <para>Instantiates the prefab, and sets its position as the transporter_collider_transform position.</para>
    ///
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        if (perma_stopped) yield break;



        (GameObject prefab, Material mat) = UnityEngine.Random.Range(0, 2) switch
        {
            0 => (friendly_prefab, friendly_mat),
            1 => (enemy_prefab, enemy_mat)
        };

        int index = UnityEngine.Random.Range(0, 4);


        Transform transporter_collider_transform = TRANSPORTER_COLLIDER_TRANSFORMS[index];
        Transform transporter_transform = TRANSPORTER_TRANSFORMS[index];
        yield return StartCoroutine(transporter_transform.gameObject.GetComponent<TokenTransportColorChange>().Flash(mat));

        var obj = Instantiate(prefab);
        obj.transform.position = transporter_collider_transform.position;



    }
}
