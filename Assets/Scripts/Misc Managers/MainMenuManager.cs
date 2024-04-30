using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{



    [SerializeField] CameraHover camHover;




    /// <summary>
    /// <para>Loads UserData, sets the resolution and fullscreen, plays ambience.</para>
    /// </summary>
    void Start()
    {

        UserDataManager.Load();


        Screen.SetResolution(UserDataManager.CURRENT_DATA.Resolution[0], UserDataManager.CURRENT_DATA.Resolution[1], UserDataManager.CURRENT_DATA.Fullscreen);


        camHover.PlayAmbience();
    }





    

    
}
