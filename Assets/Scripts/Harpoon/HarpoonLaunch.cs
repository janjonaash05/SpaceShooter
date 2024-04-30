using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonLaunch : MonoBehaviour
{





    GameObject harpoon_head, harpoon_station_charge;

    [SerializeField] GameObject charge;
    [SerializeField] HarpoonGrab grab;
    [SerializeField] float launch_distance;
    [SerializeField] float launch_speed;
    Material charge_color;
    Material off_color_head, off_color_charge;




    GameObject laser_tether;
    Transform harpoon_head_transform;
    Vector3 target;
    Vector3 start;
    bool readyToLaunch = true;


    bool turnedOff = true;

    bool successfulGrab;




    /// <summary>
    /// <para>Breaks if readyToLaunch is false.</para>
    /// <para>Sets readyToLaunch to false, assigns the target and start positions.</para>
    /// <para>Calls SetupTether() and yields the LaunchProcess coroutine.</para>
    /// <para>If successfulGrab is true, then sets readyToLaunch to true and successfulGrab to false.</para>
    /// <para>Else, yields the Recharge coroutine.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator Launch()
    {
        if (!readyToLaunch) yield break;



        readyToLaunch = false;


        target = harpoon_head_transform.localPosition + Vector3.up * launch_distance;
        start = harpoon_head_transform.localPosition;


        SetupTether();
        yield return LaunchProcess();



        if (successfulGrab) { readyToLaunch = true; successfulGrab = false; }
        else { StartCoroutine(Recharge()); };









    }




    void Start()
    {

        charge_color = MaterialHolder.Instance().SIDE_TOOLS_COLOR();


        grab.OnSuccessfulGrab += () => successfulGrab = true;

        harpoon_head = transform.GetChild(0).gameObject;
        harpoon_head_transform = harpoon_head.transform;
        harpoon_station_charge = transform.GetChild(1).gameObject;

        start = harpoon_head_transform.position; ;


        Renderer head_renderer = harpoon_head.GetComponent<Renderer>();
        off_color_head = head_renderer.materials[1];
        off_color_charge = head_renderer.materials[0];








        PlayerInputCommunication.OnMouseDown += MouseDown;
        PlayerInputCommunication.OnHarpoonColliderClick += HarpoonColliderClick;
    }



    private void OnDestroy()
    {
        PlayerInputCommunication.OnMouseDown -= MouseDown;
        PlayerInputCommunication.OnHarpoonColliderClick -= HarpoonColliderClick;
    }

    private void MouseDown()
    {
        if (!turnedOff) StartCoroutine(Launch());
    }


    /// <summary>
    /// <para>Plays the HARPOON_CONTROL_CLICK sound.</para>
    /// <para>Switches the boolean value of turnedOff.</para>
    /// <para>Sets the harpoon station charge's material to either off or charge color, based on turnedOff.</para>
    /// <para>Sets the head mats material to either off or charge color, based on turnedOff.</para>
    /// </summary>
    /// <param name="hit"></param>
    void HarpoonColliderClick(RaycastHit hit)
    {

        AudioManager.PlayActivitySound(AudioManager.ActivityType.HARPOON_CONTROL_CLICK);

        turnedOff = !turnedOff;
        harpoon_station_charge.GetComponent<Renderer>().material = (turnedOff) ? off_color_charge : charge_color;

        Material[] head_mats = harpoon_head.GetComponent<Renderer>().materials;
        head_mats[2] = turnedOff ? off_color_head : charge_color;

        harpoon_head.GetComponent<Renderer>().materials = head_mats;
    }

    /// <summary>
    /// Yields ChangeScale twice, once from the start scale to zero, then back to start scale.
    /// <para>Sets readyToLaunch to true afterwards.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator Recharge()
    {

        Vector3 start_scale = charge.transform.localScale;
        Vector3 target_scale = Vector3.zero;


        float duration = DifficultyManager.HARPOONxMISS_RECHARGE_DELAY_DIFFICULTY_DICT[DifficultyManager.DIFFICULTY];


        yield return StartCoroutine(ChangeScale(start_scale, target_scale, duration));
        yield return StartCoroutine(ChangeScale(target_scale, start_scale, duration));

        readyToLaunch = true;

    }

    /// <summary>
    /// LERPs the charge's localScale from start to end over a duration.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator ChangeScale(Vector3 start, Vector3 end, float duration)
    {
        float lerp = 0;

        while (lerp < duration)
        {
            lerp += Time.deltaTime;
            charge.transform.localScale = Vector3.Lerp(start, end, lerp / duration);

            yield return null;

        }


    }











    /// <summary>
    /// <para>Plays the HARPOON_LAUNCH sound.</para>
    /// <para>Moves the harpoon head towards the target and calls Track() in the process.</para>
    /// <para>Plays the HARPOON_RETRACTION sound. </para>
    /// <para>Moves the harpoon head back towards the start and calls Track() in the process.</para>
    /// <para>Destroys the laser tether gameObject.</para>
    /// </summary>
    /// <returns></returns>
    IEnumerator LaunchProcess()
    {



        AudioManager.PlayActivitySound(AudioManager.ActivityType.HARPOON_LAUNCH);

        while (Vector3.Distance(harpoon_head_transform.localPosition, target) > 0.001f)
        {
            harpoon_head_transform.localPosition = Vector3.MoveTowards(harpoon_head_transform.localPosition, target, launch_speed * Time.deltaTime);

            Track(harpoon_head_transform.position);
            yield return new WaitForEndOfFrame();

        }



        AudioManager.PlayActivitySound(AudioManager.ActivityType.HARPOON_RETRACTION);
        while (Vector3.Distance(harpoon_head_transform.localPosition, start) > 0.001f)
        {
            harpoon_head_transform.localPosition = Vector3.MoveTowards(harpoon_head_transform.localPosition, start, launch_speed * Time.deltaTime);

            Track(harpoon_head_transform.position);
            yield return new WaitForEndOfFrame();

        }



        Destroy(laser_tether);
    }








    Vector3 originVector;
    /// <summary>
    /// Creates the laser tether gameObjects, destroys its collider, sets localScale, angle and originVector.
    /// </summary>
    public void SetupTether()
    {



        laser_tether = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser_tether.GetComponent<Collider>());
        laser_tether.GetComponent<Renderer>().sharedMaterial = charge_color;
        laser_tether.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float angle = 45 * 180 / Mathf.PI;// transform.rotation.eulerAngles.x;

        originVector = transform.position + new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 0);





    }

    /// <summary>
    /// <para>Calculates the distance between the originVector and the arg targetVector.</para>
    /// <para>Sets length to the half of the distance.</para>
    /// <para>Calculates the middle vector as the half of the sum of target and origin vectors.</para>
    /// <para>Sets the tether position as the middle vector.</para>
    /// <para>Calculates the rotation direction as the subtraction of target and origin vector.</para>
    /// <para>Sets the tether up alignment as the rotation direction.</para>
    /// </summary>
    /// <param name="targetVector"></param>
    void Track(Vector3 targetVector)
    {


        float distance = Vector3.Distance(originVector, targetVector);

        laser_tether.transform.localScale = new Vector3(laser_tether.transform.localScale.x, distance * 0.5f, laser_tether.transform.localScale.z);

        Vector3 middleVector = 0.5f * (originVector + targetVector);
        laser_tether.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser_tether.transform.up = rotationDirection;

    }
}
