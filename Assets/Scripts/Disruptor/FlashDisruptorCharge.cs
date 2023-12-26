using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDisruptorCharge : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material white;
    float charge_up_flash_delay = 0.5f;
    float all_colors_flash_delay = 0.2f;


    void Start()
    {
        
    }


    private void Awake()
    {
        DisruptorColorChange.OnColorChange += FlashColorThenWhite;

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();
    }


    private void OnDestroy()
    {
        DisruptorColorChange.OnColorChange -= FlashColorThenWhite;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void FlashColorThenWhite(Material m)
    {

        IEnumerator flashColorThenWhite(Material m)
        {

            GetComponent<Renderer>().material = m;

            FlashParticles(m,false);
            yield return new WaitForSeconds(charge_up_flash_delay);
            GetComponent<Renderer>().material = white;


        }



        StartCoroutine(flashColorThenWhite(m));

    }




    public void FlashAllColors(Material[] ms)
    {


        IEnumerator flashALlColors(Material[] ms)
        {
            

            while (true)
            {
                foreach (var mat in ms)
                {
                    FlashParticles(mat,true);
                    GetComponent<Renderer>().material = mat;
                    yield return new WaitForSeconds(all_colors_flash_delay);
                    GetComponent<Renderer>().material = white;
                    yield return new WaitForSeconds(all_colors_flash_delay);
                }
            }







        }



        StartCoroutine(flashALlColors(ms));
    }



    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;

    void FlashParticles(Material m, bool fullArc) 
    {



        if (fullArc) { var shape = ps.shape; shape.arc = 360; }
        ps_rend.material = m;




        var emission = ps.emission;

        emission.enabled = true;



        ps.Play();


    
    }



    void PlayRepeatParticles() 
    {
        var emission = ps.emission;
        // emission.GetBurst(0).probability = 0;
        var shape = ps.shape;
        var main = ps.main;


        main.loop = true;

       

        emission.rateOverTime = 10;

        ps.Play();

    }



  



}
