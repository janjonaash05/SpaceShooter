using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        LaserTurretCommunicationChannels.Awake();
        CoreCommunication.Awake();






        PlayerInputCommunication.OnLaserTarget1Click += LaserTurretCommunicationChannels.Channel1.AttemptRaise_ManualTargeting;
        PlayerInputCommunication.OnLaserTarget2Click += LaserTurretCommunicationChannels.Channel2.AttemptRaise_ManualTargeting;





        PlayerInputCommunication.OnAutoCollider1Click += (hit) => LaserTurretCommunicationChannels.Channel1.AttempRaise_AutoTargetingAttempt();
        PlayerInputCommunication.OnAutoCollider1Click += (hit) => LaserTurretCommunicationChannels.Channel1.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);



        PlayerInputCommunication.OnColorCollider1Click += (hit) => LaserTurretCommunicationChannels.Channel1.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, false);
        PlayerInputCommunication.OnColorCollider1Click += (hit) => LaserTurretCommunicationChannels.Channel1.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);








        PlayerInputCommunication.OnAutoCollider2Click += (hit) => LaserTurretCommunicationChannels.Channel2.AttempRaise_AutoTargetingAttempt();
        PlayerInputCommunication.OnAutoCollider2Click += (hit) => LaserTurretCommunicationChannels.Channel2.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);


        PlayerInputCommunication.OnColorCollider2Click += (hit) => LaserTurretCommunicationChannels.Channel2.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, false);
        PlayerInputCommunication.OnColorCollider2Click += (hit) => LaserTurretCommunicationChannels.Channel2.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);
























    }


}
