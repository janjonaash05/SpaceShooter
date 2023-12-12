using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRingRotation : MonoBehaviour
{







    Dictionary<int, float> speed_parent_dict = new()
    {
        {5,200},
        {4,100 },
        {3,75 },
        {2,50},
        {1,25 },
        {0,0f }
    
    
    
    
    
    
    };



    float speed;


    void Start()
    {

        speed = speed_parent_dict[5];

        CoreCommunication.OnParentValueChangedCore += () => { speed = speed_parent_dict[CoreCommunication.CORE_INDEX_HOLDER.Parent]; };


      
    }


    void Update()
    {
        transform.Rotate(0, 0, speed*Time.deltaTime);
    }



 
}
