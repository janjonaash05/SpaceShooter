
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

    Material white;

    public SliderLoaderRecharge loader_recharge;


    [SerializeField] SliderLoaderFullAutoRecharge pivot_source_full_auto_recharge;

    [SerializeField] SliderLoaderBoltRecharge pivot_source_bolt_recharge;


    Vector3 laser_target;


    [SerializeField] float bullet_speed_full_auto, bullet_speed_bolt, firing_delay, bullet_life_time;
    [SerializeField] Vector3 bullet_size_full_auto, bullet_size_bolt;




    ParticleSystem full_auto_ps;
    ParticleSystem bolt_ps;

    ParticleSystem active_ps;

    void Start()
    {
        

        bolt_ps = transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<ParticleSystem>();
        full_auto_ps = transform.parent.GetChild(transform.parent.childCount - 2).GetComponent<ParticleSystem>();


        white = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        PlayerInputCommunication.OnMouseDown += StartShooting;
        PlayerInputCommunication.OnMouseUp += CancelShooting;

        isShooting = false;



        PlayerInputCommunication.OnSliderBoltClick += (_) => { loader_recharge = pivot_source_bolt_recharge; active_ps = bolt_ps; };
        PlayerInputCommunication.OnSliderFullAutoClick += (_) => { loader_recharge = pivot_source_full_auto_recharge; active_ps = full_auto_ps; };



        (bullet_speed_full_auto, bullet_speed_bolt)
        = UpgradesManager.SLIDER_SPEED_DEGREE_VALUE_DICT[UpgradesManager.UPGRADE_VALUE_DICT[UpgradesManager.UpgradeType.SLIDER_SPEED]];


        UpgradesManager.OnSliderSpeedValueChange += () => 
        (bullet_speed_full_auto, bullet_speed_bolt) 
        = UpgradesManager.SLIDER_SPEED_DEGREE_VALUE_DICT[UpgradesManager.UPGRADE_VALUE_DICT[UpgradesManager.UpgradeType.SLIDER_SPEED]];






        //  Debug.Log(laser_target);



    }

    // Update is called once per frame



    public event Action OnMouseDown;
    public event Action OnMouseUp;
    void Update()
    {
        laser_target = transform.up * -int.MaxValue;

    }


    void CancelShooting()
    {

        if (active_ps != null) { 
            active_ps.enableEmission = false;
        }
        Debug.Log("CancelShooting");
        CancelMagazine();
        isShooting = false;

    }


    void PlayPS()
    {
    }

    void StartShooting()
    {
        if (loader_recharge == null) return;
        if (!(slider_control_head.GetComponent<SliderControlActivation>().active && !isShooting && !loader_recharge.IsRecharging && loader_recharge.IsActive)) return;



        active_ps.enableEmission = true;
        active_ps.Play();

        Debug.Log("StartShooting");
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
                CancelShooting();
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
        bullet.layer = 2;


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

