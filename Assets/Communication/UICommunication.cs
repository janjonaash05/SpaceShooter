using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class UICommunication
{

    public static int Score { get; private set; } = 0;
    public static int Tokens { get; private set; } = 0;

    public static int Secs { get; private set; } = 0;
    public static int Mins { get; private set; } = 0;
    public static int Hundredths { get; private set; } = 0;



    public static float Secsf { get; private set; } = 0;

    public static float Minsf { get; private set; } = 0;



    public static void Awake() 
    {
        Score = 0;
        Tokens = 16;

    
    
    
    }






    public static event Action OnScoreChange;
    public static event Action OnTokensChange;





    public static bool CanPopup = true;



    public delegate void DifficultyChangeEvent(DifficultyEventArgs dea);
    public static event DifficultyChangeEvent OnDifficultyValueChange;



    static Queue<DifficultyEventArgs> popup_queue = new();
    public static void Raise_ScoreChange(int change)
    {
        Score += change;
        OnScoreChange?.Invoke();
    }


    public static void Raise_TokenChange(int change)
    {
        Tokens += change;
        OnTokensChange?.Invoke();
    }



    
    public static void Assign_TimeValues(float sf, int s, float mf, int m, int h)
    {

        Secsf = sf;
        Secs = s;
        Minsf = mf;
        Mins = m;
        Hundredths = h;

    }




    public static void Enqueue_PopupArguments(DifficultyEventArgs dea)
    {

        
        popup_queue.Enqueue(dea);
    }



    /// <summary>
    /// If the popup queue isn't empty, then invokes OnDifficultyValueChange with dequeued popup queue entry.
    /// </summary>
    public static void Dequeue_PopupCall() 
    {
        if (popup_queue.Count == 0) { return; }
        OnDifficultyValueChange?.Invoke(popup_queue?.Dequeue());


    }

    
    public static void Raise_OnDifficultyValueChange(DifficultyEventArgs dea) 
    {
        OnDifficultyValueChange?.Invoke(dea);

    }






}
