using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ConstellationBombFall : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float move_speed;
    [SerializeField] Vector3 rotation;
    Vector3 target;

    Rigidbody rb;
    GameObject spinner;



    bool can_move = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FormConstellation.OnAllStarsGone += () =>can_move = true;


        spinner = GameObject.FindGameObjectWithTag(Tags.SPINNER);
        target = spinner.transform.position;
    }

    // Update is called once per frame




    private void FixedUpdate()
    {

        if (!can_move) return;
        rb.MovePosition(rb.position + Time.fixedDeltaTime * move_speed * (target - rb.position));
    }





 



    private void OnCollisionEnter(Collision col)
    {

        if (col.transform.CompareTag(Tags.SPINNER))
        {

            can_move = false;


            StopAllCoroutines();
            GetComponent<ConstellationBombColorChange>().StopColorChange();

            Material final_color = GetComponent<Renderer>().materials[BombColorChange.COLOR_INDEX];


            Debug.Log(col);
            Destroy(gameObject.GetComponent<ConstellationBombFall>());



            GetComponent<ConstellationBombColorChange>().ChangeOnlyColor(final_color);
            _ = gameObject.GetComponent<DamageConstellationBomb>().StartDamage(false);


            ConstellationBombChargeUp chargeUp = GetComponent<ConstellationBombChargeUp>();


            (int p, int c) vals = chargeUp.scale_degree switch
            {
                >= 0 and <=1 => (0,1),
                >= 2 and <=4 => (0,3),
                >=5 and <=7 =>(1,0),
                8 => (2,0),
                _ =>(0,0)

            };



            


            spinner.GetComponent<SpinnerColorChange>().ChangeIndexHolder(vals.p, vals.c);
        }
    }
}
