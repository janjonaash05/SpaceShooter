using UnityEngine;

public class RotateSpinner : MonoBehaviour
{



    public Vector3 rotation;
    public float charge_up_rotation_speed;


    SpinnerColorChange color_change;


    void Start()
    {

        color_change = GetComponent<SpinnerColorChange>();


        Vector3 core_pos = GameObject.FindGameObjectWithTag(Tags.CORE).transform.position;

        Vector3 rotationDirection = (core_pos - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        transform.rotation = rot;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.y+90, transform.rotation.eulerAngles.x);

        

    }



    /// <summary>
    /// Gets the rotation speed either as the charge_up speed, or the one based on the color_change index holder Parent value. Determined by color_change charge_up_mode.
    /// </summary>
    void Update()
    {
          

        rotation.x = (color_change.charge_up_mode) ? charge_up_rotation_speed : color_change.index_holder.Parent switch
        {
            1 => 25,
            2 => 50,
            3 => 75,
            4 => 100,
            _ => 0

        }; ;
        transform.Rotate(rotation * Time.deltaTime);


    }





}