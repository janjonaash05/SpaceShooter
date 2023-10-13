using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorStartEndMovement : MonoBehaviour
{
    // Start is called before the first frame update


    public  float down_speed, up_speed;
    public float min_y, max_y;

    public event EventHandler OnMoveDownFinish, OnMoveUpFinish;




    bool cancelMoveUpExternally;
    void Start()
    {
        cancelMoveUpExternally = false;
        OnMoveDownFinish += Kill;
    }


    void Kill(object sender, EventArgs e) {

        Destroy(gameObject);
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveUp() {

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

            OnMoveDownFinish?.Invoke(this, EventArgs.Empty);
            
       


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
            OnMoveUpFinish?.Invoke(this, EventArgs.Empty);

        }
    }


   public void CancelMovingUp()
    {
        cancelMoveUpExternally = true;
        
    
    
    }
}
