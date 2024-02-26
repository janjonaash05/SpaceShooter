using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldChargeRecharge : MonoBehaviour
{
    // Start is called before the first frame update


    bool recharging;


    float max_scale;


    float capacity;






    float recharge_delay = 0.2f;


    ParticleSystem ps;



    [SerializeField] GameObject shield_emitter, shield_emitter_antenna;
    Renderer emitter_rend, antenna_rend;


    Material emitter_on, emitter_off;

    void Start()
    {




        emitter_rend = shield_emitter.GetComponent<Renderer>();

        emitter_on = emitter_rend.materials[2];
        emitter_off = emitter_rend.materials[1];






        ps = transform.parent.GetChild(1).GetComponent<ParticleSystem>();


        float scale = transform.localScale.x;

        max_scale = scale;

        CoreCommunication.OnBombFallen += (m) =>
        {



            if (CoreCommunication.SHIELD_CAPACITY == 0) return;
            transform.localScale -= (scale / DifficultyManager.SHIELD_DEFAULT_CAPACITY) * Vector3.one;
        };




        CoreCommunication.OnShieldDepleted += () => { if (!recharging) Recharge(); };

    }




    void Recharge()
    {
        IEnumerator recharge()
        {
            recharging = true;
            while (transform.localScale.x <= max_scale)
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


                transform.localScale +=  10 * max_scale * recharge_rate * Time.deltaTime * Vector3.one;


                yield return new WaitForSeconds(recharge_delay);
            }

            transform.localScale = max_scale * Vector3.one;
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
