using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using System;
using System.Collections.Generic;

public class MouseReactivity : MonoBehaviour
{
    // Start is called before the first frame update




















    void Start()
    {





         



    }

    // Update is called once per frame
    void Update()
    {

        bool valid_click = false;
        if (Input.GetButtonDown("Fire1"))
        {



            

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {

                Debug.Log(hit.transform.tag);

                 valid_click =  PlayerInputCommunication.Raise_RaycastClick(hit);
                



            }


            if (!valid_click)
            {
                PlayerInputCommunication.Raise_MouseDown();


            }
        }
       

        if (Input.GetButtonUp("Fire1")) 
        {
        
            PlayerInputCommunication.Raise_MouseUp();
        }


        



    }





    void EmptyReaction(int i, RaycastHit hit)
    {

        Debug.Log("EmptyReact");

    }

    void SliderControlReaction(int i, RaycastHit hit)
    {
      
    }



    void SliderFullAutoReaction(int i, RaycastHit hit)
    {
        // slider_control.GetComponent<SliderLoaderControlColorChange>().Engage(hit.transform.GetComponent<Renderer>().material, true, false);
        // slider_loader_full_auto_pivot.GetComponent<SliderLoaderRecharge>().OnActivationInvoke();


        //  slider_control.GetComponent<SliderLoaderControlColorChange>().Engage(hit.transform.GetComponent<Renderer>().material, false, true);
        //slider_loader_bolt_pivot.GetComponent<SliderLoaderRecharge>().OnDeactivationInvoke();


        //  slider_turret_head.GetComponent<SliderShooting>().loader_recharge = slider_loader_full_auto_pivot.GetComponent<SliderLoaderRecharge>();
    }

    void SliderBoltReaction(int i, RaycastHit hit)
    {
        //slider_control.GetComponent<SliderLoaderControlColorChange>().Engage(hit.transform.GetComponent<Renderer>().material, false, false);
        //  slider_loader_bolt_pivot.GetComponent<SliderLoaderRecharge>().OnActivationInvoke();


        //slider_control.GetComponent<SliderLoaderControlColorChange>().Engage(hit.transform.GetComponent<Renderer>().material, true, true);
        //slider_loader_full_auto_pivot.GetComponent<SliderLoaderRecharge>().OnDeactivationInvoke();


        // slider_turret_head.GetComponent<SliderShooting>().loader_recharge = slider_loader_bolt_pivot.GetComponent<SliderLoaderRecharge>();
    }


    void ColorColliderReaction(int i, RaycastHit hit)
    {

        /*


        GameObject turret_head, turret_station, turret_control;


        switch (i)
        {
            case 1:
           //     if (turret_control_1.GetComponent<LaserControlDisableManager>().Disabled) { return; }
                turret_head = turret_head_1;
                turret_control = turret_control_1;
                break;
            case 2:
          //      if (turret_control_2.GetComponent<LaserControlDisableManager>().Disabled) { return; }
                turret_head = turret_head_2;
                turret_control = turret_control_2;
                break;
            default: throw new ArgumentException("Input must be 1 or 2");
        }

        if (!turret_head.GetComponent<TargetBomb>().isTargeting)
        {
            /*
            turret_head.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = hit.transform
                .GetComponent<Renderer>()
                .material;
            */
        /* turret_station.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = hit.transform
             .GetComponent<Renderer>()
             .material;
        */


        /*
                    turret_control
                        .GetComponent<ControlColorChange>()
                        .StartChange(hit);



                }

                       */

    }

    void AutoColliderReaction(int i, RaycastHit hit)
    {


        //   GameObject turret_control, turret_head;
        //  string tag;
        /*
                switch (i)
                {
                    case 1:
                      //  if (turret_control_1.GetComponent<LaserControlDisableManager>().Disabled) { return; }


                        turret_control = turret_control_1;

                        tag = Tags.LASER_TARGET_1;

                        turret_head = turret_head_1;
                        break;
                    case 2:
                    //    if (turret_control_2.GetComponent<LaserControlDisableManager>().Disabled) { return; }

                        turret_control = turret_control_2;

                        tag = Tags.LASER_TARGET_2;

                        turret_head = turret_head_2;
                        break;
                    default: throw new ArgumentException("Input must be 1 or 2");
                }

        */
        //    if (hit.transform.GetComponent<Renderer>().material.color == block_material.color) { return; }


        /*
        turret_control
            .GetComponent<ControlColorChange>();
          //  .StartChange(hit);


       turret_head.GetComponent<TargetBomb>().BarrageStart(tag);


        */








    }

    void LaserTargetReaction(int i, RaycastHit hit)
    {

        /*
       
        GameObject turret_head;
        switch (i)
        {
            case 1:
              //  if (turret_control_1.GetComponent<LaserControlDisableManager>().Disabled) { return; }
                turret_head = turret_head_1;
                break;
            case 2:
            //    if (turret_control_2.GetComponent<LaserControlDisableManager>().Disabled) { return; }
                turret_head = turret_head_2;
                break;
            default: throw new ArgumentException("Must be 1 or 2");
        }


        GameObject turret_head_charge = turret_head.transform.GetChild(0).gameObject;

        if ((hit.transform.gameObject.GetComponent<BombColorChange>().Color.color.Equals(turret_head_charge.GetComponent<Renderer>().sharedMaterial.color)) && !turret_head.GetComponent<TargetBomb>().isTargeting)
        {
            

          _=  turret_head
                .GetComponent<TargetBomb>()
                .StartTargeting(hit.transform.gameObject);


        //    ScoreCounter.Increase(hit.transform.GetComponent<IScoreEnumerable>().ScoreReward());
        //    GameObject.FindGameObjectWithTag(Tags.SPINNER).GetComponent<SpinnerColorChange>().ChangeIndexHolder(0, -1);

        */
    }

}



