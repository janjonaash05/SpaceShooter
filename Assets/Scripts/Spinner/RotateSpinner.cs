using UnityEngine;

public class RotateSpinner : MonoBehaviour
{



    public Vector3 rotation;
    public float charge_up_rotation_speed;
    bool chargeUpMode;
    void Start()
    {

        Vector3 core_pos = GameObject.FindGameObjectWithTag(Tags.CORE).transform.position;

        Vector3 rotationDirection = (core_pos - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.y+90, transform.rotation.eulerAngles.x);



    }

    void Update()
    {
           float rotation_speed = GetComponent<SpinnerColorChange>().index_holder.Parent switch
             {
                 1 => 25,
                 2 => 50,
                 3 => 75,
                 4 => 100,
                 _ => 0

             };

        chargeUpMode = GetComponent<SpinnerColorChange>().charge_up_mode;
        rotation.x = (chargeUpMode) ? charge_up_rotation_speed : rotation_speed;
        transform.Rotate(rotation * Time.deltaTime);


    }





}