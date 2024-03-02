using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGrab : MonoBehaviour
{
    

    


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

            


            collision.transform.parent = gameObject.transform;
            collision.transform.localPosition = new(0, -0.02f, 0);

            collision.gameObject.GetComponent<TokenMovement>().Stop();
            collision.gameObject.GetComponent<TokenColorChange>().CoverInColor();
        
        }
    }
}
