using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraHover : MonoBehaviour
{




    Button current_button;

    [SerializeField] bool IndependentFromMainMenuManager;


    public static float CURRENT_MENU_AMBIENCE_PLAY_TIME = 0f;




    /// <summary>
    /// Locks the cursor, gets all buttons into a list and plays ambience if independent.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        all_buttons = GameObject.FindGameObjectsWithTag(Tags.BUTTON).ToList().Select(x => x.GetComponent<Button>()).ToList();

        if (IndependentFromMainMenuManager) 
        {
            PlayAmbience();
        }

        
    }


   
    List<Button> all_buttons;
    AudioSource src;
    /// <summary>
    /// <para>Gets the audio source and clip, sets its settings and elapsed time, plays it.</para>
    /// </summary>
    public void PlayAmbience()
    {
        src = GetComponent<AudioSource>();
        src.clip = Resources.Load<AudioClip>("Sounds/Ambience/menu_ambience");
        src.spatialBlend = 0.5f;
        src.loop = true;
        src.volume = UserDataManager.CURRENT_DATA.VolumeMultiplier;
        src.time = CURRENT_MENU_AMBIENCE_PLAY_TIME;
        src.Play();

    }



    private void OnDestroy()
    {
        CURRENT_MENU_AMBIENCE_PLAY_TIME = src.time;
    }


    /// <summary>
    /// <para>Casts a ray to the mouse position</para>
    /// <para>If it hits a BUTTON, sets the current button to the hit objects's button and makes it OnHoverEnter().</para>
    /// <para>Also, if the left mouse button is pressed, makes it OnClick().</para>
    /// <para>If it doesn't hit a BUTTON, and the current button exists, then makes all buttons OnHoverExit().</para>
    /// </summary>
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.CompareTag(Tags.BUTTON))
            {
                current_button = hit.transform.gameObject.GetComponent<Button>();
                current_button.OnHoverEnter();

                if (Input.GetMouseButtonDown(0))
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
