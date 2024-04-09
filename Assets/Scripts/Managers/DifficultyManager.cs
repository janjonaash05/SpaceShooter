using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradesManager;

public class DifficultyManager : MonoBehaviour
{
    


    public enum AffectedFeatureCircumstance
    {
        TOKEN, TIME
    }
    public enum AffectedFeature
    {
        DISRUPTORxSPAWN_CHANCE, DISRUPTORxSPEED,
        BOMB_SPAWNERxSPAWN_RATE, BOMB_SPAWNERxFORM,
        CONSTELLATIONxSPAWN_RATE, CONSTELLATIONxMAX_STARS
    }








    public static Dictionary<AffectedFeature, int> FEATURE_VALUE_DICT = new()
    {
        {AffectedFeature.DISRUPTORxSPAWN_CHANCE,0 },
        {AffectedFeature.DISRUPTORxSPEED,0 },

        {AffectedFeature.BOMB_SPAWNERxSPAWN_RATE,0 },
        {AffectedFeature.BOMB_SPAWNERxFORM, 0},

        {AffectedFeature.CONSTELLATIONxSPAWN_RATE,0 },
        {AffectedFeature.CONSTELLATIONxMAX_STARS,0 },

    };


    public const int MAX_FEATURE_VALUE = 4;

    public static List<AffectedFeature> TOKEN_CHANGABLE_FEATURES = new() { AffectedFeature.DISRUPTORxSPEED, AffectedFeature.BOMB_SPAWNERxFORM, AffectedFeature.CONSTELLATIONxMAX_STARS };

    public static List<AffectedFeature> TIME_CHANGABLE_FEATURES = new() { AffectedFeature.DISRUPTORxSPAWN_CHANCE, AffectedFeature.BOMB_SPAWNERxSPAWN_RATE, AffectedFeature.CONSTELLATIONxSPAWN_RATE };

    public static int GetCurrentBombSpawnerFormValue()
    {
        return FEATURE_VALUE_DICT[AffectedFeature.BOMB_SPAWNERxFORM];
    }


    public static Dictionary<int, int> DISRUPTOR_SPAWN_CHANCE_DEGREE_VALUE_DICT = new()
    {
        {0, 15 },
        {1, 30 },
        {2, 45 },
        {3, 60 },
        {4, 75 },

    };


    public static int GetCurrentDisruptorSpawnChanceValue()
    {
        return DISRUPTOR_SPAWN_CHANCE_DEGREE_VALUE_DICT[FEATURE_VALUE_DICT[AffectedFeature.DISRUPTORxSPAWN_CHANCE]];
    }


    public static Dictionary<int, (float min, float max)> DISRUPTOR_SPEED_DEGREE_VALUE_DICT = new()
    {
        {0, (2.5f,5) },
        {1, (1.9375f,3.875f) },
        {2, (1.375f,2.75f) },
        {3, (0.8125f,1.625f) },
        {4, (0.25f,0.5f) },

    };

    public static (float min, float max) GetCurrentDisruptorSpeedValue()
    {
        return DISRUPTOR_SPEED_DEGREE_VALUE_DICT[FEATURE_VALUE_DICT[AffectedFeature.DISRUPTORxSPEED]];
    }


    public static Dictionary<int, float> BOMB_SPAWNER_SPAWN_RATE_DEGREE_VALUE_DICT = new()
    {
        {0, 20f },
        {1, 17.5f },
        {2, 15f },
        {3, 12.5f },
        {4, 10f },

    };

    public static float GetCurrentBombSpawnerSpawnRateValue()
    {
        return BOMB_SPAWNER_SPAWN_RATE_DEGREE_VALUE_DICT[FEATURE_VALUE_DICT[AffectedFeature.BOMB_SPAWNERxSPAWN_RATE]];
    }


    public static Dictionary<int, float> CONSTELLATION_SPAWN_RATE_DEGREE_VALUE_DICT = new()
    {
        {0, 60f },
        {1, 47.5f },
        {2, 35f },
        {3, 22.5f },
        {4, 10f },

    };

    public static float GetCurrentConstellationSpawnRateValue()
    {
        return CONSTELLATION_SPAWN_RATE_DEGREE_VALUE_DICT[FEATURE_VALUE_DICT[AffectedFeature.CONSTELLATIONxSPAWN_RATE]];
    }

    public static Dictionary<int, int> CONSTELLATION_MAX_STARS_DEGREE_VALUE_DICT = new()
    {
        {0, 8 },
        {1, 5 },
        {2, 6 },
        {3, 7 },
        {4, 8 },
    };


