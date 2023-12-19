using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update



    public delegate void DifficultyChangeEvent(DifficultyEventArgs dea);
    public event DifficultyChangeEvent OnDifficultyValueChange;








    public static float BOMB_SPAWN_DELAY = 20f, DISRUPTOR_SPAWN_DELAY = 20, BOMB_SPEED_MULTIPLIER = 2;
    public static float DISRUPTOR_DISABLE_TIME = 10;
    public static int DISRUPTOR_SPAWN_CHANCE = 50;


    public static int CONSTELLATION_SPAWN_RATE = 20;


    public static int DISRUPTOR_DEFAULT_START_HEALTH = 100, DISRUPTOR_START_HEALTH;

    // TimeCounter time_counter;
    //PopupDisplay popup_display;










    void Start()
    {
        DISRUPTOR_START_HEALTH = DISRUPTOR_DEFAULT_START_HEALTH;


        //   time_counter = FindObjectOfType<TimeCounter>();

        //        popup_display = FindObjectOfType<PopupDisplay>();


        //   popup_display.OnPopupFinish += DequeuePopupCall;


    }




    void Update()
    {




        if (UICommunication.Secs == 0) { return; }

        if (UICommunication.Secsf % 5 < Time.deltaTime)
        {



            DifficultyEventArgs dif_event_args = new(AffectedFeature.TURRET, AffectedFeatureBehaviour.CAPACITY, "+1", AffectedTarget.FRIENDLY);


            if (UICommunication.CanPopup)
            {
                UICommunication.Raise_OnDifficultyValueChange(dif_event_args);

            }
            else
            {
                UICommunication.Enqueue_PopupArguments(dif_event_args);

            }

            /*
                        if (popup_display.CanPopup)
                        {
                            OnDifficultyValueChange?.Invoke(dif_event_args);

                        }
                        else
                        {
                            popup_queue.Enqueue(dif_event_args);
                        }
            */



        }
    }


    /*


    Queue<DifficultyEventArgs> popup_queue = new();
    void DequeuePopupCall()
    {
        if (popup_queue.Count == 0) { return; }
        OnDifficultyValueChange?.Invoke(popup_queue?.Dequeue());

    }


    */







}
public class DifficultyEventArgs : EventArgs
{


    public string Message { get; private set; }


    public AffectedTarget Affected { get; private set; }


    public DifficultyEventArgs(AffectedFeature feature, AffectedFeatureBehaviour behaviour, string affectedBehaviourVal, AffectedTarget target)
    {
        Message = feature.ToString() + " : " + behaviour.ToString()?.Replace("_", " ") + " " + affectedBehaviourVal;
        Affected = target;
    }




}
public enum AffectedTarget
{
    FRIENDLY, ENEMY

}
public enum AffectedFeature
{
    DISRUPTOR, BOMB, SLIDER, TURRET

}

public enum AffectedFeatureBehaviour
{
    SPAWN_RATE, SPAWN_CHANCE, CAPACITY, RECHARGE







}