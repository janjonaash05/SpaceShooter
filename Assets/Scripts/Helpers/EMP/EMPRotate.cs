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


    /// <summary>
    /// LERPs rotation speed over a duration.
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator ChangeSpeedOverTime(Vector3 original, Vector3 target, float duration)
    {
        float lerp = 0f;
        while (lerp < duration)
        {

            lerp += Time.deltaTime;

            rot_speed = Vector3.Lerp(original, target, lerp / duration);

            yield return null;
        }
    }



    /// <summary>
    /// Yields ChangeSpeedOverTime twice, from 0 to target, then from target to 0.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpeedUpDown()
    {

        Vector3 target_speed = new Vector3(0, 0, 1f) * max_speed; ;


        yield return StartCoroutine(ChangeSpeedOverTime(Vector3.zero, target_speed, HelperSpawnerManager.LIFETIME / 2));
        yield return StartCoroutine(ChangeSpeedOverTime(target_speed, Vector3.zero, HelperSpawnerManager.LIFETIME / 2));

    }
}
