using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CoreRotation : MonoBehaviour
{
    


    Dictionary<int, Vector3> speed_parent_dict = new()
    {
        {0,new(0,0,0)},
        {1,new(0,50f,0) },
        {2,new(0, 100f, 0) },
        {3,new(0,150f,150f)},
        {4,new(0,200f,200f) },
        {5,new(250f,250f,250f) }






    };


    Dictionary<int, Action> ps_parent_dict;


    Vector3 speed;



    Material default_color1;

    Material default_color2;



    Renderer rend;



    Material changing_mat;


    ParticleSystem ps;
    ParticleSystemRenderer ps_rend;
    ParticleSystem.EmissionModule ps_emission;





    Material[] mats_storage;

    bool disabled = false;







    void Start()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath += OnLaserShotPlayerDeath;


        mats_storage = MaterialHolder.Instance().PLAYER_HEALTH_SET();

        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps_rend = ps.GetComponent<ParticleSystemRenderer>();
        ps_emission = ps.emission;

        ps_parent_dict = new()
        {
            {0, () =>ps_emission.enabled = false },
            {1, ()=> ps_emission.enabled = false },
            {2, ()=> {ps_emission.enabled = true; ps_emission.rateOverTime = 10;  } },
            {3, ()=> {ps_emission.enabled = true; ps_emission.rateOverTime = 50; }  },
            {4, ()=> {ps_emission.enabled = true; ps_emission.rateOverTime = 100; }  },
            {5, ()=> {ps_emission.enabled = true; ps_emission.rateOverTime = 500;   }  }





        };





        speed = speed_parent_dict[5];

        CoreRingColorChange.OnMaterialChange += MaterialChange;
        CoreCommunication.OnParentValueChangedCore += OnParentValueChangedCore;

        changing_mat = MaterialHolder.Instance().PLAYER_HEALTH_SET()[0];

        rend = GetComponent<Renderer>();

        default_color1 = rend.materials[0];
        default_color2 = rend.materials[1];




    }



    void MaterialChange(Material m) => changing_mat = m;

    void OnParentValueChangedCore() { speed = speed_parent_dict[CoreCommunication.CORE_INDEX_HOLDER.Parent]; ps_parent_dict[CoreCommunication.CORE_INDEX_HOLDER.Parent](); }


    void OnLaserShotPlayerDeath() => disabled = true;

    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= OnLaserShotPlayerDeath;
        CoreRingColorChange.OnMaterialChange -= MaterialChange;
        CoreCommunication.OnParentValueChangedCore -= OnParentValueChangedCore;
    }




    /// <summary>
    /// If diabled, returns. Otherwise rotates the transform, sets the materials based on the core index holder parent value, sets the particle system materials to the changing mat.
    /// </summary>
    void Update()
    {
        if (disabled) return;
        transform.Rotate(-speed * Time.deltaTime);

        rend.materials = CoreCommunication.CORE_INDEX_HOLDER.Parent switch
        {
            >= 4 => new Material[] { changing_mat, changing_mat },
            > 0 => new Material[] { default_color1, changing_mat },
            _ => new Material[] { default_color1, default_color2 }



        };

        ps_rend.material = changing_mat;
        ps_rend.trailMaterial = changing_mat;



    }






}
