using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisruptorCharge : MonoBehaviour
{

    Transform target;
    [SerializeField] float speed;




    int ID;

    /// <summary>
    /// Assigns the ID and target transform based on the ID.
    /// </summary>
    /// <param name="i"></param>
    public void SetTargets(int i)
    {


        ID = i;
        target = GameObject.FindGameObjectWithTag(ID == 1 ? Tags.TURRET_CONTROL_1 : Tags.TURRET_CONTROL_2).transform;


    }

    public void StartMovement()
    {
        StartCoroutine(Move());
    }



    const float EDGE_DSITANCE = 0.6f;

    /// <summary>
    /// <para>Moves towards the target at a set speed.</para>
    /// <para>After reaching the edge distance, calculates the disable duration and calls DisableControlFor on one of the LaserTurretChannels.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        Debug.Log("targetpos "+target);
        while ((Vector3.Distance(transform.position, target.position)) > EDGE_DSITANCE)
        {

            Debug.Log(Vector3.Distance(transform.position, target.position));


            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            yield return null;
        }


        Debug.Log("Disablin");

        float disable_duration = DifficultyManager.DISRUPTORxDISABLE_TIME_DIFFICULTY_DICT[DifficultyManager.DIFFICULTY] * 1000;

        var channel = LaserTurretCommunicationChannels.GetChannelByID(ID);
        channel.DisableControlFor(disable_duration);

        Destroy(gameObject);
    }




}
