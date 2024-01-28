using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static MaterialIndexHolder;
using static UnityEngine.GraphicsBuffer;

public class DamageBomb : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float min_scale_down_size, scale_down_increment;
    GameObject bomb;

    public bool DisabledRewards { get; set; }
    CancellationTokenSource cancel_source;
    protected CancellationToken token;
    void Start()
    {
        bomb = gameObject;



        cancel_source = new();
        token = cancel_source.Token;

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
        Destroy(GetComponent<BombFall>());
        await GetComponent<BombColorChange>().CoverInColor();


        Action damage = (playerTargeted) ? DamageByPlayer : DamageByTarget;

        damage();

    }





    void DamageByTarget()
    {






        Vector3 target = GameObject.FindGameObjectWithTag(Tags.BOMB_TARGET).transform.position;

        Vector3 rotationDirection = (target - transform.position);

        transform.rotation = Quaternion.LookRotation(rotationDirection);

        //  Debug.DrawRay(transform.position, rotationDirection, Color.red, 1f);

        _ = ScaleDown(token);
        //  StartCoroutine(DamageByCore_FallIntoCore());


        Destroy(gameObject, transform.GetChild(1).GetComponent<ParticleSystem>().main.duration);


    }


    void DamageByPlayer()
    {

        Destroy(GetComponent<Renderer>());

        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }




    /*

    IEnumerator DamageByCore_FallIntoCore()
    {


        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 target = GameObject.FindGameObjectWithTag(Tags.CORE).transform.position;



        while (Vector3.Distance(rb.position,target) > 0.1)
        {
            rb.MovePosition(rb.position + (target - rb.position) * Time.fixedDeltaTime * 0.1f);
            yield return new WaitForFixedUpdate();

        }
    }
    */



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



