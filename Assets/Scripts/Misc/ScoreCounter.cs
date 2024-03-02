 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] static TextMeshProUGUI txt;
    void Start()
    {


        GetComponent<RectTransform>().anchoredPosition = new Vector3(120, -27, 0);
        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "Score: "+UICommunication.Score;






        UICommunication.OnScoreChange += () => txt.text = "Score: " + UICommunication.Score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    /*
  public static void Increase(int amount) {

       
    score += amount;
      
        txt.text = "Score " + score;
    }
    */
}
