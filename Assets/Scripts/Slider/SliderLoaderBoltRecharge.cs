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

        recharge_rate = UpgradesManager.GetCurrentSliderRechargeValue().bolt;





        charge = Instantiate(prefab, transform);
        OnFullRechargeInvoke();
        charge.GetComponent<Renderer>().material = charge.GetComponent<SliderLoaderChargeColorChange>().Off;
        init_y_scale = charge.transform.localScale.y;
    }



    /// <summary>
    /// Invokes OnDepletionInvoke, yields Drain and Recharge coroutines, invokes OnFullRechargeInvoke.
    /// </summary>
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

    /// <summary>
    /// Keeps decreasing the charge's localScale y value by drain_rate, until it reaches zero. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Drain()
    {

        while (charge.transform.localScale.y > 0)
        {
            charge.transform.localScale = new Vector3(charge.transform.localScale.x, charge.transform.localScale.y - drain_rate, charge.transform.localScale.z);
            yield return null;


        }



    }

    /// <summary>
    /// Keeps increasing the charge's localScale y value by recharge_rate, until it reaches initial value. Gets recharge_rate from the UpgradesManager.
    /// </summary>
    /// <returns></returns>
    IEnumerator Recharge()
    {
        while (charge.transform.localScale.y < init_y_scale)
        {

            recharge_rate = UpgradesManager.GetCurrentSliderRechargeValue().bolt;
            charge.transform.localScale = new Vector3(charge.transform.localScale.x, charge.transform.localScale.y + recharge_rate, charge.transform.localScale.z);
            yield return null;


        }
        charge.transform.localScale = new Vector3(charge.transform.localScale.x, init_y_scale, charge.transform.localScale.z);


    }


}
