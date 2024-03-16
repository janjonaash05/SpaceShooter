using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonLaunch : MonoBehaviour
{
    GameObject harpoon_head, harpoon_station_charge;


    [SerializeField] float launch_distance;
    [SerializeField] float launch_speed;
    Material charge_color; 
    Material off_color_head, off_color_charge;




    GameObject laser_tether;
    Transform harpoon_head_transform;
    Vector3 target;
    Vector3 start;
    bool readyToLaunch = true;


    bool turnedOff = true;
    IEnumerator Launch()
    {
        if (!readyToLaunch) yield break;



        readyToLaunch = false;
        

        target = harpoon_head_transform.localPosition + Vector3.up * launch_distance;
        start =  harpoon_head_transform.localPosition;


        SetupTether();
        yield return LaunchProcess();
        
        readyToLaunch = true;
        


    }

    // Update is called once per frame
    void Update()
    {

    }


    void Start()
    {

        charge_color = MaterialHolder.Instance().SIDE_TOOLS_COLOR();




        harpoon_head = transform.GetChild(0).gameObject;
        harpoon_head_transform = harpoon_head.transform;
        harpoon_station_charge = transform.GetChild(1).gameObject;

        start = harpoon_head_transform.position; ;

        Debug.Log(start);

        Renderer head_renderer = harpoon_head.GetComponent<Renderer>();
        off_color_head = head_renderer.materials[1];
        off_color_charge = head_renderer.materials[0];

        PlayerInputCommunication.OnHarpoonColliderClick += (hit) =>
        {
            turnedOff = !turnedOff;
            Debug.Log(turnedOff + " turned off");
            harpoon_station_charge.GetComponent<Renderer>().material = (turnedOff) ? off_color_charge : charge_color;

            Material[] head_mats = harpoon_head.GetComponent<Renderer>().materials;
            head_mats[2] = turnedOff ? off_color_head : charge_color;

            harpoon_head.GetComponent<Renderer>().materials = head_mats;


        };




        PlayerInputCommunication.OnMouseDown += () => { if (!turnedOff) StartCoroutine(Launch()); };
    
    }




    


    IEnumerator LaunchProcess()
    {


        


        // Vector3 start_point = transform.position;
       
        //Vector3 targetPoint = (backwards) ? start : target;
        //Vector3 startPoint = harpoon_head_transform.localPosition;


        //
                          
                          
                         
        while (Vector3.Distance(harpoon_head_transform.localPosition, target) > 0.001f)
        {
          //  Debug.LogWarning(Vector3.Distance(startPoint, targetPoint));

            //  harpoon_head_transform.Translate( Time.deltaTime* launch_speed*(target - harpoon_head_transform.localPosition) );


            harpoon_head_transform.localPosition = Vector3.MoveTowards(harpoon_head_transform.localPosition, target, launch_speed * Time.deltaTime);


            Track(harpoon_head_transform.position);
            yield return new WaitForEndOfFrame();

        }



        while (Vector3.Distance(harpoon_head_transform.localPosition, start) > 0.001f)
        {
            //  Debug.LogWarning(Vector3.Distance(startPoint, targetPoint));

            //  harpoon_head_transform.Translate( Time.deltaTime* launch_speed*(target - harpoon_head_transform.localPosition) );


            harpoon_head_transform.localPosition = Vector3.MoveTowards(harpoon_head_transform.localPosition, start, launch_speed * Time.deltaTime);


            Track(harpoon_head_transform.position);
            yield return new WaitForEndOfFrame();

        }



        Debug.LogWarning("end");
        Destroy(laser_tether);
    }



  




    Vector3 originVector;
    public void SetupTether()
    {



        laser_tether = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser_tether.GetComponent<Collider>());
        laser_tether.GetComponent<Renderer>().sharedMaterial = charge_color;
        laser_tether.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float angle = 45*180/Mathf.PI;// transform.rotation.eulerAngles.x;

        originVector = transform.position + new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle),0) ;

        

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
