using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        Vector3 target_scale = new(500, 500, 250);



        scales[0] = Vector3.zero;

        for (int i = 1; i <= 8; i++)
        {
            scales[i] = new((target_scale.x / 8) * i, (target_scale.y / 8) * i, (target_scale.z / 8) * i);


        }




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



        ps.enableEmission = true;
        var main = ps.main;
        main.startSize = transform.localScale.x / 500;



        foreach (Material m in shoot_mats)
        {

            ps.GetComponent<ParticleSystemRenderer>().material = m;
            ps.Play();

            CoreCommunication.Raise_ValueChange(0, 1);






            supernova_color_change.RemoveColor();


            index--;

            rotation_speed = rotation_speeds[index];

            scale = scales[index];



            yield return new WaitForSeconds(ps.main.duration / 8);









        }




        Destroy(gameObject);






    }







    // Update is called once per frame
    void Update()
    {

        transform.localScale = scale;
        if (can_rotate)
            transform.Rotate(new(0, 0, rotation_speed));


    }

   
}
