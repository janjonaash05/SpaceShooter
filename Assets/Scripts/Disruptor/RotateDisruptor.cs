using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RotateDisruptor : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float lerp_speed;

    public Quaternion GetRotation(Vector3 target)
    {

        Vector3 rotationDirection = (target - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        rot = Quaternion.Euler(rot.eulerAngles.x - 90, rot.eulerAngles.y, rot.eulerAngles.z + 90);


        return rot;
    }



    private void OnDestroy()
    {
        StopAllCoroutines();
    }


    public void EngageRotation(Vector3 target)
    {


        RotateTowards(target);
    }



    public IEnumerator  RotateTowards(Vector3 target)
    {

        Quaternion targetRot = GetRotation(target);

        float lerp = 0;
        while (lerp <= 1)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, lerp);
            lerp += Time.deltaTime * lerp_speed;
            yield return null;


        }


    }



}
