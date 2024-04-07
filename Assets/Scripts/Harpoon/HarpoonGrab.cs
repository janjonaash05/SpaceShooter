using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGrab : MonoBehaviour
{

    public event Action OnSuccessfulGrab;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }






    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HG coll");

        if (collision.transform.CompareTag(Tags.TOKEN)) 
        {

            OnSuccessfulGrab?.Invoke();



            Destroy(collision.gameObject.GetComponent<Collider>());




            collision.transform.parent = gameObject.transform;
            collision.transform.SetLocalPositionAndRotation(new(0, -0.02f, 0), Quaternion.Euler(0,180,0));

            //collision.transform.localPosition = new(0, -0.02f, 0);
            //collision.transform.localRotation = Quaternion.Euler(0, 180, 0);
            //

            collision.gameObject.GetComponent<TokenMovement>().Stop();
            collision.gameObject.GetComponent<TokenColorChange>().CoverInColor();
        
        }
    }
}
