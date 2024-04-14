using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI easy_label;
    [SerializeField] TextMeshProUGUI normal_label;
    [SerializeField] TextMeshProUGUI hard_label;


    void Start()
    {

        var data = UserDataManager.CURRENT_DATA;


        easy_label.text = GenerateText(data.BestScores[0], data.BestTimeEasy); 
        normal_label.text = GenerateText(data.BestScores[1], data.BestTimeNormal);
        hard_label.text = GenerateText(data.BestScores[2], data.BestTimeHard);

    }


    string GenerateText(int score, int[] time) 
    {
        return "Score: " + score + "\n" + "Time: " + string.Format("{0:00}:{1:00}:{2:00}", time[0], time[1], time[2]);


    }
    
}
