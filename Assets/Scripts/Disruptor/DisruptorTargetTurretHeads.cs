using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;


public class DisruptorTargetTurretHeads : MonoBehaviour, IEMPDisruptable
{

    GameObject charge1, charge2;

    Transform target_left;
    Transform cam;

    RotateDisruptor rotateDisruptor;

    void Start()
    {
        charge1 = transform.GetChild(0).gameObject;
        charge2 = transform.GetChild(1).gameObject;
        cam = Camera.main.transform;
        target_left = GameObject.FindGameObjectWithTag(Tags.TURRET_CONTROL_1).transform;
        rotateDisruptor = GetComponent<RotateDisruptor>();



    }




   public void OnEMP()
    {
        StopAllCoroutines();
    
    }


    public void InitiateRotations()
    {
        StartCoroutine(Rotations());
    }


    /// <summary>
    /// Rotates towards the target's middle, plays DISRUPTOR_SHOOT sound, moves both charges, rotates back towards the player and moves down.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Rotations()
    {



        Vector3 target_middle = target_left.position;
        target_middle.x = 0;
        yield return StartCoroutine(rotateDisruptor.RotateTowards(target_middle));

        AudioManager.PlayActivitySound(AudioManager.ActivityType.DISRUPTOR_SHOOT);


        charge1.GetComponent<MoveDisruptorCharge>().StartMovement();
        charge2.GetComponent<MoveDisruptorCharge>().StartMovement();


        Vector3 target_player = GameObject.FindWithTag(Tags.PLAYER).transform.position;
        target_player.y = transform.position.y;

        yield return StartCoroutine(rotateDisruptor.RotateTowards(target_player));



        GetComponent<DisruptorStartEndMovement>().MoveDown();



    }

}
