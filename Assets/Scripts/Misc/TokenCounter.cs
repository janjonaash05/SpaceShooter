using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TokenCounter : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI txt;
    void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(122, -80, 0);


        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "Tokens: " + UICommunication.Tokens;






        UICommunication.OnScoreChange += () => txt.text = "Tokens: " + UICommunication.Tokens;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
