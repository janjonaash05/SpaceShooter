using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarFall : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 target;

    public delegate void StarFallEventHandler(Material color);
    public static event StarFallEventHandler OnStarFallen;

    [SerializeField] float speed;


    Coroutine fall_cr;

    void Start()
    {
        target = Vector3.zero + GameObject.FindGameObjectWithTag(Tags.SUPERNOVA).transform.localPosition;
        GetComponent<StarChargeUp>().OnChargeUp += Fall;


        HelperSpawnerManager.OnEMPSpawn += OnEMP;
    }



    void OnEMP()
    {
        if(fall_cr != null)
        StopCoroutine(fall_cr);
    }

    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMP;
    }


    IEnumerator fall()
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.1)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);


            yield return null;

        }
        OnStarFallen?.Invoke(GetComponent<Renderer>().materials[1]);
        Destroy(gameObject);



    }




    public void Fall()
    {
        

       
       fall_cr = StartCoroutine(fall());



    }

}
