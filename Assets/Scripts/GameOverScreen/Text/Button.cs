using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    bool isHovering = false;



    [SerializeField] ButtonFunction buttonFunction;

    [SerializeField] Material on_mat;
    Material off_mat;





    Renderer rend;



    public Action OnClick;



    public Dictionary<ButtonFunction, Action> function_dict;







    void RETRY()
    {

        SceneManager.LoadScene(1);

    }



    void EXIT_TO_MENU()
    {

        Debug.Log("Exitting to menu");

    }


    void EXIT_GAME()
    {

        Debug.Log("Exitting game");

    }


    public enum ButtonFunction
    {


        RETRY, EXIT_TO_MENU, EXIT_GAME


    }


    void Start()
    {
        rend = GetComponent<Renderer>();
        off_mat = rend.materials[1];

        function_dict = new()
        {
            {ButtonFunction.RETRY, RETRY },
            {ButtonFunction.EXIT_TO_MENU, EXIT_TO_MENU },
             {ButtonFunction.EXIT_GAME, EXIT_GAME }




        };





        OnClick = function_dict[buttonFunction];
    }




    public void OnHoverEnter()
    {
        if (isHovering) return;









        Material[] mats = rend.materials;

        mats[0] = off_mat;
        mats[1] = on_mat;

        rend.materials = mats;


        isHovering = true;



    }



    public void OnHoverExit()
    {

        if (!isHovering) return;



        Material[] mats = rend.materials;

        mats[0] = on_mat;
        mats[1] = off_mat;

        rend.materials = mats;

        isHovering = false;

    }



    void Update()
    {

    }
}
