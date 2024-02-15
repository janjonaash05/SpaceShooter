using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderControlActivation : MonoBehaviour
{


    public bool active = false;
    [SerializeField] float targeting_distance;


    public const float covX = 60, covY = 60;



     Material on_material, off_material;



    public GameObject slider_pivot;


    Transform slider_pivot_transform;


    void Start()
    {
        on_material = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        off_material = GetComponent<Renderer>().materials[1];

        PlayerInputCommunication.OnSliderControlClick += (_) => EngageActivation();



        slider_pivot_transform = slider_pivot.transform;

    }



    void Update()
    {

        if (!active)
        {


            return;
        }






        Vector3 camPos = Camera.main.transform.position + Camera.main.transform.forward * targeting_distance;



        Vector3 rotationDirection = (camPos - slider_pivot_transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        slider_pivot_transform.rotation = rot;


        float yRot = slider_pivot_transform.rotation.eulerAngles.y + 90; // rotate turret front to actually be infront
                                                                         //Debug.Log(slider_pivot.transform.rotation.eulerAngles.x);




        float zRot = slider_pivot_transform.rotation.eulerAngles.x; //z for final quat, x is because cam rot doesnt work and had to replace z with x, yRot is for vertical lock


        zRot = zRot switch
        {

            < (360 - covY - 180) => 359,

            < 360 - covY => 360 - covY,
            _ => zRot

        };


        yRot = yRot switch
        {

            < (360 - covX) => 360 - covX,

            > 360 + covX => 360 + covX,
            _ => yRot

        };


        slider_pivot.transform.rotation = Quaternion.Euler(0, yRot, zRot);




    }




    public void EngageActivation()
    {

        active = !active;

        if (!active) { slider_pivot.GetComponentInChildren<SliderShooting>().CancelMagazine(); }

        slider_pivot.transform.GetChild(1).GetComponent<Renderer>().material = (active) ? on_material : off_material;

    }













}
