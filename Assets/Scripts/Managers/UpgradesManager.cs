using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{

    public enum UpgradeType { TURRET_CAPACITY, TURRET_RECHARGE, SLIDER_RECHARGE, SLIDER_SPEED }

    public const int MAX_VALUE = 4;

    public static readonly Dictionary<UpgradeType, int> UPGRADE_VALUE_DICT = new() { { UpgradeType.TURRET_CAPACITY, 0 }, { UpgradeType.TURRET_RECHARGE, 0 }, { UpgradeType.SLIDER_RECHARGE, 0 }, { UpgradeType.SLIDER_SPEED, 0 }, };


    public static event Action OnTurretCapacityValueChange;
    public static event Action OnTurretRechargeValueChange;
    public static event Action OnSliderRechargeValueChange;
    public static event Action OnSliderSpeedValueChange;








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
        {0,0.2f},
        {1,0.175f },
        {2,0.15f },
        {3,0.1f },
        {4,8.0075f }


    };


    public static readonly Dictionary<int, (float full_auto, float bolt)> SLIDER_RECHARGE_DEGREE_VALUE_DICT = new()
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


















    public static bool IncreaseValue(UpgradeType type)
    {
        if (UPGRADE_VALUE_DICT[type] == MAX_VALUE) { return false; }


        UPGRADE_VALUE_DICT[type]++;

        Action toExecute = type switch
        {
            UpgradeType.TURRET_CAPACITY => () => OnTurretCapacityValueChange?.Invoke(),
            UpgradeType.TURRET_RECHARGE => () => OnTurretRechargeValueChange?.Invoke(),
            UpgradeType.SLIDER_RECHARGE => () => OnSliderRechargeValueChange?.Invoke(),
            UpgradeType.SLIDER_SPEED => () => OnSliderSpeedValueChange?.Invoke(),
            _ => () => { }
        }; ; ;


        toExecute();



        return true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
