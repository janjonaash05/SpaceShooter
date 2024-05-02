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

    /// <summary>
    /// Assigns the changing mat and if laserRend isn't null to arg m.
    /// </summary>
    void Start()
    {
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


    /// <summary>
    /// Sets charging_up to false, stops playing the SPINNER_CHARGE_UP sound.
    /// <para>Stops all coroutines, sets the charge localScale and size_unit to 0. </para>
    /// </summary>
    public void EndCharging()
    {

        charging_up = false;
        AudioManager.StopActivitySound(AudioManager.ActivityType.SPINNER_CHARGE_UP);
        StopAllCoroutines();
        charge.transform.localScale = Vector3.zero;
        size_unit = 0;


    }



    public const float CHARGE_UP_TIME = 6f;



    /// <summary>
    /// If already charging up, returns.
    /// <para>Sets charging to true, plays the SPINNER_CHARGE_UP sound. </para>
    /// <para>LERPs the charge's localScale from zero to max size over a set duration (CHARGE_UP_TIME).</para>
    /// <para>Sets the charge's scale to max, starts the Shoot coroutine.</para>
    /// </summary>
    /// <returns></returns>
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




    /// <summary>
    /// LERPs the laser transform localScale from origin to target over a duration. Calls AdjustLaser() after each change.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Adjusts the laser so it is in the middle and facing the targetVector.
    /// </summary>
    void AdjustLaser()
    {

        Vector3 middleVector = (originVector + targetVector) / 2f;
        laser.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser.transform.up = rotationDirection;
    }

    Vector3 originVector;
    Vector3 targetVector;







    /// <summary>
    /// <para>Assigns the index holder, sets charge_up_mode to false.</para>
    /// <para>While the result of calling ChangeIndex(-1) on the index holder doesn't result in -1, calls ChangeMaterialArray() and waits a set amount of time. </para>
    /// <para>Calls InitialColorSetup afterwards.</para>
    /// </summary>
    /// <returns></returns>
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





    /// <summary>
    /// Plays the SPINNER_SHOOT sound.
    /// <para>If the laser isn't null, returns.</para>
    /// <para>Creates the laser as a cylinder, assigns changing_mat to its renderer and sets its position to the charge's position.</para>
    /// <para>Assigns the originVector as the laser position, calculates the targetVector as the CORE tagged gameObject's position with an offset. </para>
    /// <para>Sets laser scale to 0, calculates the distance from the origin to target vector.</para>
    /// <para>Starts the Deplete coroutine.</para>
    /// <para>Calculates the start and target scales.</para>
    /// <para>Yields LaserScaleChange coroutine with start and target scales, disables the charge, yields it again but with opposite scale order.</para>
    /// <para>Invokes OnLaserShotPlayerDeath and destroys the laser.         w</para>
    /// </summary>
    /// <returns></returns>
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
