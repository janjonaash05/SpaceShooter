using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum AffectedFeatureCircumstance
    {
        TOKEN, TIME
    }
    public enum AffectedFeature
    {
        DISRUPTORxSPAWN_CHANCE, DISRUPTORxSPEED,
        BOMB_SPAWNERxSPAWN_RATE, BOMB_SPAWNERxFORM,
        SUPERNOVAxSPAWN_RATE, SUPERNOVAxCHARGE_UP_SPEED
    }



    public static Dictionary<AffectedFeature, int> FEATURE_VALUE_DICT = new()
    {
        {AffectedFeature.DISRUPTORxSPAWN_CHANCE,0 },
        {AffectedFeature.DISRUPTORxSPEED,0 },
        {AffectedFeature.BOMB_SPAWNERxSPAWN_RATE,0 },
        {AffectedFeature.BOMB_SPAWNERxFORM,0 },
        {AffectedFeature.SUPERNOVAxSPAWN_RATE,0 },
        {AffectedFeature.SUPERNOVAxCHARGE_UP_SPEED,0 },


    };


    public static List<AffectedFeature> TOKEN_CHANGABLE_FEATURES = new() { AffectedFeature.DISRUPTORxSPEED, AffectedFeature.BOMB_SPAWNERxFORM, AffectedFeature.SUPERNOVAxCHARGE_UP_SPEED };

    public static List<AffectedFeature> TIME_CHANGABLE_FEATURES = new() { AffectedFeature.DISRUPTORxSPAWN_CHANCE, AffectedFeature.BOMB_SPAWNERxFORM, AffectedFeature.SUPERNOVAxCHARGE_UP_SPEED };



    public static Dictionary<int, int> DISRUPTOR_SPAWN_CHANCE_DEGREE_VALUE_DICT = new()
    {
        {0, 15 },
        {1, 30 },
        {2, 45 },
        {3, 60 },
        {4, 75 },

    };


    //TODO
    public static Dictionary<int, (int min, int max)> DISRUPTOR_SPEED_DEGREE_VALUE_DICT = new()
    {
      /*  {0, 15 },
        {1, 30 },
        {2, 45 },
        {3, 60 },
        {4, 75 },
      */
    };


    public static Dictionary<int, float> BOMB_SPAWNER_SPAWN_RATE_DEGREE_VALUE_DICT = new()
    {
        {0, 20f },
        {1, 17.5f },
        {2, 15f },
        {3, 12.5f },
        {4, 10f },

    };








    public delegate void DifficultyChangeEvent(DifficultyEventArgs dea);
    public event DifficultyChangeEvent OnDifficultyValueChange;








    public static float BOMB_SPAWN_DELAY = 20f, DISRUPTOR_SPAWN_DELAY = 20, BOMB_SPEED_MULTIPLIER = 20;
    public static float DISRUPTOR_DISABLE_TIME = 10;
    public static int DISRUPTOR_SPAWN_CHANCE = 100;





    public static int CONSTELLATION_SPAWN_RATE = 200;


    public static int DISRUPTOR_DEFAULT_START_HEALTH = 100, DISRUPTOR_START_HEALTH;






    static int bomb_spawner_value = 0;





    void Start()
    {
        DISRUPTOR_START_HEALTH = DISRUPTOR_DEFAULT_START_HEALTH;

    }




    public void EnemyChange()
    {

        DifficultyEventArgs dif_event_args = new(AffectedFeature.BOMB_SPAWNER, AffectedFeatureBehaviour.FORM, "", AffectedFeatureCircumstance.ENEMY);




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


        DifficultyEventArgs dif_event_args = new(AffectedFeature.DISRUPTOR, AffectedFeatureBehaviour.CHARGE_UP_SPEED, "6", AffectedFeatureCircumstance.ENEMY);


        if (UICommunication.CanPopup)
        {
            UICommunication.Raise_OnDifficultyValueChange(dif_event_args);

        }
        else
        {
            UICommunication.Enqueue_PopupArguments(dif_event_args);

        }










    }






    void Update()
    {




        if (UICommunication.Secs == 0) { return; }

        if (UICommunication.Secsf % 5 < Time.deltaTime)
        {



            DifficultyEventArgs dif_event_args = new(AffectedFeature.DISRUPTOR, AffectedFeatureBehaviour.CHARGE_UP_SPEED, "6", AffectedFeatureCircumstance.ENEMY);


            if (UICommunication.CanPopup)
            {
                UICommunication.Raise_OnDifficultyValueChange(dif_event_args);

            }
            else
            {
                UICommunication.Enqueue_PopupArguments(dif_event_args);

            }



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


    public DifficultyManager.AffectedFeatureCircumstance Target { get; private set; }

    //TODO
    public DifficultyEventArgs(DifficultyManager.AffectedFeature feature, string affectedBehaviourVal, DifficultyManager.AffectedFeatureCircumstance target)
    {
       // Message = feature.ToString()?.Replace("_", " ") + " : " + behaviour.ToString()?.Replace("_", " ") + " " + affectedBehaviourVal;
        Target = target;
    }




}
