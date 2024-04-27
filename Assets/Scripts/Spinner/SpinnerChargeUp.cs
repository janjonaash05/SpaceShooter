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






    bool charging_up = false;

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

        charging_up = false;
        AudioManager.StopActivitySound(AudioManager.ActivityType.SPINNER_CHARGE_UP);
        StopAllCoroutines();
        charge.transform.localScale = new Vector3(0, 0, 0);
        size_unit = 0;


    }



    public const float CHARGE_UP_TIME = 6f;

    IEnumerator Charge()
    {

        if (charging_up) yield break;
        charging_up = true;


        AudioManager.PlayActivitySound(AudioManager.ActivityType.SPINNER_CHARGE_UP);
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





    IEnumerator LaserScaleChange(Vector3 origin, Vector3 target, float duration) 
    {

        float lerp = 0;

        while (lerp < duration)
        {
            lerp += Time.deltaTime;

            laser.transform.localScale = Vector3.Lerp(origin, target, lerp / duration);
            AdjustLaser();
            yield return null;

        }

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

        colorChange.charge_up_mode = false;





        int res = 0;


        while (res != -1)
        {
            res = index_holder.ChangeIndex(0, -1);

            colorChange.ChangeMaterialArray();

            yield return new WaitForSeconds(0.05f);

        }

        colorChange.InitialColorSetup();
     


  




    }






    IEnumerator Shoot()
    {



        AudioManager.PlayActivitySound(AudioManager.ActivityType.SPINNER_SHOOT);
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

      
        Vector3 start_scale = new(0, distance / 2f, 0);
        Vector3 target_scale = new(max_laser_size, distance / 2f, max_laser_size);


        yield return StartCoroutine(LaserScaleChange(start_scale,target_scale, 0.5f));


        charge.SetActive(false);


        yield return StartCoroutine(LaserScaleChange(target_scale, start_scale, 0.5f));

        

        OnLaserShotPlayerDeath?.Invoke();

        Destroy(laser);
    }
}
