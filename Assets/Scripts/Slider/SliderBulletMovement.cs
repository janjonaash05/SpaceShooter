using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBulletMovement : MonoBehaviour
{
    public Vector3 target;
    public Vector3 origin;
    public float bullet_life_time;
   
    public float speed;


    float time = 0;
    void Start()
    {
        transform.position = origin;
    }

    
    void Update()
    {
        time += Time.deltaTime;
        origin = transform.position;
        transform.position = Vector3.MoveTowards(origin, target, speed * Time.deltaTime);
       




        if (time > bullet_life_time) {
            Destroy(gameObject);
            Debug.Log("removing bullet");
        }
    }
}
