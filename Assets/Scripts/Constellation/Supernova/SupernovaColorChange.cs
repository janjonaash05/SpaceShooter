using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaColorChange : MonoBehaviour, IEMPDisruptable
{
    // Start is called before the first frame update



    [SerializeField] List<Material> color_mats;

    [SerializeField] Material primary, secondary;


    [SerializeField] float color_change_delay;

    [SerializeField] Material white;


    Renderer r;




    public  event Action OnColorUpFinished;

    public event Action OnDeathColorUpFinished;

   
    




    void InitColorUp()
    {

        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = secondary;
        }

        start_current_mats[PRIMARY_INDEX] = primary;

        start_current_mats[CENTER_INDEX] = secondary;


        r.materials = start_current_mats;

    }



    void OnlyCenterColorUp(Material m) 
    {
        Material[] start_current_mats = new Material[r.materials.Length];
        for (int i = 0; i < r.materials.Length; i++)
        {
            start_current_mats[i] = r.materials[i];
        }



        start_current_mats[CENTER_INDEX] = m;

        r.materials = start_current_mats;

    }


    void SetCentertAndAddColorToList(Material m)
    {
        color_mats.Add(m);

        OnlyCenterColorUp(m);





    }

    private void Awake()
    {

    }





    void Start()
    {



        StarFall.OnStarFallen += SetCentertAndAddColorToList;
        FormConstellation.OnAllStarsGone += AllColorUp;

        color_mats = new();


        r = GetComponent<Renderer>();


        InitColorUp();



    }

    private void OnDestroy()
    {
        StarFall.OnStarFallen -= SetCentertAndAddColorToList;
        FormConstellation.OnAllStarsGone -= AllColorUp;
    }



  








    void AllColorUp() 
    {
        OnlyCenterColorUp(white);

        IEnumerator colorUp()
        {

            for (int i = 0; i < color_mats.Count; i++)
            {



                AddColor(color_mats[i]);
                


                yield return new WaitForSeconds(color_change_delay);
            }



            OnColorUpFinished?.Invoke();

        }

        StartCoroutine(colorUp());

    }




    // Update is called once per frame
    void Update()
    {

    }





    int current_color_index = 1;

    void AddColor(Material color) 
    {
       

        Material[] new_mats = new Material[r.materials.Length];



        int index_to_change = color_order_index_dict[current_color_index];
        for (int j = 0; j < r.materials.Length; j++)
        {

            new_mats[j] = j == index_to_change ? color: r.materials[j];

        }




        r.materials = new_mats;
        current_color_index++;

    }


   public void RemoveColor() 
    {
        current_color_index--;

        Material[] new_mats = new Material[r.materials.Length];



        int index_to_change = color_order_index_dict[current_color_index];
        for (int j = 0; j < r.materials.Length; j++)
        {

            new_mats[j] = j == index_to_change ? secondary : r.materials[j];

        }




        r.materials = new_mats;
       

    }

    public void OnEMP()
    {
        StopAllCoroutines();
    }

    const int CENTER_INDEX = 0;


    const int PRIMARY_INDEX = 1;


    static readonly Dictionary<int, int> color_order_index_dict = new()
    {
        {1,9 },
        {2,2 },
        {3,4 },
        {4,6 },
        {5,7 },
        {6,3 },
        {7,8 },
        {8,5 },
    };









}
