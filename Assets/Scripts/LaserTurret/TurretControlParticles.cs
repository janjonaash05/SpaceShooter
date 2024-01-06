using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControlParticles : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] int ID;



    void Start()
    {

        switch (ID) 
        {
            case 1:
                //LaserTurretCommunication1.OnControlDisabled += TurnOff;
                //LaserTurretCommunication1.OnControlEnabled += TurnOn;
                break;

        }

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
