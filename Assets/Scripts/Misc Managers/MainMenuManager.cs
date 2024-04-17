using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    
    void Start()
    {
        UserDataManager.Load();


        Screen.SetResolution(UserDataManager.CURRENT_DATA.Resolution[0], UserDataManager.CURRENT_DATA.Resolution[1], UserDataManager.CURRENT_DATA.Fullscreen);



    }

    
    void Update()
    {
        
    }
}
