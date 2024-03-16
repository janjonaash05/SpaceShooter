using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPSColorChange : MonoBehaviour
{
    // Start is called before the first frame update



    
    void Start()
    {



        CoreCommunication.OnBombFallen += (m) => StartCoroutine(Play(m));
        






    }





    IEnumerator Play(Material m) 
    {
        float waitTime = transform.CompareTag(Tags.SHIELD_HEMISPHERE_PS) ? 0 : 0;

        ParticleSystem ps = GetComponent<ParticleSystem>();

        ParticleSystemRenderer ps_rend = ps.GetComponent<ParticleSystemRenderer>();

        ps_rend.material = m;
        ps_rend.trailMaterial = m;

        var emission = ps.emission;

        emission.enabled = true;

        yield return new WaitForSeconds(waitTime);

        ps.Play();


    }









    // Update is called once per frame
    void Update()
    {
        
    }
}
