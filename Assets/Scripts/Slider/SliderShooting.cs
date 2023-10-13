
using System;
using System.Collections;
using UnityEngine;


public class SliderShooting : MonoBehaviour
{
    // Start is called before the first frame update

    //  GameObject laser;
    public GameObject slider_head_charge;
    public GameObject slider_control_head;
    public bool isShooting;

    [SerializeField] Material white;
    
    public SliderLoaderRecharge loader_recharge;


    [SerializeField] SliderLoaderFullAutoRecharge pivot_source_full_auto_recharge;

    [SerializeField] SliderLoaderBoltRecharge pivot_source_bolt_recharge;


    Vector3 laser_target;


    [SerializeField] float bullet_speed_full_auto, bullet_speed_bolt, firing_delay, bullet_life_time;
    [SerializeField] Vector3 bullet_size_full_auto, bullet_size_bolt;

    void Start()
    {
        OnMouseUp += CancelShooting;
        OnMouseDown += StartShooting;
        isShooting = false;



        PlayerInputSO.OnSliderBoltClick += (_) => loader_recharge = pivot_source_bolt_recharge;
        PlayerInputSO.OnSliderFullAutoClick += (_) => loader_recharge = pivot_source_full_auto_recharge;






        Debug.Log(laser_target);



    }

    // Update is called once per frame



    public event Action OnMouseDown;
    public event Action OnMouseUp;
    void Update()
    {
        laser_target = transform.up * -int.MaxValue;



        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {

                string tag = hit.transform.tag;
                if (tag == "SliderControlCollider" || tag == "SliderFullAutoCollider" || tag == "SliderBoltCollider")
                {
                    return;
                }
            }

            if (slider_control_head.GetComponent<SliderControlActivation>().active && !isShooting && !loader_recharge.IsRecharging && loader_recharge.IsActive)
            {
                OnMouseDown?.Invoke();
            }

        }
        

        if (Input.GetButtonUp("Fire1"))
        {

            OnMouseUp?.Invoke();
        }




    }


    void CancelShooting()
    {

        CancelMagazine();
        isShooting = false;

    }


    void StartShooting()
    {

        Shoot();
        isShooting = true;


    }










    GameObject bullet;
    public void Shoot()
    {

        StartCoroutine(EmptyMagazine());



    }

    IEnumerator EmptyMagazine()
    {



        switch (loader_recharge)
        {
            case SliderLoaderFullAutoRecharge auto:

                while (auto.ChangeIndex(-1) != -1)
                {
                    CreateBullet();

                    yield return new WaitForSeconds(firing_delay);
                }
                break;
            case SliderLoaderBoltRecharge bolt:
                bolt.Use();
                CreateBullet();
                break;







        }







    }




    public void CancelMagazine()
    {
        StopAllCoroutines();
    }

    void CreateBullet()
    {
        bullet = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        bullet.GetComponent<Renderer>().sharedMaterial = white;
        bullet.transform.localScale = (loader_recharge is SliderLoaderFullAutoRecharge) ? bullet_size_full_auto : bullet_size_bolt;


        Vector3 rotationDirection = (laser_target - transform.position);
        bullet.transform.up = rotationDirection;


        bullet.tag = "SliderBullet";

        bullet.AddComponent<Rigidbody>();
        bullet.GetComponent<Rigidbody>().useGravity = false;
        bullet.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        bullet.AddComponent<SliderBulletMovement>();
        bullet.GetComponent<SliderBulletMovement>().bullet_life_time = bullet_life_time;
        bullet.GetComponent<SliderBulletMovement>().origin = transform.position - transform.up * bullet.transform.localScale.y;
        bullet.GetComponent<SliderBulletMovement>().target = laser_target;
        bullet.GetComponent<SliderBulletMovement>().speed = (loader_recharge is SliderLoaderFullAutoRecharge) ? bullet_speed_full_auto : bullet_speed_bolt;
        bullet.AddComponent<SliderBulletCollision>();
        bullet.GetComponent<SliderBulletCollision>().DamagePotential = (loader_recharge is SliderLoaderFullAutoRecharge) ? (DifficultyManager.DISRUPTOR_DEFAULT_START_HEALTH / 10) : DifficultyManager.DISRUPTOR_DEFAULT_START_HEALTH;
    }

}

