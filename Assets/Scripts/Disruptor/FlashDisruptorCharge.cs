using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDisruptorCharge : MonoBehaviour
{
    
    [SerializeField] Material white;
    readonly float charge_up_flash_delay = 0.5f;
    readonly float all_colors_flash_delay = 0.2f;


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



    /// <summary>
    /// Sets the material to m, plays DISRUPTOR_CHARGE_UP sound, calls FlashParticles(), waits a set amount of time and sets the material to white.
    /// </summary>
    /// <param name="m"></param>
    public void FlashColorThenWhite(Material m)
    {

        IEnumerator flashColorThenWhite(Material m)
        {

            GetComponent<Renderer>().material = m;
            AudioManager.PlayActivitySound(AudioManager.ActivityType.DISRUPTOR_CHARGE_UP);
            FlashParticles(m, false);
            yield return new WaitForSeconds(charge_up_flash_delay);
            GetComponent<Renderer>().material = white;


        }



        StartCoroutine(flashColorThenWhite(m));

    }



    /// <summary>
    /// <para>For each color, in an infinite loop:</para>
    /// Flashes particles, sets the material to color, plays DISRUPTOR_CHARGE_UP sound, waits set amount of time, sets the material back to white and waits again.
    /// </summary>
    /// <param name="ms"></param>
    public void FlashAllColors(Material[] ms)
    {


        IEnumerator flashALlColors(Material[] ms)
        {


            while (true)
            {
                foreach (var mat in ms)
                {
                    FlashParticles(mat, true);
                    GetComponent<Renderer>().material = mat;
                    AudioManager.PlayActivitySound(AudioManager.ActivityType.DISRUPTOR_CHARGE_UP);
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
    /// <summary>
    /// If fullArc is true, the changes the particle system settings and sets it to loop, finally sets its material to m and plays it.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="fullArc"></param>
    void FlashParticles(Material m, bool fullArc)
    {


        var emission = ps.emission;
        if (fullArc)
        {
            var burst = emission.GetBurst(0); burst.probability = 0;

            var shape = ps.shape;
            var main = ps.main;


            main.loop = true;



            emission.rateOverTime = 2; shape.arc = 360;
        }
        ps_rend.material = m;

        emission.enabled = true;



        ps.Play();



    }










}
