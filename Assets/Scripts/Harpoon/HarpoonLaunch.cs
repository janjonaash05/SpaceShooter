using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonLaunch : MonoBehaviour
{
    GameObject harpoon_head;
    [SerializeField] float launch_distance;
    [SerializeField] float launch_speed;



    GameObject laser_hook;
    Transform harpoon_head_transform;
    Vector3 target;
    void Start()
    {
        harpoon_head = transform.GetChild(0).gameObject;
        harpoon_head_transform = harpoon_head.transform;


      //  Launch();

        target = Vector3.down * launch_distance;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Launch()
    {


        IEnumerator launchProcess()
        {

            Vector3 start_point = transform.position;
 

          
            while (Vector3.Distance(harpoon_head_transform.position, target) > 0.001 )
            {

                harpoon_head_transform.Translate( Time.deltaTime* launch_speed*(target - harpoon_head_transform.localPosition) );

                yield return new WaitForEndOfFrame();
            
            }



        
        
        
        }


        StartCoroutine(launchProcess());


    
    }
}
