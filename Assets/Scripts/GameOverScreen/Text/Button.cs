using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    bool isHovering = false;



    [SerializeField] ButtonFunction buttonFunction;

    [SerializeField] Material on_mat;
    Material off_mat;






    AudioSource src;



    Renderer rend;



    public Action OnClick;



    public Dictionary<ButtonFunction, Action> function_dict;







    void SETTINGS()
    {
        SceneManager.LoadScene(3);
        
    }


    void STATS()
    {
        SceneManager.LoadScene(2);

    }



    void APPLY() 
    {

        SceneManager.LoadScene(0);
    }

    void PLAY_EASY() 
    {

        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.EASY);
        SceneManager.LoadScene(1);

    }


    void PLAY_NORMAL()
    {

        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.NORMAL);
        SceneManager.LoadScene(1);

    }

    void PLAY_HARD()
    {

        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.HARD);
        SceneManager.LoadScene(1);

    }


    void RETRY()
    {




        SceneManager.LoadScene(1);

    }



    void EXIT_TO_MENU()
    {

        SceneManager.LoadScene(0);

    }


    void EXIT_GAME()
    {

        Application.Quit();
    }


    public enum ButtonFunction
    {


        RETRY, EXIT_TO_MENU, EXIT_GAME, PLAY_EASY, PLAY_NORMAL,PLAY_HARD, SETTINGS, APPLY


    }



    const float DEFAULT_VOLUME = 0.5f;


    void Start()
    {

        src = GetComponent<AudioSource>();
        src.volume = DEFAULT_VOLUME;
        src.spatialBlend = 0.5f;

        rend = GetComponent<Renderer>();
        off_mat = rend.materials[1];

        function_dict = new()
        {
            {ButtonFunction.RETRY, RETRY },
            {ButtonFunction.EXIT_TO_MENU, EXIT_TO_MENU },
            {ButtonFunction.EXIT_GAME, EXIT_GAME },
            {ButtonFunction.PLAY_EASY, PLAY_EASY },
            {ButtonFunction.PLAY_NORMAL, PLAY_NORMAL },
            {ButtonFunction.PLAY_HARD, PLAY_HARD},
            {ButtonFunction.SETTINGS, SETTINGS },
            {ButtonFunction.APPLY, APPLY },



        };


        





        OnClick = () => { src.pitch = 1.1f ; src.volume = DEFAULT_VOLUME * UserDataManager.VOLUME_MULTIPLIER; src.Play();  function_dict[buttonFunction](); };
    }




    public void OnHoverEnter()
    {
        if (isHovering) return;



        src.pitch = 1.25f;
        src.volume = DEFAULT_VOLUME * UserDataManager.VOLUME_MULTIPLIER;
        src.Play();







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
