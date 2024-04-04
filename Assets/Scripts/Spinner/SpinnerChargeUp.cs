using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Android;

public class SpinnerChargeUp : MonoBehaviour
{
    float size_unit;
    [SerializeField] float max_size_unit, delta_size_unit, delay, laser_size_increase, max_laser_size;
    public Vector3 rotation;

    public GameObject charge;
    Material changing_mat;

    public static event Action OnLaserShotPlayerDeath;

    void Start()
    {
        //problematic
        SpinnerColorChange.OnMaterialChange += (m) =>
        {
            changing_mat = m;
            if (laserRend != null) { laserRend.material = m; }
        };





        CoreCommunication.OnSpinnerChargeUpStart += StartCharging;
        CoreCommunication.OnSpinnerChargeUpEnd += EndCharging;

    }


    void Update()
    {
        if (charge == null) return;
        charge.transform.Rotate(rotation * Time.deltaTime);
    }




    private void OnDestroy()
    {
        CoreCommunication.OnSpinnerChargeUpStart -= StartCharging;
        CoreCommunication.OnSpinnerChargeUpEnd -= EndCharging;
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



    public const float CHARGE_UP_TIME = 5f;

    IEnumerator Charge()
    {

        float duration = CHARGE_UP_TIME;

        float lerp = 0;





       

        while(lerp < duration)
        {
            lerp += Time.deltaTime;

            charge.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * max_size_unit, lerp / duration); ;

            yield return null;

        }





        charge.transform.localScale = new Vector3(max_size_unit, max_size_unit, max_size_unit);
        StartCoroutine(Shoot());
    }






    GameObject laser;
    Renderer laserRend;



    void AdjustLaser()
    {




        Vector3 middleVector = (originVector + targetVector) / 2f;
        laser.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser.transform.up = rotationDirection;
    }

    Vector3 originVector;
    Vector3 targetVector;








    IEnumerator Deplete()
    {
        var index_holder = CoreCommunication.SPINNER_INDEX_HOLDER;



        SpinnerColorChange colorChange = GetComponent<SpinnerColorChange>();



        index_holder.SetEdge(MaterialIndexHolder.Edge.UPPER);



        do
        {
            colorChange.ChangeMaterialArray();

            yield return new WaitForSeconds(0.05f);

        }
        while (index_holder.ChangeIndex(0, -1) != -1);


    }






    IEnumerator Shoot()
    {
        if (laser != null) yield break;
        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laserRend = laser.GetComponent<Renderer>();
        laserRend.material = changing_mat;
        laser.transform.position = charge.transform.position;

        originVector = laser.transform.position;
        targetVector = GameObject.FindWithTag(Tags.CORE).transform.position + Vector3.forward * 0.5f;

        laser.transform.localScale = Vector3.zero;



        float distance = Vector3.Distance(originVector, targetVector);


        StartCoroutine(Deplete());


        while (laser.transform.localScale.x < max_laser_size)
        {

            laser.transform.localScale = new Vector3(laser.transform.localScale.x + laser_size_increase, distance / 2f, laser.transform.localScale.z + laser_size_increase);

            AdjustLaser();
            yield return null;

        }


        charge.SetActive(false);

        while (laser.transform.localScale.x > 0)
        {

            laser.transform.localScale = new Vector3(laser.transform.localScale.x - laser_size_increase, distance / 2f, laser.transform.localScale.z - laser_size_increase);

            AdjustLaser();
            yield return null;

        }



        OnLaserShotPlayerDeath?.Invoke();

        Destroy(laser);
    }
}
