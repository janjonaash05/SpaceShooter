using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorStartEndMovement : MonoBehaviour, IEMPDisruptable
{
    


    public float down_speed, up_speed;
    public float min_y, max_y;

    public event Action OnMoveDownFinish, OnMoveUpFinish;



    public void OnEMP()
    {

        StopAllCoroutines();
    }


    bool cancelMoveUpExternally;

   

    void Start()
    {
        cancelMoveUpExternally = false;
        OnMoveDownFinish += Kill;
    }

    private void Awake()
    {
        AudioManager.PlayActivitySound(AudioManager.ActivityType.DISRUPTOR_SPAWN);
    }


    void Kill()
    {

        Destroy(gameObject);

    }

    
    void Update()
    {

    }

    public void MoveUp()
    {

        StartCoroutine(MovingUp());
    }


    public void MoveDown()
    {

        StartCoroutine(MovingDown());
    }




    IEnumerator MovingDown()
    {

        while (transform.position.y > min_y)
        {

            transform.Translate(new Vector3(0, -down_speed * Time.deltaTime, 0), Space.World);

            yield return null;


        }

        OnMoveDownFinish?.Invoke();




    }


    IEnumerator MovingUp()
    {
        while (transform.position.y < max_y)
        {
            if (cancelMoveUpExternally)
            {
                break;

            }

            transform.Translate(new Vector3(0, up_speed * Time.deltaTime, 0), Space.World);


            yield return null;


        }
        if (!cancelMoveUpExternally)
        {
            OnMoveUpFinish?.Invoke();

        }
    }


    public void CancelMovingUp()
    {
        cancelMoveUpExternally = true;



    }
}
