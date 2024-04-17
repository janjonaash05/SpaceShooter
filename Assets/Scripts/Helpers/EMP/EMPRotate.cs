using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRotate : MonoBehaviour
{

    [SerializeField] float max_speed;
    
    void Start()
    {
        
    }


    private void Awake()
    {
        StartCoroutine(SpeedUpDown());
    }


    
    void Update()
    {
        transform.Rotate(rot_speed * Time.deltaTime);
    }



    Vector3 rot_speed = Vector3.zero;

    IEnumerator ChangeSpeedOverTime(Vector3 original, Vector3 target, float duration)
    {
        float counter = 0f;
        while (counter < duration)
        {

            counter += Time.deltaTime;

            rot_speed = Vector3.Lerp(original, target, counter / duration);

            yield return null;
        }
    }


    IEnumerator SpeedUpDown()
    {

        Vector3 target_speed = new Vector3(0, 0, 1f) * max_speed; ;


        yield return StartCoroutine(ChangeSpeedOverTime(Vector3.zero, target_speed, HelperSpawnerManager.LIFETIME / 2));
        yield return StartCoroutine(ChangeSpeedOverTime(target_speed, Vector3.zero, HelperSpawnerManager.LIFETIME / 2));

    }
}
