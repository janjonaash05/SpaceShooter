using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretCommunicationChannels
{


    public static  LaserTurretChannel Channel1;
    public static  LaserTurretChannel Channel2;



    public static void Awake() 
    {
        Channel1 = new();
        Channel2 = new();

        Channel1.Awake();
        Channel2.Awake();
    }

    
}
