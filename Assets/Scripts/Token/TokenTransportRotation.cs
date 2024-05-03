using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTransportRotation : MonoBehaviour
{
    
    void Start()
    {
        



        int mult = transform.CompareTag(Tags.TOKEN_TRANSPORT_PIVOT_RIGHT) ? -1:1;

      
        
        Vector3 rotationDirection = (GameObject.FindWithTag(Tags.TOKEN_CENTER).transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;

        transform.Rotate(Vector3.up, 90*mult);
    }

    



    float speed = 45f;
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0 , 0, Space.Self);
    }
}