    public static int GetCurrentConstellationMaxStarsValue()
    {
        return CONSTELLATION_MAX_STARS_DEGREE_VALUE_DICT[FEATURE_VALUE_DICT[AffectedFeature.CONSTELLATIONxMAX_STARS]];
    }


    public static string GetCurrentFormattedValue(AffectedFeature feature)
    {

        return feature switch
        {
            AffectedFeature.DISRUPTORxSPEED => "AVG " +  1/ Math.Round((GetCurrentDisruptorSpeedValue().min + GetCurrentDisruptorSpeedValue().max) / 2, 2),
            AffectedFeature.DISRUPTORxSPAWN_CHANCE => GetCurrentDisruptorSpawnChanceValue() + "% / minute",
            AffectedFeature.CONSTELLATIONxSPAWN_RATE => GetCurrentConstellationSpawnRateValue() + "s",
            AffectedFeature.CONSTELLATIONxMAX_STARS => GetCurrentConstellationMaxStarsValue().ToString(),
            AffectedFeature.BOMB_SPAWNERxSPAWN_RATE => GetCurrentBombSpawnerSpawnRateValue() + "s",
            AffectedFeature.BOMB_SPAWNERxFORM => GetCurrentBombSpawnerFormValue() + "/" + MAX_FEATURE_VALUE,
            _ => ""

        }; ;



    }



    //public delegate void DifficultyChangeEvent(DifficultyEventArgs dea);
    //  public event DifficultyChangeEvent OnDifficultyValueChange;

    public static event Action OnBombSpawnerForm;






    public static float BOMB_SPAWN_DELAY = 20f, DISRUPTOR_SPAWN_DELAY = 20, BOMB_SPEED_MULTIPLIER = 20;
    public static float DISRUPTOR_DISABLE_TIME = 10;
    public static int DISRUPTOR_SPAWN_CHANCE = 100;





    public static int CONSTELLATION_SPAWN_RATE = 10;


    public static int DISRUPTOR_DEFAULT_START_HEALTH = 100, DISRUPTOR_START_HEALTH;






    static int bomb_spawner_value = 0;





    void Start()
    {
        DISRUPTOR_START_HEALTH = DISRUPTOR_DEFAULT_START_HEALTH;

    }




    public void EnemyChange()
    {

        // DifficultyEventArgs dif_event_args = new(AffectedFeature.BOMB_SPAWNER, AffectedFeatureBehaviour.FORM, "", AffectedFeatureCircumstance.ENEMY);




    }


    public static void ChangeRandomDifficulty(AffectedFeatureCircumstance circumstance)
    {

        List<AffectedFeature> list = circumstance == AffectedFeatureCircumstance.TOKEN ? TOKEN_CHANGABLE_FEATURES : TIME_CHANGABLE_FEATURES;


        AffectedFeature feature = list[new System.Random().Next(list.Count)];






        FEATURE_VALUE_DICT[feature]++;







        DifficultyEventArgs dif_event_args = new(feature, circumstance);


        if (UICommunication.CanPopup)
        {
            UICommunication.Raise_OnDifficultyValueChange(dif_event_args);

        }
        else
        {
            UICommunication.Enqueue_PopupArguments(dif_event_args);

        }






        if (FEATURE_VALUE_DICT[feature] == MAX_FEATURE_VALUE)
        {
            if (feature == AffectedFeature.BOMB_SPAWNERxFORM)
            {
                OnBombSpawnerForm?.Invoke();


                FEATURE_VALUE_DICT[AffectedFeature.BOMB_SPAWNERxFORM] = 0;


            }
            else
            {
                list.Remove(feature);

            }



        }

    }






    void Update()
    {




        if (UICommunication.Secs == 0) { return; }

        if (UICommunication.Secsf % 100 < Time.deltaTime)
        {

            ChangeRandomDifficulty(AffectedFeatureCircumstance.TIME);


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
    public DifficultyEventArgs(DifficultyManager.AffectedFeature feature, DifficultyManager.AffectedFeatureCircumstance target)
    {



        string[] split = feature.ToString().Split("x");




        string feature_object = split[0].Replace("_", " ");
        string feature_behaviour = split[1].Replace("_", " ");
        Message = feature_object + " : " + feature_behaviour + " [" + DifficultyManager.GetCurrentFormattedValue(feature) + "]";
        Target = target;
    }




}
