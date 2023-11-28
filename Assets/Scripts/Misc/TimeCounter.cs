using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour 
{



    [SerializeField] static TextMeshProUGUI txt;


    public int Hundredths { get; private set; }

    public int Secs { get; private set; }
    public float Secsf { get; private set; }


    public int Mins { get; private set; }
    public float Minsf { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, -27, 0);
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

       
        Secsf = Time.time % 60;
        Secs = Mathf.FloorToInt(Secsf);
        Minsf = Time.time / 60;
        Mins = Mathf.FloorToInt(Minsf);


        Hundredths = (int) ( ((Time.time - Secs) * 100) % 99 );

        UICommunication.Assign_TimeValues(Secsf,Secs,Minsf, Mins,Hundredths);


        txt.text = string.Format("{0:00}:{1:00}:{2:00}" ,Mins, Secs, Hundredths);
    }
}
