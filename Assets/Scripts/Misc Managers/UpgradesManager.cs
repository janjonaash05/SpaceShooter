using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager
{

    public enum UpgradeType { TURRET_CAPACITY, TURRET_RECHARGE, SLIDER_RECHARGE, SLIDER_SPEED }

    public const int MAX_VALUE = 4;

    public static readonly Dictionary<UpgradeType, int> UPGRADE_VALUE_DICT = new() { { UpgradeType.TURRET_CAPACITY, 0 }, { UpgradeType.TURRET_RECHARGE, 0 }, { UpgradeType.SLIDER_RECHARGE, 0 }, { UpgradeType.SLIDER_SPEED, 0 }, };


    public static event Action OnTurretCapacityValueChange;
    public static event Action OnTurretRechargeValueChange;
    public static event Action OnSliderRechargeValueChange;
    public static event Action OnSliderSpeedValueChange;


    public static event Action OnShieldMaxCapacityValueChange;


    public static int SHIELD_MAX_CAPACITY { get; private set; } = 4;

    public static void Awake() => SHIELD_MAX_CAPACITY = 4;



    public static int GetCurrentTurretCapacityValue()
    {
        return TURRET_CAPACITY_DEGREE_VALUE_DICT[UPGRADE_VALUE_DICT[UpgradeType.TURRET_CAPACITY]];
    }

    public static float GetCurrentTurretRechargeValue()
    {
        return TURRET_RECHARGE_DEGREE_VALUE_DICT[UPGRADE_VALUE_DICT[UpgradeType.TURRET_RECHARGE]];
    }




    public static (int full_auto, float bolt) GetCurrentSliderRechargeValue()
    {

        return SLIDER_RECHARGE_DEGREE_VALUE_DICT[UPGRADE_VALUE_DICT[UpgradeType.SLIDER_RECHARGE]];
    }


    public static (float full_auto, float bolt) GetCurrentSliderSpeedValue()
    {

        return SLIDER_SPEED_DEGREE_VALUE_DICT[UPGRADE_VALUE_DICT[UpgradeType.SLIDER_SPEED]];
    }






    public static readonly Dictionary<int, int> TURRET_CAPACITY_DEGREE_VALUE_DICT = new()
    {
        {0,4 },
        {1,5 },
        {2,6 },
        {3,7 },
        {4,8 }


    };

    public static readonly Dictionary<int, float> TURRET_RECHARGE_DEGREE_VALUE_DICT = new()
    {
        {0,1f},
        {1,0.8f},
        {2,0.6f },
        {3,0.4f },
        {4,0.2f }


    };


    public static readonly Dictionary<int, (int full_auto, float bolt)> SLIDER_RECHARGE_DEGREE_VALUE_DICT = new()
    {
        {0, (500,0.25f) },
        {1,(375,0.375f) },
        {2,(250,0.5f) },
        {3,(175,0.625f)},
        {4,(100,0.75f)}

    };


    public static readonly Dictionary<int, (int full_auto, int bolt)> SLIDER_SPEED_DEGREE_VALUE_DICT = new()
    {
        {0, (250,75) },
        {1,(300,100) },
        {2,(350,125) },
        {3,(400,150)},
        {4,(450,175)}

    };




   

    public static void ResetValues()
    {
        var keySet = UPGRADE_VALUE_DICT.Keys.ToList();

        foreach (var key in keySet)
        {



            UPGRADE_VALUE_DICT[key] = 0;
        }

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IncreaseValue(UpgradeType type)
    {
        if (UPGRADE_VALUE_DICT[type] == MAX_VALUE) { return false; }


        AudioManager.ActivityType activity_type = AudioManager.ActivityType.UPGRADE_STATION_UPGRADE_CLICK;

        UPGRADE_VALUE_DICT[type]++;
        if (UPGRADE_VALUE_DICT[type] == MAX_VALUE) 
        {
            SHIELD_MAX_CAPACITY++; OnShieldMaxCapacityValueChange?.Invoke();
            activity_type = AudioManager.ActivityType.UPGRADE_STATION_FINAL_UPGRADE_CLICK;
        }



        Action toExecute = type switch
        {
            UpgradeType.TURRET_CAPACITY => () => OnTurretCapacityValueChange?.Invoke(),
            UpgradeType.TURRET_RECHARGE => () => OnTurretRechargeValueChange?.Invoke(),
            UpgradeType.SLIDER_RECHARGE => () => OnSliderRechargeValueChange?.Invoke(),
            UpgradeType.SLIDER_SPEED => () => OnSliderSpeedValueChange?.Invoke(),
            _ => () => { }
        }; ; ;


        toExecute();

        AudioManager.PlayActivitySound(activity_type);

        return true;

    }

    
   
}
