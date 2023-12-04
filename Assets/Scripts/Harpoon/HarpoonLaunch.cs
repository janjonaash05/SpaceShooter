using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonLaunch : MonoBehaviour
{
    GameObject harpoon_head;
    [SerializeField] float launch_distance;
    [SerializeField] float launch_speed;
    [SerializeField] Material white;




    GameObject laser_tether;
    Transform harpoon_head_transform;
    Vector3 target;
    Vector3 start;
    IEnumerator Start()
    {
        harpoon_head = transform.GetChild(0).gameObject;
        harpoon_head_transform = harpoon_head.transform;

        target = harpoon_head_transform.localPosition + Vector3.up * launch_distance;
        start = harpoon_head_transform.localPosition;



        SetupTether();
        yield return LaunchProcess(false);
        yield return LaunchProcess(true);
        Debug.LogWarning(target);

        ;


    }

    // Update is called once per frame
    void Update()
    {

    }




    IEnumerator LaunchProcess(bool backwards)
    {

        // Vector3 start_point = transform.position;
       
        Vector3 targetPoint = (backwards) ? start : target;
        Vector3 startPoint = harpoon_head_transform.localPosition;


        //
                          
                          
                         
        while (Vector3.Distance(harpoon_head_transform.localPosition, target) > 0.1f)
        {
          //  Debug.LogWarning(Vector3.Distance(startPoint, targetPoint));

            //  harpoon_head_transform.Translate( Time.deltaTime* launch_speed*(target - harpoon_head_transform.localPosition) );


            harpoon_head_transform.localPosition = Vector3.MoveTowards(harpoon_head_transform.localPosition, targetPoint, launch_speed * Time.deltaTime);


            Track(harpoon_head_transform.position);
            yield return new WaitForEndOfFrame();

        }



        Debug.LogWarning("end");

    }








    Vector3 originVector;
    public void SetupTether()
    {



        laser_tether = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser_tether.GetComponent<Collider>());
        laser_tether.GetComponent<Renderer>().sharedMaterial = white;
        laser_tether.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        originVector = transform.position;


        //  isTargeting = true;


    }


    void Track(Vector3 targetVector)
    {


        float distance = Vector3.Distance(originVector, targetVector);

        laser_tether.transform.localScale = new Vector3(laser_tether.transform.localScale.x, distance * 0.5f, laser_tether.transform.localScale.z);

        Vector3 middleVector = 0.5f * (originVector + targetVector);
        laser_tether.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser_tether.transform.up = rotationDirection;

    }
}
