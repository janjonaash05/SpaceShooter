using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGrab : MonoBehaviour
{

    public event Action OnSuccessfulGrab;
    


    /// <summary>
    /// <para>If collides with TOKEN gameObject:</para>
    /// <para>Invokes OnSuccessfulGrab.</para>
    /// <para>Destroys the collider, assigns the parent to this object and adjusts its rotation, position.</para>
    /// <para>Calls Stop() on TokenMovement and orders CoverInColor() on the TokenColorChange.</para>
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag(Tags.TOKEN)) 
        {

            OnSuccessfulGrab?.Invoke();

            Destroy(collision.gameObject.GetComponent<Collider>());


            collision.transform.parent = gameObject.transform;
            collision.transform.SetLocalPositionAndRotation(new(0, -0.02f, 0), Quaternion.Euler(0,180,0));

            collision.gameObject.GetComponent<TokenMovement>().Stop();
            collision.gameObject.GetComponent<TokenColorChange>().CoverInColor();
        
        }
    }
}
