using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TokenCounter : MonoBehaviour
{
    

   TextMeshProUGUI txt;
    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(122, -80, 0);


        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "Tokens: " + UICommunication.Tokens;






        UICommunication.OnTokensChange += () => txt.text = "Tokens: " + UICommunication.Tokens;
    }

    
    void Update()
    {
        
    }
}
