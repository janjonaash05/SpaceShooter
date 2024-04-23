using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{



    [SerializeField] CameraHover camHover;





    void Start()
    {

        if (CameraHover.CURRENT_MENU_AMBIENCE_PLAY_TIME == 0) 
        {
            CameraHover.CURRENT_MENU_AMBIENCE_PLAY_TIME = 0;


        }

        UserDataManager.Load();


        Screen.SetResolution(UserDataManager.CURRENT_DATA.Resolution[0], UserDataManager.CURRENT_DATA.Resolution[1], UserDataManager.CURRENT_DATA.Fullscreen);


        camHover.PlayAmbience();
    }





    

    void Update()
    {

    }
}
