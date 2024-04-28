using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{
    
    
    
   
   public Vector2 turn;
   public float sensitivity;
   public Vector3 deltaMove;
   public float speed = 1;
   public GameObject mover;
   public float[] covY;
   public float[] covX;



    void Start(){

        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Gets the turn x and y values, clamps y, sets the localRotation to the values.
    /// </summary>
    private void Update()
    {


        
        turn.x += Input.GetAxis("Mouse X") * sensitivity;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity;

       turn.y =Mathf.Clamp(turn.y,covY[0],covY[1]);
       
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);


    }
}
