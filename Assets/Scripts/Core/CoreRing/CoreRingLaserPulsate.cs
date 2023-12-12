using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.UI.Image;

public class CoreRingLaserPulsate : MonoBehaviour
{
    // Start is called before the first frame update










    GameObject laser1, laser2;


    bool active;

    CoreRingColorChange colorChange;

    Vector3 initScale = new(5, 50f, 5);

    void SetupLasers()
    {



        Debug.LogError("Setup");
        laser1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser1.GetComponent<Collider>());
        laser1.transform.localScale = initScale;
        laser1.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(90, 0, 90));

        Debug.LogError(laser1 + " laser1");




        laser2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser2.GetComponent<Collider>());
        laser2.transform.localScale = initScale;
        laser2.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(90, 0, 0));

        Debug.LogError(laser2 + " laser2");


        rend1 = laser1.GetComponent<Renderer>();
        rend2 = laser2.GetComponent<Renderer>();



        active = true;
    }




    Renderer rend1, rend2;
    void Start()
    {
        CoreCommunication.OnCoreFullParticlesStart += () => { if (active == false) SetupLasers(); };
        CoreCommunication.OnCoreFullParticlesEnd += () => { active = false; Destroy(laser1); Destroy(laser2); };



        colorChange = GetComponent<CoreRingColorChange>();
        SetupLasers();



    }

    // Update is called once per frame



    const float pulsate_speed = 25f;

    

    void Update()
    {
        if (!active) return;


        float sizeIncrease = (Mathf.Sin(Time.time * pulsate_speed) * initScale.x);
        // laser1.transform.localScale = new Vector3(initScale.x + sizeIncrease, laser1.transform.localScale.y, initScale.z + sizeIncrease);




        // laser1.transform.localScale = new Vector3(laser1.transform.localScale.x, initScale.y + sizeIncrease, initScale.z + sizeIncrease);

      


        

        Vector3 newScale = new(initScale.x + sizeIncrease, initScale.y , initScale.z+ sizeIncrease);

        laser1.transform.localScale = newScale;
        laser2.transform.localScale = newScale;



        laser1.transform.Rotate(0, 0, 90);


        rend1.material = colorChange.changing_mat;
        rend2.material = colorChange.changing_mat;


    }
}
