using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarFall : MonoBehaviour, IEMPDisruptable
{


    Vector3 target;

    public delegate void StarFallEventHandler(Material color);
    public static event StarFallEventHandler OnStarFallen;

    [SerializeField] float speed;


    Coroutine fall_cr;

    void Start()
    {
        target = Vector3.zero + GameObject.FindGameObjectWithTag(Tags.SUPERNOVA).transform.localPosition;
        GetComponent<StarChargeUp>().OnChargeUp += Fall;
        

    }



    public void OnEMP()
    {
        if (fall_cr != null)
            StopCoroutine(fall_cr);
    }



    /// <summary>
    /// Moves the transform towards the target, when it reaches, invokes OnStarFallen with its color material and destroys this gameObject,
    /// </summary>
    /// <returns></returns>
    IEnumerator FallProcess()
    {

        while (Vector3.Distance(transform.localPosition, target) > 0.1)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);


            yield return null;

        }
        OnStarFallen?.Invoke(GetComponent<Renderer>().materials[1]);
        Destroy(gameObject);



    }



    /// <summary>
    /// Plays the STAR_FALL sound, assigns the start of FallProcess as the fall_cr coroutine.
    /// </summary>
    public void Fall()
    {


        AudioManager.PlayActivitySound(AudioManager.ActivityType.STAR_FALL);
        fall_cr = StartCoroutine(FallProcess());



    }

}
