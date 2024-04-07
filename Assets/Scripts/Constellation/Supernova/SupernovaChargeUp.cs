using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SupernovaChargeUp : MonoBehaviour, IEMPDisruptable
{
    // Start is called before the first frame update






    bool can_rotate = false;

    float[] rotation_speeds;
    Vector3[] scales;



    float rotation_speed;
    Vector3 scale;



    int index = 1;





    List<Material> shoot_mats = new();




    SupernovaColorChange supernova_color_change;


   public void OnEMP() => StopAllCoroutines();

   
    void Awake()
    {
        

        supernova_color_change = GetComponent<SupernovaColorChange>();
        float target_speed = 2f;


        rotation_speeds = new float[9];


        for (int i = 1; i <= 8; i++)
        {
            rotation_speeds[i] = (target_speed / 8) * i;


        }


        scales = new Vector3[9];

        Vector3 target_scale = new(750, 750, 250);



        scales[0] = Vector3.zero;

        
        for (int i = 1; i <= 8; i++)
        {
            scales[i] = new Vector3((target_scale.x / 8) * i, (target_scale.y / 8) * i, (target_scale.z / 8) * i);


        }
        


        /*
        for (int i = 1; i <= 8; i++)
        {
            scales[i] = target_scale - 50 * Mathf.Abs(8 - i) * target_scale.normalized;


        }
        */


        rotation_speed = rotation_speeds[0];

        scale = scales[0];





        StarFall.OnStarFallen += StarFallen;





        GetComponent<SupernovaColorChange>().OnColorUpFinished += () => StartCoroutine(RotateTowardsCore());
       // GetComponent<SupernovaColorChange>().OnDeathColorUpFinished += () => StartCoroutine(ShrinkAndDie(0.75f));
    }



    /*
    public IEnumerator ShrinkAndDie( float duration)
    {
        Vector3 original = transform.localScale;
        Vector3 target = Vector3.zero;


        float counter = 0f;
        while (counter < duration)
        {

            counter += Time.deltaTime;

            transform.localScale = Vector3.Lerp(original, target, counter / duration);

            yield return null;
        }



        Destroy(gameObject);

    }

    */

    private void OnDestroy()
    {
        StarFall.OnStarFallen -= StarFallen;
    }



    void StarFallen(Material m) 
    {
        if (index >= 9) return;
        rotation_speed = rotation_speeds[index];

        scale = scales[index];

        index++;





        shoot_mats.Add(m);

    }

    float lerp_speed = 2;
    public IEnumerator RotateTowardsCore()
    {

        Quaternion targetRot = Quaternion.LookRotation(GameObject.FindGameObjectWithTag(Tags.CORE).transform.position - transform.position);


        Quaternion start_rotation = transform.rotation;



        float lerp = 0;
        while (lerp <= 1)
        {

            transform.rotation = Quaternion.Slerp(start_rotation, targetRot, lerp);
            lerp += Time.deltaTime * lerp_speed;
            yield return null;


        }

        can_rotate = true;

        StartCoroutine(Shoot());
    }






    IEnumerator Shoot()
    {





        ParticleSystem ps = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();



        Vector3 ps_pos = transform.GetChild(0).position;



        


        Debug.LogError(ps_pos + " PS pos");




        /*
        List<float> size_based_lifetimes = new List<float>();

        float min_lifetime = 0.125f;
        float lifetime_increase = 0.05f;

        for (int i = 0; i < 8; i++) 
        {
            size_based_lifetimes.Add(min_lifetime + lifetime_increase*i);
        
        }
        */
        



        ps.enableEmission = true;
        var main = ps.main;
        main.startSize = transform.localScale.x / 500;

        

        foreach (Material m in shoot_mats)
        {

            ps.GetComponent<ParticleSystemRenderer>().material = m;
            ps.GetComponent<ParticleSystemRenderer>().trailMaterial = m;
            ps.Play();

            CoreCommunication.Raise_ValueChange(0, 1);


           



            supernova_color_change.RemoveColor();


            yield return StartCoroutine(ShootLaser(m));


            index--;

            rotation_speed = rotation_speeds[index];

            scale = scales[index];
            transform.GetChild(0).position = ps_pos;
            

            Debug.LogError(transform.GetChild(0).position+  "child set PS pos");


            yield return new WaitForSeconds(ps.main.duration);









        }




        Destroy(gameObject);






    }



    void AdjustLaser()
    {

        Vector3 middleVector = (originVector + targetVector) / 2f;
        laser.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser.transform.up = rotationDirection;
    }

    Vector3 originVector;
    Vector3 targetVector;



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


    float max_laser_size = 15;

    IEnumerator ShootLaser(Material mat)
    {
        if (laser != null) yield break;
        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laserRend = laser.GetComponent<Renderer>();
        laserRend.material = mat;
        laser.transform.position = transform.position;

        originVector = laser.transform.position;
        targetVector = GameObject.FindWithTag(Tags.CORE).transform.position + Vector3.forward * 0.5f;

        laser.transform.localScale = Vector3.zero;



        float distance = Vector3.Distance(originVector, targetVector);



        Vector3 start_scale = new(0, distance / 2f, 0);
        Vector3 target_scale = new(max_laser_size, distance / 2f, max_laser_size);


        yield return StartCoroutine(LaserScaleChange(start_scale, target_scale, 0.05f));

        yield return StartCoroutine(LaserScaleChange(target_scale, start_scale, 0.05f));




        Destroy(laser);
    }




    // Update is called once per frame
    void Update()
    {

        transform.localScale = scale;
        if (can_rotate)
            transform.Rotate(new(0, 0, rotation_speed));


    }

   
}
