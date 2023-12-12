using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerChargeUp : MonoBehaviour
{
    float size_unit;
    [SerializeField] float max_size_unit, delta_size_unit, delay, laser_size_increase, max_laser_size;
    public Vector3 rotation;

    public GameObject charge;
    Material changing_mat;



    void Start()
    {
        SpinnerColorChange.OnMaterialChange += (m) => { if (laserRend != null) changing_mat = m; laserRend.material = m; };
    }


    void Update()
    {
        charge.transform.Rotate(rotation * Time.deltaTime);
    }


    public void StartCharging()
    {
        StartCoroutine(Charge());
    }

    public void EndCharging()
    {

        StopAllCoroutines();
        charge.transform.localScale = new Vector3(0, 0, 0);
        size_unit = 0;


    }


    IEnumerator Charge()
    {

        while (size_unit < max_size_unit)
        {

            size_unit += delta_size_unit;
            charge.transform.localScale = new Vector3(size_unit, size_unit, size_unit);
            yield return new WaitForSeconds(delay);

        }

        charge.transform.localScale = new Vector3(max_size_unit, max_size_unit, max_size_unit);
        StartCoroutine(Shoot());
    }






    GameObject laser;
    Renderer laserRend;
    IEnumerator Shoot()
    {
        if (laser != null) yield break;
         laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laserRend = laser.GetComponent<Renderer>();
        laserRend.material = changing_mat;
        laser.transform.position = charge.transform.position;

        Vector3 originVector = laser.transform.position;
        Vector3 targetVector = GameObject.FindWithTag(Tags.CORE).transform.position + Vector3.forward*0.5f;

        laser.transform.localScale = Vector3.zero;
       

       
        float distance = Vector3.Distance(originVector, targetVector);



        while (laser.transform.localScale.x < max_laser_size) 
        {
          
        laser.transform.localScale = new Vector3(laser.transform.localScale.x + laser_size_increase, distance / 2f, laser.transform.localScale.z + laser_size_increase);

        Vector3 middleVector = (originVector + targetVector) / 2f;
        laser.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser.transform.up = rotationDirection;
            yield return null;

        }

    }
}
