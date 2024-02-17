using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTransportRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        



        int mult = transform.CompareTag(Tags.TOKEN_TRANSPORT_PIVOT_RIGHT) ? -1:1;

        Debug.Log(mult);
        
        Vector3 rotationDirection = (GameObject.FindWithTag(Tags.TOKEN_CENTER).transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;

        transform.Rotate(Vector3.up, 90*mult);
       // transform.Rotate(Vector3.right, -90);
     //   transform.Rotate(0, 46, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(15 * Time.deltaTime, 0 , 0, Space.Self);
    }
}
