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



    void DestroyObj()
    {

        Destroy(gameObject);
    }

    public void Engage()
    {



        Vector3 target = Camera.main.transform.position;




        TryGetComponent(out RotateDisruptor rd);

        if(rd!= null )
        {
            GetComponent<RotateDisruptor>().EngageRotation(target);
            Destroy(GetComponent<RotateDisruptor>());

        }


        






        if (transform.GetChild(0) != null) { Destroy(transform.GetChild(0).gameObject); }
        if (transform.GetChild(1) != null) { Destroy(transform.GetChild(1).gameObject); }



        Destroy(transform.GetChild(transform.childCount-2).gameObject);



   



        GetComponent<DisruptorStartEndMovement>().CancelMovingUp();
        Destroy(GetComponent<DisruptorMovement>());
        Destroy(GetComponent<DisruptorColorChange>());



        Destroy(GetComponent<Renderer>());


        UICommunicationSO.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ScoreReward());






        GetComponent<IScoreEnumerable>().DisabledRewards = true;

      


/*
       
*/



        var system = transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>();
        var emission = system.emission;
        emission.enabled = true;

        system.Play();
        Invoke(nameof(DestroyObj), system.main.duration);

        



     

        

    }






    void ColorChange()
    {

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];


        Array.Fill(mats, white);


        GetComponent<Renderer>().materials = mats;

    }


}
