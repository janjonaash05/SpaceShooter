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






    void STATS()
    {
        SceneManager.LoadScene(4);
        
    
    }


    void SETTINGS()
    {
        SceneManager.LoadScene(3);
        
    }

    void APPLY()
    {

        UserDataManager.SetSettingsData(SettingsManager.NewSettings);
        UserDataManager.Save();
        SceneManager.LoadScene(0);

    }


    void DISCARD()
    {
        SettingsManager.DiscardSettings();
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


    void RESET() 
    {
        SceneManager.LoadScene(5);
        
    }



    void CONFIRM_DELETION() 
    {
        UserDataManager.ResetUserScoreTime();
        SceneManager.LoadScene(4);
    }


    void CANCEL_DELETION()
    {
        SceneManager.LoadScene(4);
    }

    public enum ButtonFunction
    {


        RETRY, EXIT_TO_MENU, EXIT_GAME, PLAY_EASY, PLAY_NORMAL,PLAY_HARD, SETTINGS, DISCARD,APPLY, STATS, 
        CONFIRM_DELETION, CANCEL_DELETION, RESET


    }



    const float DEFAULT_VOLUME = 0.5f;

    /// <summary>
    /// <para>Gets the audio source and the off material, assigns ButtonFunctions to functions in a dictionary</para>
    /// <para>Assigns an OnClick function to set the pitch, volume, play sound and call the appropriate ButtonFunction function.</para>
    /// </summary>
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
            {ButtonFunction.STATS, STATS },
            {ButtonFunction.APPLY, APPLY },
            {ButtonFunction.DISCARD, DISCARD },
            {ButtonFunction.CONFIRM_DELETION,CONFIRM_DELETION },
            {ButtonFunction.CANCEL_DELETION, CANCEL_DELETION },
            {ButtonFunction.RESET, RESET },



        };


        





        OnClick = () => { src.pitch = 1.1f ; src.volume = DEFAULT_VOLUME * UserDataManager.CURRENT_DATA.VolumeMultiplier; src.Play();  function_dict[buttonFunction](); };
    }



    /// <summary>
    /// <para>If isHovering is true, returns early.</para>
    /// <para>Sets the source pitch and sets the volume based on user data, plays.</para>
    /// <para>Assigns the off and on materials to the renderer.</para>
    /// <para>Sets isHovering to true.</para>
    /// </summary>
    public void OnHoverEnter()
    {
        if (isHovering) return;



        src.pitch = 1.25f;
        src.volume = DEFAULT_VOLUME * UserDataManager.CURRENT_DATA.VolumeMultiplier;
        src.Play();







        Material[] mats = rend.materials;

        mats[0] = off_mat;
        mats[1] = on_mat;

        rend.materials = mats;


        isHovering = true;



    }



    /// <summary>
    /// <para>If isHovering is false, returns early.</para>
    /// <para>Assigns the on and off materials to the renderer.</para>
    /// <para>Sets isHovering to false.</para>
    /// </summary>
    public void OnHoverExit()
    {

        if (!isHovering) return;



        Material[] mats = rend.materials;

        mats[0] = on_mat;
        mats[1] = off_mat;

        rend.materials = mats;

        isHovering = false;

    }



 

}
