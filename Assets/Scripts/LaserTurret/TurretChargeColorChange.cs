using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int ID;

    Renderer rend;
    Material off_mat;



    void Awake()
    {

        rend = GetComponent<Renderer>();
        off_mat = rend.material;

       

        switch (ID)
        {

            case 1:
                LaserTurretCommunication1.OnTurretChargeColorChange += (mat, turn_off) => { rend.material = (turn_off) ? off_mat : mat; };

                break;
            case 2:
                LaserTurretCommunication2.OnTurretChargeColorChange += (mat, turn_off) => rend.material = (turn_off) ? off_mat : mat;
                break;


        }







    }
}
