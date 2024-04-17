using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraHover : MonoBehaviour
{




    Button current_button;

    

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;


        all_buttons = GameObject.FindGameObjectsWithTag(Tags.BUTTON).ToList().Select(x=> x.GetComponent<Button>()).ToList();


        

    }



    List<Button> all_buttons;


  

    
    void Update()
    {


       

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        
        Debug.DrawRay(ray.origin,ray.direction, Color.magenta);

        if (Physics.Raycast(ray, out var hit))
        {


          
            if (hit.transform.CompareTag(Tags.BUTTON))
            {
                current_button = hit.transform.gameObject.GetComponent<Button>();
                current_button.OnHoverEnter();




                if(Input.GetMouseButtonDown(0)) 
                {
                    current_button.OnClick();
                
                
                }










            }

        }
        else
        {
            if (current_button == null) return;
            foreach (var item in all_buttons)
            {
                item.OnHoverExit();
            }
        }





    }
}
