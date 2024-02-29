using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShieldRecharge : MonoBehaviour
{

    [SerializeField] int max_capacity;

    int capacity_index = 0;

    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;


    const float POSITION_FOR_SIZE_DIVIDER = 100;


    bool recharging;










    float recharge_delay = 0.2f;


    ParticleSystem ps;



    [SerializeField] GameObject shield_emitter, shield_emitter_antenna;
    Renderer emitter_rend, antenna_rend;


    Material emitter_on, emitter_off;
    void Start()
    {


        max_capacity = DifficultyManager.SHIELD_MAX_CAPACITY;


        charges = new();
        
        GenerateCharges();

        emitter_rend = shield_emitter.GetComponent<Renderer>();

        emitter_on = emitter_rend.materials[2];
        emitter_off = emitter_rend.materials[1];





        ps = transform.GetComponentInChildren<ParticleSystem>();



      

        CoreCommunication.OnBombFallen += (m) =>
        {



            if (CoreCommunication.SHIELD_CAPACITY == 0 || recharging) return;
            charges[capacity_index].SetActive(false);
            capacity_index++;



        };




        CoreCommunication.OnShieldDepleted += () => { if (!recharging) Recharge(); };

    }


    void GenerateCharges()
    {

        max_capacity = DifficultyManager.SHIELD_MAX_CAPACITY;


        float startSize = charge_prefab.transform.localScale.x;
        float size = startSize / max_capacity;
        float positionUnit = size / (POSITION_FOR_SIZE_DIVIDER);

        float scaled_size = startSize / (max_capacity + (positionUnit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

        if (max_capacity % 2 == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 0; j < max_capacity / 2; j++)
                {
                    GameObject charge = Instantiate(charge_prefab, transform, false);
                    float pos = positionUnit + positionUnit * 2 * j;


                    charge.transform.localPosition = new(0, pos * mult, 0);
                    charge.transform.localScale = new(startSize, scaled_size, scaled_size);
                }

            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                float mult = i == 0 ? 1 : -1;
                for (int j = 1; j <= (max_capacity - 1) / 2; j++)
                {
                    GameObject charge = Instantiate(charge_prefab, transform, false);
                    float pos = positionUnit * 2 * j;

                    charge.transform.localPosition = new(0, pos * mult, 0);
                    charge.transform.localScale = new(startSize, scaled_size, scaled_size);

                }
            }

            GameObject charge_center = Instantiate(charge_prefab, transform, false);


            charge_center.transform.localPosition = Vector3.zero;
            charge_center.transform.localScale = new(startSize, scaled_size, scaled_size);
        }




        foreach (Transform child in transform)
        {
            if (child.CompareTag(Tags.SHIELD_CHARGE))
            {
                charges.Add(child.gameObject);

            }

        }


        charges = charges.OrderBy(x => x.transform.localPosition.y).ToList();

      
     

        Debug.LogError("charges le" + charges.Count);
    }



 

   




    void Recharge()
    {
        foreach (GameObject charge in charges) 
        {
            charge.SetActive(true);
            Destroy(charge);
        
        }
        charges.Clear();







        Debug.LogError("charges le after clear" + charges.Count);
        GenerateCharges();
        Debug.LogError("childcount " + transform.childCount);

        foreach (GameObject charge in charges)
        {
            charge.SetActive(false);

        }








        IEnumerator recharge()
        {
            recharging = true;
            foreach (GameObject charge in charges)
            {
                float recharge_rate = CoreCommunication.CORE_INDEX_HOLDER.Parent switch
                {
                    5 => 0.5f,
                    4 => 0.35f,
                    3 => 0.25f,
                    2 => 0.1f,
                    1 => 0.075f,
                    0 => 0f
                }; ;

                charge.SetActive(true);






                yield return new WaitForSeconds(recharge_delay);
            }

            recharging = false;
            CoreCommunication.Raise_ShieldRecharged();


            ps.enableEmission = false;
            ChangeEmitterColor(emitter_on);
        }


        ps.enableEmission = true;
        ChangeEmitterColor(emitter_off);
        StartCoroutine(recharge());

    }



    // Update is called once per frame
    void Update()
    {

    }


    void ChangeEmitterColor(Material m)
    {
        var mats = emitter_rend.materials;

        mats[^1] = m;

        emitter_rend.materials = mats;

    }



}
