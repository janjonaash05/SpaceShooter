using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserControlColorChange : MonoBehaviour
{
    
    [SerializeField] GameObject auto_collider;






    [SerializeField] int ID;


    public const int AUTO_INDEX = 6;


    bool turned_off = false;








    LaserTurretChannel channel;

    private void OnDestroy()
    {


        channel.OnColorCollider_ControlColorChange -= StartChange;
        channel.OnAutoCollider_ControlColorChange -= StartChange;

        channel.OnColorCollider_ControlColorChange -= PlaySound;
        channel.OnAutoCollider_ControlColorChange -= PlaySound;

        channel.OnAutoTargetingDisabled -= DisableAutoTargeting;
        channel.OnAutoTargetingEnabled -= EnableAutoTargeting;


        channel.OnControlDisabled -= TurnOff;
        channel.OnControlEnabled -= TurnOn;

    }


    Renderer rend;


    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        rend = GetComponent<Renderer>();
        current_mats = rend.materials;



        normal_mats = new Material[rend.materials.Length];

        for (int i = 0; i < normal_mats.Length; i++)
        {
            normal_mats[i] = new Material(rend.materials[i]);
        }



        mat_index_dict = new Dictionary<string, int>();
        for (int i = 0; i < current_mats.Length; i++)
        {

            try { mat_index_dict.Add(current_mats[i].name, i); } catch (Exception) { };

        }

        channel = LaserTurretCommunicationChannels.GetChannelByID(ID);

        channel.OnColorCollider_ControlColorChange += StartChange;
        channel.OnAutoCollider_ControlColorChange += StartChange;

        channel.OnColorCollider_ControlColorChange += PlaySound;
        channel.OnAutoCollider_ControlColorChange += PlaySound;

        channel.OnAutoTargetingDisabled += DisableAutoTargeting;
        channel.OnAutoTargetingEnabled += EnableAutoTargeting;


        channel.OnControlDisabled += TurnOff;
        channel.OnControlEnabled += TurnOn;







        off_mat = MaterialHolder.Instance().TURRET_CONTROL_AUTO_COLOR_OFF();
       

        off_mats = new Material[]
        {
            normal_mats[0],normal_mats[1],
            off_mat,off_mat,
            off_mat,off_mat,
            off_mat
        };

      
    }


    Material[] normal_mats, off_mats;
    Material off_mat;




    /// <summary>
    /// Plays either TURRET_CONTROL_CLICK_1 or TURRET_CONTROL_CLICK_2 sound based on ID.
    /// </summary>
    /// <param name="_"></param>
    void PlaySound(Material _)
    {
        var activity_type = (ID == 1) ? AudioManager.ActivityType.TURRET_CONTROL_CLICK_1 : AudioManager.ActivityType.TURRET_CONTROL_CLICK_2;


        AudioManager.PlayActivitySound(activity_type);
    }




   
    /// <summary>
    /// Sets turned_off to true, stops all coroutines, sets the renderer materials to off mats.
    /// </summary>
    void TurnOff()
    {
        turned_off = true;



        StopAllCoroutines();

        rend.materials = off_mats;


    }


    /// <summary>
    /// Sets turned_off to true, sets the renderer materials to normal mats.
    /// </summary>
    void TurnOn()
    {

        turned_off = false;
        rend.materials = normal_mats;
    }


    /// <summary>
    /// Stops all coroutines.
    /// <para>Sets the auto_collider's renderer materia to block_material.</para>
    /// <para>Sets the object's renderer material at AUTO_INDEX to off mat.</para> 
    /// </summary>
    void DisableAutoTargeting()
    {

        StopAllCoroutines();
        auto_collider.GetComponent<Renderer>().material = block_material;


        Material[] mats = rend.materials; ;

        mats[AUTO_INDEX] = off_mat;

        rend.materials = mats;

    }









    /// <summary>
    /// <para>Sets the auto_collider's renderer material to allow_material.</para>
    /// <para>If turned_off, returns.</para>
    /// <para>Sets the renderer materials at AUTO_INDEX to</para>
    /// </summary>
    void EnableAutoTargeting()
    {
        auto_collider.GetComponent<Renderer>().material = allow_material;
        if (turned_off) { return; }

        Material[] mats = rend.materials;


        mats[AUTO_INDEX] = normal_mats[AUTO_INDEX];

        rend.materials = mats;


    }













   



    public static readonly float DARKENING_WAIT_TIME = 0.075f;
    [SerializeField] float darkening_intensity;
    [SerializeField] Dictionary<string, int> mat_index_dict;


    private Material[] current_mats;


    [SerializeField][Tooltip("used for emission material timer_color only")] protected Material block_material, allow_material;



    protected const string EMISSION_COLOR = "_EmissionColor";


    /// <summary>
    /// <para>Assigns the current mats as a copy of the renderer materials.</para>
    /// <para>Attempts to get the index from the mat_index_dict using the material name</para>
    /// <para>Starts the Change coroutine with the index.</para>
    /// </summary>
    /// <param name="mat"></param>
    public void StartChange(Material mat)
    {
        current_mats = new Material[rend.materials.Length];
        Array.Copy(rend.materials, current_mats, current_mats.Length);

        if (mat_index_dict.ContainsKey(mat.name))
        {
            int index = mat_index_dict[mat.name];
            StartCoroutine(Change(index));
        }


    }














    /// <summary>
    /// <para>Gets the old color as the current mats emission color at arg index.</para>
    /// <para>Sets the emission color at arg index to the same one, multiplied by darkening intensity.</para>
    /// <para>Assigns the materials to the renderer.</para>
    /// <para>Waits a set amount of time.</para>
    /// <para><para>Sets the emission color at arg index to the old color.</para>
    /// <para>Assigns the materials to the renderer.</para>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IEnumerator Change(int index)
    {


        Color old = current_mats[index].GetColor(EMISSION_COLOR);

        current_mats[index].SetColor(EMISSION_COLOR, current_mats[index].color * darkening_intensity);

        rend.materials = current_mats;

        yield return new WaitForSeconds(DARKENING_WAIT_TIME);

        current_mats[index].SetColor(EMISSION_COLOR, old);
        rend.materials = current_mats;




    }

    












}
