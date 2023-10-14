using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DestroyStar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    [SerializeField] Material white;
    [SerializeField] float min_scale_down_size, scale_down_increment;



    void CoverInWhite()
    {
        TryGetComponent(out StarFallIntoBomb b);
        Destroy(b);


        Material[] mats = new Material[GetComponent<Renderer>().materials.Length]; Array.Fill(mats, white);
        GetComponent<Renderer>().materials = mats;



    }






    public void Destroy()
    {
        Destroy(GetComponent<StarChargeUp>());
        Destroy(GetComponent<StarEmergence>());
        CoverInWhite();

        // ScoreCounter.Increase();

        UICommunicationSO.Raise_ScoreChange(GetComponent<IScoreEnumerable>().ScoreReward());


        GetComponent<IScoreEnumerable>().DisabledRewards = true;
        ScaleDown();



        transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        Destroy(gameObject, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);

    }





    async void ScaleDown()
    {



        while (transform.localScale.y > min_scale_down_size)
        {

            transform.localScale = new Vector3(transform.localScale.x - scale_down_increment, transform.localScale.y - scale_down_increment, transform.localScale.z - scale_down_increment);
            
            await Task.Yield();
        }

        Debug.LogWarning("done shrinkin");
        Destroy(gameObject);






    }





}
