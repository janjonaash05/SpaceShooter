using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreEnumerable
{
    
    int CalculateScoreReward();
    bool DisabledRewards { get; set; }


    public int ValidateScoreReward()
    {
        if (!DisabledRewards) 
        {
            DisabledRewards = true;
            return CalculateScoreReward();
        }

        return 0;
    }

}
