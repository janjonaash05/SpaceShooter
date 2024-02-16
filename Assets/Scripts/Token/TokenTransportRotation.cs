using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTransportRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        
        Vector3 rotationDirection = (GameObject.FindWithTag(Tags.TOKEN_CENTER).transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;

       // transform.Rotate(Vector3.up, 90);
        transform.Rotate(Vector3.right, -90);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
