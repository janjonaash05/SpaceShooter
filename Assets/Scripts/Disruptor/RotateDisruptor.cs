using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RotateDisruptor : MonoBehaviour, IEMPDisruptable
{
    

    


    /// <summary>
    /// <para>Calculates the direction as the normalized difference of the target and this transform's position.</para>
    /// <para>Gets the Quaternion rotation as LookRotation of the direction.</para>
    /// <para>Adjusts the rotation.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <returns>The adjusted rotation</returns>
    public Quaternion GetRotation(Vector3 target)
    {

        Vector3 rotationDirection = (target - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(rotationDirection);
        rot = Quaternion.Euler(rot.eulerAngles.x - 90, rot.eulerAngles.y, rot.eulerAngles.z + 90);


        return rot;
    }


   public void OnEMP() => StopAllCoroutines();

 

    public void EngageRotation(Vector3 target)
    {
        StartCoroutine(RotateTowards(target));
    }


    /// <summary>
    /// Gets target rotation as GetRotation with the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator RotateTowards(Vector3 target)
    {

        Quaternion targetRot = GetRotation(target);
        Quaternion startRot = transform.rotation;

        float duration = 0.35f;
        float lerp = 0;
        while (lerp <= duration)
        {
            lerp += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, lerp/duration);
            
            yield return null;


        }


    }



}
