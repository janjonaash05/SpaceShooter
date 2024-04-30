using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelperSpawnerManager : MonoBehaviour
{



    public enum HelperType { EMP, BLACK_HOLE }

    public static event Action OnBlackHoleSpawn, OnBlackHoleDestroy;
    public static event Action OnEMPSpawn, OnEMPDestroy;



    [SerializeField] GameObject black_hole_prefab, emp_prefab;
    public const int LIFETIME = 4;


    Dictionary<HelperType, (Action SpawnInvoke, GameObject prefab, Action DestroyInvoke)> type_management_dict;


    public Vector3 BlackHolePosition { get; private set; }



    /// <summary>
    /// <para>Gets the tuple from the type management dictionary.</para>
    /// <para>Calls SpawnInvoke() on the tuple, instantiates the tuples prefab, waits a certain amount, Destroys the prefab and calls DestroyInvoke() on the tuple. </para>
    /// </summary>
    /// <param name="type"></param>
    public void SpawnHelper(HelperType type)
    {


        IEnumerator spawnHelper()
        {
            var set = type_management_dict[type];

            set.SpawnInvoke();

            var prefab = Instantiate(set.prefab);
            yield return new WaitForSeconds(LIFETIME);

            Destroy(prefab);

            set.DestroyInvoke();




        }


        StartCoroutine(spawnHelper());
    }




    void Start()
    {
        BlackHolePosition = black_hole_prefab.transform.position;


        type_management_dict = new()
        {
            {HelperType.BLACK_HOLE, (()=>{OnBlackHoleSpawn?.Invoke(); },black_hole_prefab,() => {OnBlackHoleDestroy?.Invoke(); }    )  },
            {HelperType.EMP, (()=>{OnEMPSpawn?.Invoke(); },emp_prefab,() => {OnEMPDestroy?.Invoke(); }    )  },

        };


    }



    public static HelperSpawnerManager Instance()
    {
        return GameObject.FindWithTag(Tags.HELPER_SPAWNER_MANAGER).GetComponent<HelperSpawnerManager>();


    }
}
