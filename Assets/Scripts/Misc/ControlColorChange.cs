using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class ControlColorChange : MonoBehaviour
{

    public enum CONTROL_TYPE { SLIDER, TURRET_1, TURRET_2 };



    public static readonly float DARKENING_WAIT_TIME = 0.075f;
    [SerializeField] float darkening_intensity;
    [SerializeField] Dictionary<string, int> mat_index_dict;

    [SerializeField] CONTROL_TYPE control_type;

    private Material[] current_mats;







    [SerializeField][Tooltip("used for emission material color only")] protected Material block_material, allow_material;



    protected const string EMISSION_COLOR = "_EmissionColor";







   

    public virtual void Start()
    {
       



        current_mats = GetComponent<Renderer>().materials;

        mat_index_dict = new Dictionary<string, int>();


        for (int i = 0; i < current_mats.Length; i++)
        {

            try { mat_index_dict.Add(current_mats[i].name, i); } catch (Exception) { };

        }







    }






    public void StartChange(Material mat)
    {
       // Material mat = hit.transform.GetComponent<Renderer>().material;
        current_mats = GetComponent<Renderer>().materials;

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
