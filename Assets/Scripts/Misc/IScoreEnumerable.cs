using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreEnumerable
{
    // Start is called before the first frame update
    int ScoreReward();
    public bool DisabledRewards { get; set; }

}
