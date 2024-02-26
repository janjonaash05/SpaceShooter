using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRecharge : MonoBehaviour
{

    [SerializeField] int capacity;
    [SerializeField] GameObject charge_prefab;

    List<GameObject> charges;



   
    
    void Start()
    {


        charges = new();
        
    }



    void GenerateCharges()
    {
        float margin = capacity - 1;

        float startSize = charge_prefab.transform.localScale.x;



        for (int i =  0; i < capacity; i++) 
        {
            GameObject charge = Instantiate(charge_prefab,transform,false);

            float size = (startSize / capacity + margin);
            charge.transform.localScale = new Vector3(startSize, startSize, size); ;

            charge.transform.position = new Vector3(0, 0, (size + margin) * i);
            
            


        
        }
    
    }






    // Update is called once per frame
    void Update()
    {
        
    }
}
