using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFallIntoBomb : MonoBehaviour
{
    // Start is called before the first frame update


    Vector3 target;

    public delegate void StarFallEventHandler(Material color);
    public static event StarFallEventHandler OnStarFallen;

    [SerializeField] float speed;
   
    void Start()
    {
        target = Vector3.zero + GameObject.FindGameObjectWithTag(Tags.CONSTELLATION_BOMB).transform.localPosition ;
        GetComponent<StarChargeUp>().OnChargeUp += Fall;
       

    }





    private void OnDestroy()
    {
        
    }



    // Update is called once per frame



    public void Fall()
    {
        IEnumerator fall()
        {
            while (Vector3.Distance(transform.localPosition, target) > 0.1)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed* Time.deltaTime);
             

                yield return null;
            
            }
            OnStarFallen?.Invoke(GetComponent<Renderer>().materials[1]);
            Destroy(gameObject);
        
        
        
        }


        StartCoroutine(fall());


        
    }




    
}
