using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorMovement : MonoBehaviour
{
    // Start is called before the first frame update
    float y_amplitude, y_period, z_amplitude, z_period;
    [SerializeField] Vector2 amplitude_interval, period_interval;
    Vector3 initPos;

    Transform player;


    bool targeting, initial_movement_phase;

    DisruptorStartEndMovement start_end_movement;



    delegate float MovementTypeY(float f);
    delegate float MovementTypeZ(float f);




    MovementTypeY movement_y;
    MovementTypeZ movement_z;


    void OnEMP() => StopAllCoroutines();


    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMP;
    }

    void Start()
    {

        HelperSpawnerManager.OnEMPSpawn += OnEMP;


        player = Camera.main.transform;

        y_amplitude = new System.Random().Next((int)amplitude_interval.x, (int)amplitude_interval.y);
        z_amplitude = new System.Random().Next((int)amplitude_interval.x, (int)amplitude_interval.y);

        float min_period = DifficultyManager.GetCurrentDisruptorSpeedValue().min;

        float max_period = DifficultyManager.GetCurrentDisruptorSpeedValue().max;


        y_period = UnityEngine.Random.Range(min_period, max_period);//UnityEngine.Random.Range(period_interval.x, period_interval.y);
        z_period = UnityEngine.Random.Range(min_period, max_period);

        movement_y = (new System.Random().Next(0, 2) == 1) ? Mathf.Sin : Mathf.Cos;
        movement_z = (new System.Random().Next(0, 2) == 1) ? Mathf.Sin : Mathf.Cos;


        Vector3 targetPos = transform.position + new Vector3(0, y_amplitude * movement_y(0), z_amplitude * movement_z(0));

        transform.position = new Vector3(transform.position.x, transform.position.y, targetPos.z);

        start_end_movement = GetComponent<DisruptorStartEndMovement>();
        start_end_movement.MoveUp();



        start_end_movement.OnMoveUpFinish += EnableMovement;

        initial_movement_phase = true;


        SetTargeting(false);





    }


    void EnableMovement()
    {

        StartCoroutine(Enable());





    }


    IEnumerator Enable()
    {
        RotateDisruptor rotate = GetComponent<RotateDisruptor>();



        initPos = transform.position + new Vector3(0, -y_amplitude * movement_y(0), -z_amplitude * movement_z(0));




        yield return StartCoroutine(rotate.RotateTowards(Camera.main.transform.position));

        initial_movement_phase = false;


        time_difference = Time.time;
    }






    // Update is called once per frame
    float time_difference;


    void Update()
    {

        if (targeting || initial_movement_phase) { return; }

        Vector3 rotationDirection = (player.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90);

        //NOTE sin y sin z - wave | sin y cos z - circle



        transform.position = initPos + new Vector3(0, (y_amplitude * movement_y((Time.time - time_difference) / y_period)), (z_amplitude * movement_z((Time.time - time_difference) / z_period)));
        



    }


    public void SetTargeting(bool targeting)
    {

        this.targeting = targeting;


    }




}
