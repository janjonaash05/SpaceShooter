using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DestroyDisruptor : MonoBehaviour
{
    // Start is called before the first frame update


    Dictionary<int, int> index_order_dict;



    [SerializeField] float color_change_delay;
    [SerializeField] Material white;

    void Start()
    {
        index_order_dict = new Dictionary<int, int>() { { 1, 8 }, { 2, 7 }, { 3, 6 }, { 4, 5 }, { 5, 4 }, { 6, 3 }, { 7, 2 }, { 8, 9 }, { 9, 1 }, { 10, 0 } };
    }

    // Update is called once per frame
    void Update()
    {

    }



    void DisableEmission(ref bool emission_enabled)
    {
        emission_enabled = false;
    }

    public void Engage()
    {



        Vector3 target = Camera.main.transform.position;

        GetComponent<RotateDisruptor>().EngageRotation(target);
        Destroy(GetComponent<RotateDisruptor>());

        ///  ScoreCounter.Increase(GetComponent<IScoreEnumerable>().ScoreReward());
        int a;


        UICommunicationSO.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ScoreReward());



        GetComponent<IScoreEnumerable>().DisabledRewards = true;

        ColorChange();
        GetComponent<DisruptorStartEndMovement>().CancelMovingUp();



        Destroy(GetComponent<Renderer>());
        var system = transform.GetChild(3).GetComponent<ParticleSystem>();
        var emission = system.emission;
        emission.enabled = true;

        system.Play();
        Invoke(nameof(DisableEmission), system.main.duration);





        Destroy(GetComponent<DisruptorMovement>());
        Destroy(GetComponent<DisruptorColorChange>());




    }






    void ColorChange()
    {

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];


        Array.Fill(mats, white);


        GetComponent<Renderer>().materials = mats;

    }


}
