using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLoaderBoltRecharge : SliderLoaderRecharge
{
    [SerializeField] GameObject prefab;


    GameObject charge;
    [SerializeField] float recharge_rate, drain_rate;






    void Start()
    {
        PlayerInputCommunication.OnSliderBoltClick += (_) => OnActivationInvoke();
        PlayerInputCommunication.OnSliderFullAutoClick += (_) => OnDeactivationInvoke();






        charge = Instantiate(prefab, transform);
        OnFullRechargeInvoke();
        charge.GetComponent<Renderer>().material = charge.GetComponent<SliderLoaderChargeColorChange>().Off;
        init_y_scale = charge.transform.localScale.y;
    }




    public void Use()
    {

        IEnumerator ExecutionOrder()
        {
            OnDepletionInvoke();
            yield return StartCoroutine(Drain());
            yield return StartCoroutine(Recharge());
            OnFullRechargeInvoke();
        }

        StartCoroutine(ExecutionOrder());


    }


    float init_y_scale;
    IEnumerator Drain()
    {

        while (charge.transform.localScale.y > 0)
        {
            charge.transform.localScale = new Vector3(charge.transform.localScale.x, charge.transform.localScale.y - drain_rate, charge.transform.localScale.z);
            yield return null;


        }



    }

    IEnumerator Recharge()
    {
        while (charge.transform.localScale.y < init_y_scale)
        {
            charge.transform.localScale = new Vector3(charge.transform.localScale.x, charge.transform.localScale.y + recharge_rate, charge.transform.localScale.z);
            yield return null;


        }
        charge.transform.localScale = charge.transform.localScale = new Vector3(charge.transform.localScale.x, init_y_scale, charge.transform.localScale.z);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
