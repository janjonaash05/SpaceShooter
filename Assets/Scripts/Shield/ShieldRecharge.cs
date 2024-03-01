using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
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










    


    ParticleSystem ps;

    [SerializeField] GameObject shield_emitter, shield_emitter_antenna;
    Renderer emitter_rend, antenna_rend;

    Material on_mat, off_mat;
    void Start()
    {


        max_capacity = DifficultyManager.SHIELD_MAX_CAPACITY;


        charges = new();
        GenerateCharges();
        emitter_rend = shield_emitter.GetComponent<Renderer>();

        on_mat = emitter_rend.materials[2];
        off_mat = emitter_rend.materials[1];


        antenna_rend = shield_emitter_antenna.GetComponent<Renderer>();


        ps = transform.GetComponentInChildren<ParticleSystem>();



      

        CoreCommunication.OnBombFallen += (m) =>
        {



            if (recharging) return;

            Destroy(charges[capacity_index]);
            capacity_index++;
            




            
            Debug.LogError(charges.Count + "oncoll chargeslen, index increased to "+capacity_index + " shield capacity "+ CoreCommunication.SHIELD_CAPACITY);




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

      
     

    }



 
   
   




    void Recharge()
    {
        recharging = true;


        charges.Clear();
        capacity_index = 0;







        GenerateCharges();

        foreach (GameObject charge in charges)
        {
            charge.SetActive(false);

        }




        ps.enableEmission = true;
        ChangeEmitterAndAntennaColor(off_mat);
        StartCoroutine(recharge());



        IEnumerator recharge()
        {
            
            foreach (GameObject charge in charges.Reverse<GameObject>())
            {
                float recharge_delay = CoreCommunication.CORE_INDEX_HOLDER.Parent switch
                {
                    5 => 0.1f,
                    4 => 0.5f,
                    3 => 1f,
                    2 => 1.5f,
                    1 => 2f,
                    0 => float.NaN
                }; ;
                

                if (recharge_delay == float.NaN) { yield return null; }
                yield return new WaitForSeconds(recharge_delay);
                charge.SetActive(true);



               
            }

            recharging = false;
            CoreCommunication.Raise_ShieldRecharged();


            ps.enableEmission = false;
            ChangeEmitterAndAntennaColor(on_mat);
        }


        

    }



    // Update is called once per frame
    void Update()
    {

    }


    void ChangeEmitterAndAntennaColor(Material m)
    {
        var e_mats = emitter_rend.materials;
        e_mats[^1] = m;

        var a_mats = antenna_rend.materials;
        a_mats[^1] = m;

        emitter_rend.materials = e_mats;
        antenna_rend.materials = a_mats;

    }



}
