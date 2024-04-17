using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MouseReactivity : MonoBehaviour
{
    

    void Start()
    {

    }

    
    void Update()
    {

        bool valid_click = false;
        if (Input.GetButtonDown("Fire1"))
        {





            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {

                Debug.Log(hit.transform.tag);

                valid_click = PlayerInputCommunication.Raise_RaycastClick(hit);




            }


            if (!valid_click)
            {
                PlayerInputCommunication.Raise_MouseDown();


            }
        }


        if (Input.GetButtonUp("Fire1"))
        {

            PlayerInputCommunication.Raise_MouseUp();
        }




        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GroundDeathManager.ResetValuesAndSaveScore();
        
        
        }
        



    }




    

}



