using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    
    void Start()
    {

    }
    [SerializeField] Material white;
    [SerializeField] float min_scale_down_size, scale_down_increment;



    void CoverInWhite()
    {
       TryGetComponent(out StarFall b);
        Destroy(b);


        Material[] mats = new Material[GetComponent<Renderer>().materials.Length]; Array.Fill(mats, white);
        GetComponent<Renderer>().materials = mats;

       

    }






    public void Destroy()
    {




        AudioManager.PlayActivitySound(AudioManager.ActivityType.STAR_DESTROYED);
        Destroy(GetComponent<StarChargeUp>());
        Destroy(GetComponent<StarEmergence>());
        CoverInWhite();

        // ScoreCounter.Increase();

        UICommunication.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ScoreReward());


        GetComponent<IScoreEnumerable>().DisabledRewards = true;
        _ = ScaleDown();



        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }



    public void DestroyByEMP() 
    {
    
    
    }

    async Task ScaleDown()
    {



        while (transform.localScale.y > min_scale_down_size)
        {

            transform.localScale = new Vector3(transform.localScale.x - scale_down_increment, transform.localScale.y - scale_down_increment, transform.localScale.z - scale_down_increment);
            
            await Task.Yield();
        }

        Destroy(gameObject);






    }





}
