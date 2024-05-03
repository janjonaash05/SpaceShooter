using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour 
{



    TextMeshProUGUI txt;

    [SerializeField] bool OnGameOverScreen;
    public int Hundredths { get; private set; }

    public int Secs { get; private set; }
    public float Secsf { get; private set; }


    public int Mins { get; private set; }
    public float Minsf { get; private set; }

    
    void Start()
    {
        
        GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, -27, 0);
        txt = GetComponent<TextMeshProUGUI>();



        if(OnGameOverScreen) txt.text = string.Format("{0:00}:{1:00}:{2:00}", UICommunication.Mins, UICommunication.Secs, UICommunication.Hundredths);
    }

    

    /// <summary>
    /// Calculates seconds, minutes, hundredths elapsed since the level loading(float and floored to int), assigns them to UICommunication, formats them in the TMP text in MM:SS:HH format.
    /// </summary>
    void Update()
    {
        if (OnGameOverScreen) return;

        float time = Time.timeSinceLevelLoad;




        Secsf = time % 60;
        Secs = Mathf.FloorToInt(Secsf);
        Minsf = time / 60;
        Mins = Mathf.FloorToInt(Minsf);


        Hundredths = (int) ( ((time - Secs) * 100) % 99 );

        UICommunication.Assign_TimeValues(Secsf,Secs,Minsf, Mins,Hundredths);


        txt.text = string.Format("{0:00}:{1:00}:{2:00}" ,Mins, Secs, Hundredths);
    }

}
