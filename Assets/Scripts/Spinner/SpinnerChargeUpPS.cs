using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SpinnerChargeUpPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpinnerColorChange.OnMaterialChange += OnMaterialChange;

        CoreCommunication.OnSpinnerChargeUpStart += SpinnerChargeUpStart;
        CoreCommunication.OnSpinnerChargeUpEnd += SpinnerChargeUpEnd;





        ps = GetComponent<ParticleSystem>();

        ps_rend = ps.GetComponent<ParticleSystemRenderer>();

    }

    private void OnDestroy()
    {
        SpinnerColorChange.OnMaterialChange -= OnMaterialChange;

        CoreCommunication.OnSpinnerChargeUpStart -= SpinnerChargeUpStart;
        CoreCommunication.OnSpinnerChargeUpEnd -= SpinnerChargeUpEnd;
    }





    void SpinnerChargeUpStart() 
    {
        StartCoroutine(ScaleUp());
    
    }
    void SpinnerChargeUpEnd() 
    {
        StopAllCoroutines();

        var main = ps.main;
        var emission = ps.emission;
        var shape = ps.shape;
        emission.enabled = false;




        main.startSpeed = speed_interval.min;
        shape.radius = radius_interval.min;
        emission.rateOverTime = rate_interval.min;


    } 



    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;




    void OnMaterialChange(Material m) 
    {
        ps_rend.material = m;

    }

    (float min, float max) radius_interval = (55, 165);

    (float min, float max) speed_interval = (-5, -150);

    (float min, float max) rate_interval = (10, 100);
    IEnumerator ScaleUp() 
    {
        var main = ps.main;
        var emission = ps.emission;
        var shape = ps.shape;
        emission.enabled = true;




        float duration = SpinnerChargeUp.CHARGE_UP_TIME;




      

        float lerp = 0;

        while (lerp < duration)
        {



            lerp += Time.deltaTime;
            



            main.startSpeed = Mathf.Lerp(speed_interval.min, speed_interval.max, lerp / duration);
            shape.radius = Mathf.Lerp(radius_interval.min, radius_interval.max, lerp / duration);
            emission.rateOverTime = Mathf.Lerp(rate_interval.min, rate_interval.max, lerp / duration);
            yield return null;

        }


        emission.enabled = false;



    }







    // Update is called once per frame
    void Update()
    {
        
    }
}
