using UnityEngine;

public class RotateSpinner : MonoBehaviour
{



    public Vector3 rotation;
    public float charge_up_rotation_speed;
    bool chargeUpMode;
    void Start()
    {

        Vector3 rotationDirection = (Camera.main.transform.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.y+90, transform.rotation.eulerAngles.x);



    }

    void Update()
    {
     //   Debug.Log(index_holder.parent);
           float rotation_speed = GetComponent<SpinnerColorChange>().index_holder.parent switch
             {
                 1 => 25,
                 2 => 50,
                 3 => 75,
                 4 => 100,
                 _ => 0

             };

        chargeUpMode = GetComponent<SpinnerColorChange>().chargeUpMode;
        rotation.x = (chargeUpMode) ? charge_up_rotation_speed : rotation_speed;
       // Debug.Log("ROTX " +rotation.x);
        transform.Rotate(rotation * Time.deltaTime);


    }





}