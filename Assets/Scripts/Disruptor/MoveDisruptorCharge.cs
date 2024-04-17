using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDisruptorCharge : MonoBehaviour
{
    
    Transform target;
    [SerializeField] float speed;

   


    int ID;

 

    void Start()
    {

    }

    
    void Update()
    {


    }

    public void SetTargets(int i)
    {
        switch (i)
        {
            case 1:
                ID = 1;
                target = GameObject.FindGameObjectWithTag(Tags.TURRET_CONTROL_1).transform;


                break;
            case 2:
                ID = 2;
                target = GameObject.FindGameObjectWithTag(Tags.TURRET_CONTROL_2).transform;


                break;
        }
    }

    public void StartMovement()
    {
        StartCoroutine(Move());
    }


    IEnumerator Move()
    {

        while (Vector3.Distance(transform.position, target.position) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            yield return null;
        }

        // turret_control.GetComponent<LaserControlDisableManager>().DisableFor(DifficultyManager.DISRUPTOR_DISABLE_TIME);



        float disable_duration = DifficultyManager.DISRUPTORxDISABLE_TIME_DIFFICULTY_DICT[DifficultyManager.DIFFICULTY] * 1000;
        switch (ID) 
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.DisableControlFor(disable_duration);
                break;
            case 2:
                LaserTurretCommunicationChannels.Channel2.DisableControlFor(disable_duration);
                break;
        }
      

        Destroy(gameObject);
    }




}
