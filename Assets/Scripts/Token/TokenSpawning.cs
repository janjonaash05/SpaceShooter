using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokenSpawning : MonoBehaviour
{

    public static Transform[] transporter_collider_transforms { get; private set; }
    public static Transform[] transporter_transforms { get; private set; }
    public static Transform center_transform { get; private set; }
    public static Transform harpoon_station_transform { get; private set; }

    [SerializeField] GameObject friendly_prefab, enemy_prefab;

    bool can_spawn;



    Material friendly_mat, enemy_mat;

    void Start()
    {


        friendly_mat = friendly_prefab.GetComponent<Renderer>().sharedMaterials[^1];

        enemy_mat = enemy_prefab.GetComponent<Renderer>().sharedMaterials[^1];

        can_spawn = true;



        center_transform = transform;
        transporter_collider_transforms = GameObject.FindGameObjectsWithTag(Tags.TOKEN_TRANSPORT_COLLIDER).Select(x => x.transform).ToArray();
        transporter_transforms = GameObject.FindGameObjectsWithTag(Tags.TOKEN_TRANSPORT).Select(x => x.transform).ToArray();
        harpoon_station_transform = GameObject.FindWithTag(Tags.HARPOON_STATION).transform;


        StartCoroutine(Spawning());




    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Spawning()
    {
        while (true)
        {




            float waitTime = UnityEngine.Random.Range(2, 10);
            yield return new WaitForSeconds(waitTime);
            if (GameObject.FindGameObjectWithTag(Tags.TOKEN) == null)
              yield return  StartCoroutine(Spawn());
        }





    }





    IEnumerator Spawn()
    {




        (GameObject prefab, Material mat) = UnityEngine.Random.Range(0, 2) switch { 0 => (friendly_prefab, friendly_mat), _ => (enemy_prefab, enemy_mat) };


        int index = UnityEngine.Random.Range(0, 4);


        Transform transporter_collider_transform = transporter_collider_transforms[index];
        Transform transporter_transform = transporter_transforms[index];
        yield return StartCoroutine(transporter_transform.gameObject.GetComponent<TokenTransportColorChange>().Flash(mat));

        var obj = Instantiate(prefab);
        obj.transform.position = transporter_collider_transform.position;



    }
}
