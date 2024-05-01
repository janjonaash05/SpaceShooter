using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MouseReactivity : MonoBehaviour
{


    /// <summary>
    /// <para>Sets valid_click to false.</para>
    /// <para>If the player presses left mouse button:</para>
    /// <para>- Shoots a Raycast from the camera's position and towards forward facing direction. If the Raycast hits, assigns valid_click to the success of Raise_RaycastClick with the hit.</para>
    /// <para>- If valid_click is still false, calls Raise_MouseDown.</para>
    /// <para>If the player releases the left mouse button, calls Raise_MouseUp.</para>
    /// <para>If the player presses ESC, calls ResetValuesAndSaveScore. </para>
    /// </summary>
    void Update()
    {

        bool valid_click = false;
        if (Input.GetButtonDown("Fire1"))
        {

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {
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



