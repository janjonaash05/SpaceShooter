using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LaserTurretCommunication1.Awake();
        LaserTurretCommunication2.Awake();
        CoreCommunication.Awake();

    }

 
}
