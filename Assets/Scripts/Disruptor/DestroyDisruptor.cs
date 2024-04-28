using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DestroyDisruptor : MonoBehaviour
{
    


    Dictionary<int, int> index_order_dict;



    [SerializeField] float color_change_delay;

    Material color_mat;
    void Start()
    {

        color_mat = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        index_order_dict = new Dictionary<int, int>() { { 1, 8 }, { 2, 7 }, { 3, 6 }, { 4, 5 }, { 5, 4 }, { 6, 3 }, { 7, 2 }, { 8, 9 }, { 9, 1 }, { 10, 0 } };
    }

    
    void Update()
    {

    }



    void DestroyObj()
    {
       
        Destroy(gameObject);
    }


    /// <summary>
    /// <para>Plays DISRUPTOR_DESTROYED sound.</para>
    /// <para>Destroys AfterEMP and Collider, enagages in RotateDisruptor and destroys it.</para>
    /// <para>Destroys the charges</para>
    /// <para>Cancels MovingUp, Destroys StartEndMovement, DisruptorColorChange and Renderer.</para>
    /// <para>Raises ScoreChange.</para>
    /// <para>Plays the destruction particle system, waits for it to finish and destroys the gameObject.</para>
    /// </summary>
    public void Engage()
    {

        AudioManager.PlayActivitySound(AudioManager.ActivityType.DISRUPTOR_DESTROYED);

        Vector3 target = Camera.main.transform.position;



        Destroy(GetComponent<AfterEMP>());
        Destroy(GetComponent<Collider>());



        TryGetComponent(out RotateDisruptor rd);

        if (rd != null)
        {
            GetComponent<RotateDisruptor>().EngageRotation(target);
            Destroy(GetComponent<RotateDisruptor>());

        }



        for(int i = 0; i< transform.childCount; i++) 
        {
            if (transform.GetChild(i).GetComponent<MoveDisruptorCharge>() != null) { Destroy(transform.GetChild(i).gameObject); }
        
        }


        GetComponent<DisruptorStartEndMovement>().CancelMovingUp();
        Destroy(GetComponent<DisruptorMovement>());
        Destroy(GetComponent<DisruptorColorChange>());
        Destroy(GetComponent<Renderer>());


        UICommunication.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ValidateScoreReward());


        var system = transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>();
        var emission = system.emission;
        emission.enabled = true;

        system.Play();



        Invoke(nameof(DestroyObj), system.main.duration);

    }





    /// <summary>
    /// Fills the renderer materials with color_mat.
    /// </summary>
    void ColorChange()
    {

        Material[] mats = new Material[GetComponent<Renderer>().materials.Length];


        Array.Fill(mats, color_mat);


        GetComponent<Renderer>().materials = mats;

    }


}
