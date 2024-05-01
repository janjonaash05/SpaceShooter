using System;
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


    public static event Action<bool> OnEngagement;

    Transform slider_pivot_transform;



    void SliderControlClick(RaycastHit hit) => EngageActivation();


    private void OnDestroy()
    {
        PlayerInputCommunication.OnSliderControlClick -= SliderControlClick;
    }


    void Start()
    {
        on_material = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        off_material = GetComponent<Renderer>().materials[1];

        PlayerInputCommunication.OnSliderControlClick += SliderControlClick;




        slider_pivot_transform = slider_pivot.transform;

    }


    /// <summary>
    /// If inactive, returns.
    /// <para>Gets the camPos at the camera's position + it's front times the targeting distance.</para>
    /// <para>Gets the rotation direction as the normalized difference of the camPos and the pivots position.</para>
    /// <para>Assigns the pivot rotation as the Quaternion LookRotation result of the rotationDirection.</para>
    /// <para>Adjusts the y rotation.</para>
    /// <para>Manually clamps the zRot based on cones of vision. </para>
    /// <para>Reassigns the pivot rotation.</para>
    /// </summary>
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
        float zRot = slider_pivot_transform.rotation.eulerAngles.x; //z for final quat, x is because cam rot doesn't work and had to replace z with x, yRot is for vertical lock


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



    /// <summary>
    /// Plays the SLIDER_CONTROL_CLICK sound.
    /// <para>Switches variable active's value and invokes OnEngagement. </para>
    /// <para>If inactive, calls CancelMagazine. </para>
    /// <para>Sets the slider charge material to on/off based on active's value.</para>
    /// </summary>
    public void EngageActivation()
    {

        AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_CONTROL_CLICK);


        active = !active;
        OnEngagement?.Invoke(active);

        if (!active) { slider_pivot.GetComponentInChildren<SliderShooting>().CancelMagazine(); }

        slider_pivot.transform.GetChild(1).GetComponent<Renderer>().material = (active) ? on_material : off_material;

    }













}
