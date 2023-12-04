using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class DamageBomb : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float min_scale_down_size, scale_down_increment;
    GameObject bomb;

    public bool DisabledRewards { get; set; }
    CancellationTokenSource cancel_source;
  protected  CancellationToken token;
    void Start()
    {
        bomb = gameObject;



         cancel_source = new();
         token= cancel_source.Token;

    }

    // Update is called once per frame

    private void OnDestroy()
    {
        cancel_source.Cancel();
        cancel_source.Dispose();
    }


    public virtual async Task StartDamage(bool playerTargeted)
    {







        Destroy(GetComponent<Collider>());


        Destroy(bomb.GetComponent<BombFall>());
        

        await GetComponent<BombColorChange>().CoverInColor();


        Action damage = (playerTargeted) ? DamageByPlayer : DamageByCore;

        damage();


     
        /*
        ScaleDown(token);

        
        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
        */
    }





    void DamageByCore()
    {



        ParticleSystem ps = transform.GetChild(1).GetComponent<ParticleSystem>();
        

        Vector3 target = GameObject.FindGameObjectWithTag(Tags.CORE).transform.position;
    //    Debug.LogWarning(target + "core " + transform.position + " bmb");



        Vector3 rotationDirection = (target - transform.position);

        Debug.LogWarning(rotationDirection + " rot");


        
       transform.rotation = Quaternion.LookRotation(rotationDirection);
        
     
       

        Debug.DrawRay(transform.position, rotationDirection, Color.red, 1f);





       ps.enableEmission = true;
       ps.Play();

        _ = ScaleDown(token);
        Destroy(gameObject, transform.GetChild(1).GetComponent<ParticleSystem>().main.duration);

        
    }


     void DamageByPlayer() 
    {

        Destroy(GetComponent<Renderer>());

        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }


    
  protected async Task ScaleDown(CancellationToken token)
    {

        while (bomb.transform.localScale.x > min_scale_down_size)
        {
            if (token.IsCancellationRequested) { break; }
            bomb.transform.localScale += (-scale_down_increment * Vector3.one);
            
            
            await Task.Yield();
        }


        



    }


}



