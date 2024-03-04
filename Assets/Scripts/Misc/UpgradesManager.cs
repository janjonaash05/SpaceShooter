using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{

    public enum UpgradeType {TURRET_CAPACITY, TURRET_RECHARGE, SLIDER_RECHARGE, SLIDER_SPEED }

    const int MAX_VALUE = 4;

    public static Dictionary<UpgradeType, int> UPGRADE_VALUE_DICT = new() { { UpgradeType.TURRET_CAPACITY, 0 }, { UpgradeType.TURRET_RECHARGE, 0 }, { UpgradeType.SLIDER_RECHARGE, 0 }, { UpgradeType.SLIDER_SPEED, 0 }, };




    public static bool IncreaseValue(UpgradeType type) 
    {
        if (UPGRADE_VALUE_DICT[type] == MAX_VALUE) { return false; }


        UPGRADE_VALUE_DICT[type]++;

        return true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
