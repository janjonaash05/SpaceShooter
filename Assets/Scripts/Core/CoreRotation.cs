using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRotation : MonoBehaviour
{
    // Start is called before the first frame update


    Dictionary<int, Vector3> speed_parent_dict = new()
    {
        {5,new(0,0,0)},
        {4,new(0,50f,0) },
        {3,new(0, 100f, 0) },
        {2,new(0,150f,150f)},
        {1,new(0,200f,200f) },
        {0,new(250f,250f,250f) }






    };


    Vector3 speed;



    Material default_color1;

    Material default_color2;



    Renderer rend;

    void Start()
    {
        speed = speed_parent_dict[5];

        CoreCommunication.OnParentValueChangedCore += () =>
        {
            speed = speed_parent_dict[CoreCommunication.CORE_INDEX_HOLDER.Parent];


            if (CoreCommunication.CORE_INDEX_HOLDER.Parent == 0)
            {
                rend.materials = new Material[] { GetComponent<CoreRingColorChange>().changing_mat, GetComponent<CoreRingColorChange>().changing_mat};

            }
            else { rend.materials = new Material[] { default_color1, default_color2 }; }




        };




        rend = GetComponent<Renderer>();

        default_color1 = rend.materials[0];
        default_color2 = rend.materials[1];




    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-speed * Time.deltaTime);



    }
}
