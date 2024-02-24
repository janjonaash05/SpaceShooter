using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserControlColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject auto_collider;






    [SerializeField] int ID;


    private readonly List<int> COLOR_INDEXES = new() { 2, 3, 4, 5 };
    public const int AUTO_INDEX = 6;


    bool turned_off = false;







    public void Start()
    {
        current_mats = GetComponent<Renderer>().materials;



        normal_mats = new Material[GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < normal_mats.Length; i++)
        {
            normal_mats[i] = new Material(GetComponent<Renderer>().materials[i]);
        }



        mat_index_dict = new Dictionary<string, int>();
        for (int i = 0; i < current_mats.Length; i++)
        {

            try { mat_index_dict.Add(current_mats[i].name, i); } catch (Exception) { };

        }
      

        switch (ID)
        {
            case 1:
                LaserTurretCommunicationChannels.Channel1.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunicationChannels.Channel1.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunicationChannels.Channel1.OnControlDisabled += TurnOff;
                LaserTurretCommunicationChannels.Channel1.OnControlEnabled += TurnOn;


                break;

            case 2:

                LaserTurretCommunicationChannels.Channel2.OnColorCollider_ControlColorChange += StartChange;
                LaserTurretCommunicationChannels.Channel2.OnAutoCollider_ControlColorChange += StartChange;


                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingDisabled += DisableAutoTargeting;
                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingEnabled += EnableAutoTargeting;


                LaserTurretCommunicationChannels.Channel2.OnControlDisabled += TurnOff;
                LaserTurretCommunicationChannels.Channel2.OnControlEnabled += TurnOn;

                break;

        }





       

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

    void TurnOff()
    {
        turned_off = true;



        StopAllCoroutines();

        GetComponent<Renderer>().materials = off_mats;


    }

    void TurnOn()
    {

        turned_off = false;
        GetComponent<Renderer>().materials = normal_mats;
    }

    void DisableAutoTargeting()
    {






        StopAllCoroutines();

        auto_collider.GetComponent<Renderer>().material = block_material;


        Debug.LogWarning("DisableAutoTargeting");


        Renderer rend = GetComponent<Renderer>();

        Material[] mats = rend.materials; ;

        // mats[AUTO_INDEX].SetColor(EMISSION_COLOR, block_material.GetColor(EMISSION_COLOR));
        mats[AUTO_INDEX] = off_mat;

        rend.materials = mats;

    }










    void EnableAutoTargeting()
    {
        auto_collider.GetComponent<Renderer>().material = allow_material;
        if (turned_off) { return; }

        Debug.LogWarning("ableAutoTargeting");

        Renderer rend = GetComponent<Renderer>();


        Material[] mats = rend.materials;
        // mats[AUTO_INDEX].SetColor(EMISSION_COLOR, allow_material.GetColor(EMISSION_COLOR));


        mats[AUTO_INDEX] = normal_mats[AUTO_INDEX];

        rend.materials = mats;


    }













    public enum CONTROL_TYPE { SLIDER, TURRET_1, TURRET_2 };



    public static readonly float DARKENING_WAIT_TIME = 0.075f;
    [SerializeField] float darkening_intensity;
    [SerializeField] Dictionary<string, int> mat_index_dict;

    [SerializeField] CONTROL_TYPE control_type;

    private Material[] current_mats;


    [SerializeField][Tooltip("used for emission material color only")] protected Material block_material, allow_material;



    protected const string EMISSION_COLOR = "_EmissionColor";



    public void StartChange(Material mat)
    {
        // Material mat = hit.transform.GetComponent<Renderer>().material;
        current_mats = new Material[GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < current_mats.Length; i++)
        {
            current_mats[i] = GetComponent<Renderer>().materials[i];
        }

        if (mat_index_dict.ContainsKey(mat.name))
        {
            int index = mat_index_dict[mat.name];
            StartCoroutine(Change(index));
        }


    }








  




    

    IEnumerator Change(int index)
    {


        Color old = current_mats[index].GetColor(EMISSION_COLOR);

        current_mats[index].SetColor(EMISSION_COLOR, current_mats[index].color * darkening_intensity);

        GetComponent<Renderer>().materials = current_mats;

        yield return new WaitForSeconds(DARKENING_WAIT_TIME);

        current_mats[index].SetColor(EMISSION_COLOR, old);
        GetComponent<Renderer>().materials = current_mats;




    }

    












}
