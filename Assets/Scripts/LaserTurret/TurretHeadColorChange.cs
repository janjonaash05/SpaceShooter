using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHeadColorChange : MonoBehaviour
{
    // Start is called before the first frame update


    Renderer rend;
    [SerializeField] int ID;
    void Start()
    {

        rend = GetComponent<Renderer>();
    

        switch (ID) 
        {
            case 1:

                LaserTurretCommunicationChannels.Channel1.OnGeneralTargetingStart += ActivateColor;
                LaserTurretCommunicationChannels.Channel1.OnGeneralTargetingEnd += DeactivateColor;

                break;




            case 2:

                LaserTurretCommunicationChannels.Channel2.OnGeneralTargetingStart += ActivateColor;
                LaserTurretCommunicationChannels.Channel2.OnGeneralTargetingEnd += DeactivateColor;

                break;

        }



        off_mat = rend.materials[2]; ;
    }


    Material off_mat;

    // Update is called once per frame


    void ActivateColor()
    {
        Material on_mat = transform.GetChild(0).GetComponent<Renderer>().material;
        rend.materials = new Material[] { rend.materials[0], rend.materials[1], on_mat };
    }

    void DeactivateColor()
    {

        rend.materials = new Material[] { rend.materials[0], rend.materials[1], off_mat };
    }
}
