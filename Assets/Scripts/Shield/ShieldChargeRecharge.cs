using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldChargeRecharge : MonoBehaviour
{
    // Start is called before the first frame update


    bool recharging;


    float max_scale;




    float recharge_delay = 0.2f;


    ParticleSystem ps;



    [SerializeField] GameObject shield_emitter;


    void Start()
    {



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
                    5 => 0.05f,
                    4 => 0.035f,
                    3 => 0.025f,
                    2 => 0.01f,
                    1 => 0.0075f,
                    0 => 0f
                }; ;


                transform.localScale += (max_scale * recharge_rate) * Vector3.one;


                yield return new WaitForSeconds(recharge_delay);
            }

            transform.localScale = max_scale * Vector3.one;
            recharging = false;
            CoreCommunication.Raise_ShieldRecharged();


            ps.enableEmission = false;
        }


        ps.enableEmission = true;
        StartCoroutine(recharge());
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
