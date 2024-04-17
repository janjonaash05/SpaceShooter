using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreEnumerable
{
    
    int ScoreReward();
    public bool DisabledRewards { get; set; }

}
