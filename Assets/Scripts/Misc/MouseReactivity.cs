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







        PlayerInputSO.OnLaserTarget1Click += LaserTurretCommunicationSO1.AttemptRaise_ManualTargeting;
        PlayerInputSO.OnLaserTarget2Click += LaserTurretCommunicationSO2.AttemptRaise_ManualTargeting;

        PlayerInputSO.OnLaserTarget1Click += (hit) => { UICommunicationSO.Raise_ScoreChange( hit.transform.GetComponent<IScoreEnumerable>().ScoreReward()); };
        PlayerInputSO.OnLaserTarget2Click += (hit) => { UICommunicationSO.Raise_ScoreChange(hit.transform.GetComponent<IScoreEnumerable>().ScoreReward()); };




        PlayerInputSO.OnAutoCollider1Click += (hit) => LaserTurretCommunicationSO1.AttempRaise_AutoTargetingAttempt();
        PlayerInputSO.OnAutoCollider1Click += (hit) => LaserTurretCommunicationSO1.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);



        PlayerInputSO.OnColorCollider1Click += (hit) => LaserTurretCommunicationSO1.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, false);
        PlayerInputSO.OnColorCollider1Click += (hit) => LaserTurretCommunicationSO1.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);








        PlayerInputSO.OnAutoCollider2Click += (hit) => LaserTurretCommunicationSO2.AttempRaise_AutoTargetingAttempt();
        PlayerInputSO.OnAutoCollider2Click += (hit) => LaserTurretCommunicationSO2.AttemptRaise_AutoCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);


        PlayerInputSO.OnColorCollider2Click += (hit) => LaserTurretCommunicationSO2.AttemptRaise_TurretCharge_ColorChange(hit.transform.GetComponent<Renderer>().material, false);
        PlayerInputSO.OnColorCollider2Click += (hit) => LaserTurretCommunicationSO2.AttemptRaise_ColorCollider_ControlColorChange(hit.transform.GetComponent<Renderer>().material);









    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {





            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {

                Debug.Log(hit.transform.tag);

                PlayerInputSO.Raise_RaycastClick(hit);
               



            }





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



