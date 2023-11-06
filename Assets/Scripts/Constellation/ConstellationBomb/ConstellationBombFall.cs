using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ConstellationBombFall : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] ParticleSystem trail_particle_system_prefab;
    [SerializeField] float move_speed;
    [SerializeField] Vector3 rotation;
    Vector3 target;

    Rigidbody rb;
    GameObject spinner;



    bool can_move = false;




    List<Material> trail_colors;
    List<ParticleSystem> trails;



    float trail_rate_over_distance;

    void Start()
    {


        trail_rate_over_distance = trail_particle_system_prefab.emission.rateOverDistanceMultiplier;




        rb = GetComponent<Rigidbody>();
        FormConstellation.OnAllStarsGone += () =>can_move = true;
        FormConstellation.OnAllStarsGone += CreateTrails;


        spinner = GameObject.FindGameObjectWithTag(Tags.SPINNER);
        target = spinner.transform.position;
        
    }

    // Update is called once per frame



    private void OnDestroy()
    {
        FormConstellation.OnAllStarsGone -= () => can_move = true;
        FormConstellation.OnAllStarsGone -= CreateTrails;
    }


    void CreateTrails() 
    {
        trail_colors = GetComponent<ConstellationBombColorChange>().Colors;

        trails = new();

        foreach(Material trailmat in trail_colors) 
        {
            var trail = Instantiate(trail_particle_system_prefab,transform.position, transform.rotation);
          
            
            trail.GetComponent<ParticleSystemRenderer>().material = trailmat;


            
            var em = trail.emission;
           
            em.enabled = true;

            var main = trail.main;
            main.startSize = transform.localScale.x / 10;




            trails.Add(trail);


        
        
        }


    }

    bool initEmissionSetOver = false;

    private void FixedUpdate()
    {

        if (!can_move) return;





        foreach (ParticleSystem ps in trails) 
        {
            ps.transform.position = transform.position;
            Vector3 rotationDirection = -(target - rb.position);
            ps.transform.rotation = Quaternion.LookRotation(rotationDirection);

           



        }

        initEmissionSetOver = true;
        rb.AddForce(Time.fixedDeltaTime * move_speed * (target - rb.position), ForceMode.VelocityChange);
        
      
        
    }





 



    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.SPINNER))
        {

            can_move = false;


            StopAllCoroutines();
            GetComponent<ConstellationBombColorChange>().StopColorChange();

            Material final_color = GetComponent<Renderer>().materials[BombColorChange.COLOR_INDEX];


            Debug.Log(col);
            Destroy(gameObject.GetComponent<ConstellationBombFall>());



            GetComponent<ConstellationBombColorChange>().ChangeOnlyColor(final_color);
            _ = gameObject.GetComponent<DamageConstellationBomb>().StartDamage(false);


            ConstellationBombChargeUp chargeUp = GetComponent<ConstellationBombChargeUp>();


            (int p, int c) vals = chargeUp.scale_degree switch
            {
                >= 0 and <=1 => (0,1),
                >= 2 and <=4 => (0,2),
                >=5 and <=7 =>(1,0),
                8 => (2,0),
                _ =>(0,0)

            };



            


            spinner.GetComponent<SpinnerColorChange>().ChangeIndexHolder(vals.p, vals.c);
        }
    }
}
