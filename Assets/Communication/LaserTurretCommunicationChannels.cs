using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretCommunicationChannels
{


    public static  LaserTurretChannel Channel1;
    public static  LaserTurretChannel Channel2;

    public static LaserTurretChannel GetChannelByID(int ID) => ID == 1 ? Channel1 : Channel2;

    public static void Awake() 
    {
        Channel1 = new();
        Channel2 = new();

        Channel1.Awake(Tags.LASER_TARGET_1);
        Channel2.Awake(Tags.LASER_TARGET_2);
    }

    
}
