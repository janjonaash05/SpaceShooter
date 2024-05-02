
using System;
using System.Collections;

using UnityEngine;


public class SliderShooting : MonoBehaviour
{
    

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



    private void OnDestroy()
    {
        PlayerInputCommunication.OnMouseDown -= StartShooting;
        PlayerInputCommunication.OnMouseUp -= CancelShooting;

        PlayerInputCommunication.OnSliderBoltClick -= SliderBoltClick;
        PlayerInputCommunication.OnSliderFullAutoClick -= SliderFullAutoClick;

        UpgradesManager.OnSliderSpeedValueChange -= SliderSpeedValueChange;

    }




    void SliderBoltClick(RaycastHit _) { loader_recharge = pivot_source_bolt_recharge; active_ps = bolt_ps; }
    void SliderFullAutoClick(RaycastHit _) { loader_recharge = pivot_source_full_auto_recharge; active_ps = full_auto_ps; }


    void SliderSpeedValueChange() => (bullet_speed_full_auto, bullet_speed_bolt)
        = UpgradesManager.SLIDER_SPEED_DEGREE_VALUE_DICT[UpgradesManager.UPGRADE_VALUE_DICT[UpgradesManager.UpgradeType.SLIDER_SPEED]];




    void Start()
    {
        

        bolt_ps = transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<ParticleSystem>();
        full_auto_ps = transform.parent.GetChild(transform.parent.childCount - 2).GetComponent<ParticleSystem>();


        white = MaterialHolder.Instance().SIDE_TOOLS_COLOR();
        PlayerInputCommunication.OnMouseDown += StartShooting;
        PlayerInputCommunication.OnMouseUp += CancelShooting;

        isShooting = false;


  
        PlayerInputCommunication.OnSliderBoltClick += SliderBoltClick;
        PlayerInputCommunication.OnSliderFullAutoClick += SliderFullAutoClick;





      (bullet_speed_full_auto, bullet_speed_bolt)
        = UpgradesManager.SLIDER_SPEED_DEGREE_VALUE_DICT[UpgradesManager.UPGRADE_VALUE_DICT[UpgradesManager.UpgradeType.SLIDER_SPEED]];



        UpgradesManager.OnSliderSpeedValueChange += SliderSpeedValueChange;








    }

    



    public event Action OnMouseDown;
    public event Action OnMouseUp;
    void Update()
    {
        laser_target = transform.up * -int.MaxValue;

    }

    /// <summary>
    /// If the particle system isn't null, disables its emission. Then calls CancelMagazine() and sets isShooting to false.
    /// </summary>
    void CancelShooting()
    {

        if (active_ps != null) { 
            active_ps.enableEmission = false;
        }
        CancelMagazine();
        isShooting = false;

    }


    /// <summary>
    /// If the loader_recharge is null, returns.
    /// <para>Also, if either the slider control head isn't active, isShooting, isRecharging, or loader recharge isn't active, returns. </para>
    /// <para>Enables emission and plays the particle system, calls Shoot() and sets isShooting to true.</para>
    /// </summary>
    void StartShooting()
    {
        if (loader_recharge == null) return;
       // if (!(slider_control_head.GetComponent<SliderControlActivation>().active && !isShooting && !loader_recharge.IsRecharging && loader_recharge.IsActive)) return;

        if (!(slider_control_head.GetComponent<SliderControlActivation>().active) || isShooting || loader_recharge.IsRecharging || !loader_recharge.IsActive) return;


        active_ps.enableEmission = true;
        active_ps.Play();

        Shoot();
        isShooting = true;


    }


    GameObject bullet;
    public void Shoot()
    {
        StartCoroutine(EmptyMagazine());
    }



    /// <summary>
    /// Based on the loader_recharge type:
    /// If SliderLoaderFullAutoRecharge:
    /// <para>- Calls ChangeIndex on auto recharge with -1 until reaching -1 (depleting), meanwhile calls CreateBullet, plays SLIDER_FULL_AUTO_SHOT sound and waits firing_delay.</para>
    /// <para>If SliderLoaderBoltRecharge:</para>
    /// <para>- Calls Use() on bolt recharge, calls CreateBullet() and plays the SLIDER_BOLT_SHOT sound. </para>
    /// </summary>
    /// <returns></returns>
    IEnumerator EmptyMagazine()
    {
        switch (loader_recharge)
        {
            case SliderLoaderFullAutoRecharge auto:

                while (auto.ChangeIndex(-1) != -1)
                {
                    CreateBullet();
                    AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_FULL_AUTO_SHOT);

                    yield return new WaitForSeconds(firing_delay);
                }
                CancelShooting();
                break;


            case SliderLoaderBoltRecharge bolt:
                bolt.Use();
                CreateBullet();
                AudioManager.PlayActivitySound(AudioManager.ActivityType.SLIDER_BOLT_SHOT);
                break;
        }
    }




    public void CancelMagazine()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// <para>Create a capsule gameObject, assigns its material and scale based on loader_recharge type.</para>
    /// <para>Aligns its direction tith the laser_target, sets its tag and layer.</para>
    /// <para>Adds a RigidBody component, disables its gravity and sets its collision detection mode to ContinuousDynamic.</para>
    /// <para>Adds a SliderBulletMovement, sets it origin infront of the turret, sets its target, sets its speed based on loader_recharge type. </para>
    /// <para>Adds a SliderBulletCollision component, sets its damage potential based on loader_recharge type.</para>
    /// </summary>
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

