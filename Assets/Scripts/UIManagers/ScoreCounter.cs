 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    
     TextMeshProUGUI txt;



   [SerializeField] bool OnGameOverScreen;




    /// <summary>
    /// Assigns ghe anchored position and if the object isn't OnGameOverScreen, sets up functionality to change the text on score change.
    /// </summary>
    void Start()
    {


        GetComponent<RectTransform>().anchoredPosition = new Vector3(120, -27, 0);
        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "Score: " + UICommunication.Score;



        if (OnGameOverScreen) return;


        UICommunication.OnScoreChange += () => txt.text = "Score: " + UICommunication.Score;
    }

 
}
