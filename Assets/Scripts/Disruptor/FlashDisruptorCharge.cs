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

    // Update is called once per frame
    void Update()
    {

    }


    public void FlashColorThenWhite(Material m)
    {

        IEnumerator flashColorThenWhite(Material m)
        {

            GetComponent<Renderer>().material = m;
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
                    GetComponent<Renderer>().material = mat;
                    yield return new WaitForSeconds(all_colors_flash_delay);
                    GetComponent<Renderer>().material = white;
                    yield return new WaitForSeconds(all_colors_flash_delay);
                }
            }







        }



        StartCoroutine(flashALlColors(ms));
    }


    





}
