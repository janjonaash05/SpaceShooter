using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CommunicationHelper : MonoBehaviour
{
    
    void Awake()
    {
        MaterialHolder.Awake();
        UpgradesManager.Awake();
        LaserTurretCommunicationChannels.Awake();
        CoreCommunication.Awake();
        UICommunication.Awake();
        
      


        PlayerInputCommunication.OnLaserTarget1Click += LaserTarget1CLick;
        PlayerInputCommunication.OnLaserTarget2Click += LaserTarget2CLick;

        PlayerInputCommunication.OnAutoCollider1Click += AutoCollider1Click;
        PlayerInputCommunication.OnAutoCollider2Click += AutoCollider2Click;

        PlayerInputCommunication.OnColorCollider1Click += ColorCollider1Click;
        PlayerInputCommunication.OnColorCollider2Click += ColorCollider2Click;



        BombColorChange.InitCoverages();

    }



    void LaserTarget1CLick(RaycastHit hit) { LaserTurretCommunicationChannels.Channel1.AttemptRaise_ManualTargeting(hit); }
    void LaserTarget2CLick(RaycastHit hit) { LaserTurretCommunicationChannels.Channel2.AttemptRaise_ManualTargeting(hit); }

    void AutoCollider1Click(RaycastHit hit)
    {
        LaserTurretCommunicationChannels.Channel1.AttempRaise_AutoTargetingAttempt();
        LaserTurretCommunicationChannels.Channel1.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);
    }

    void AutoCollider2Click(RaycastHit hit)
    {
        LaserTurretCommunicationChannels.Channel2.AttempRaise_AutoTargetingAttempt();
        LaserTurretCommunicationChannels.Channel2.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);
    }
    void ColorCollider2Click(RaycastHit hit)
    {
        LaserTurretCommunicationChannels.Channel2.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, Enum.Parse<COLOR>(hit.transform.name, true), false);
        LaserTurretCommunicationChannels.Channel2.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);

    }


    void ColorCollider1Click(RaycastHit hit)
    {
        LaserTurretCommunicationChannels.Channel1.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, Enum.Parse<COLOR>(hit.transform.name, true), false);
        LaserTurretCommunicationChannels.Channel1.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);

    }
















    public void OnDestroy()
    {

        PlayerInputCommunication.OnLaserTarget1Click -= LaserTarget1CLick;
        PlayerInputCommunication.OnLaserTarget2Click -= LaserTarget2CLick;



        PlayerInputCommunication.OnAutoCollider1Click -= AutoCollider1Click;
        PlayerInputCommunication.OnAutoCollider2Click -= AutoCollider2Click;

        PlayerInputCommunication.OnColorCollider1Click -= ColorCollider1Click;
        PlayerInputCommunication.OnColorCollider2Click -= ColorCollider2Click;
    }


}
