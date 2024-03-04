using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update



    public delegate void DifficultyChangeEvent(DifficultyEventArgs dea);
    public event DifficultyChangeEvent OnDifficultyValueChange;








    public static float BOMB_SPAWN_DELAY = 20f, DISRUPTOR_SPAWN_DELAY = 20, BOMB_SPEED_MULTIPLIER = 20;
    public static float DISRUPTOR_DISABLE_TIME = 10;
    public static int DISRUPTOR_SPAWN_CHANCE = 50;


    public const int SHIELD_MAX_CAPACITY = 3;


    public static int CONSTELLATION_SPAWN_RATE = 20;


    public static int DISRUPTOR_DEFAULT_START_HEALTH = 100, DISRUPTOR_START_HEALTH;






    static int bomb_spawner_value = 0;





    void Start()
    {
        DISRUPTOR_START_HEALTH = DISRUPTOR_DEFAULT_START_HEALTH;

    }




    public void EnemyChange()
    {

        DifficultyEventArgs dif_event_args = new(AffectedFeature.BOMB_SPAWNER, AffectedFeatureBehaviour.FORM, "", AffectedTarget.ENEMY);




    }


/*
    public Dictionary<AffectedFeature, List<AffectedFeatureBehaviour>> feature_behaviour_dict = new() 
    {
        {AffectedFeature.DISRUPTOR},
        { },
    
    
    
    };
*/


    public void FriendlyChange() 
    {








        DifficultyEventArgs dif_event_args = new(AffectedFeature.TURRET, AffectedFeatureBehaviour.CAPACITY, "+1", AffectedTarget.ENEMY);

    }






    void Update()
    {




        if (UICommunication.Secs == 0) { return; }

        if (UICommunication.Secsf % 5 < Time.deltaTime)
        {



            DifficultyEventArgs dif_event_args = new(AffectedFeature.DISRUPTOR, AffectedFeatureBehaviour.CHARGE_UP_SPEED, "6", AffectedTarget.ENEMY);


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
        Message = feature.ToString()?.Replace("_", " ") + " : " + behaviour.ToString()?.Replace("_", " ") + " " + affectedBehaviourVal;
        Affected = target;
    }




}
public enum AffectedTarget
{
    FRIENDLY, ENEMY

}
public enum AffectedFeature
{
    DISRUPTOR, BOMB_SPAWNER, SLIDER, TURRET, SUPERNOVA

}

public enum AffectedFeatureBehaviour
{
    SPAWN_RATE, SPAWN_CHANCE, CAPACITY, RECHARGE,FORM, SPEED, CHARGE_UP_SPEED







}