using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopupDisplay : MonoBehaviour
{

    [SerializeField]  TextMeshProUGUI txt;
    [SerializeField] Vector2 font_size_range;
    [SerializeField] float popup_speed;
    Color32 default_color = new(255, 255, 225, 255), alert_color_friendly = new(15, 129, 32, 255), alert_color_enemy = new(158, 12, 26, 255);






    void Start()
    {


      





        GetComponent<RectTransform>().anchoredPosition = new Vector3(-122, -80, 0);
        txt = GetComponent<TextMeshProUGUI>();
        txt.color = default_color;
        txt.text = "";





        UICommunication.OnDifficultyValueChange += Popup;



    }

   
    void Update()
    {

    }


    void Popup(DifficultyEventArgs e)
    {
        IEnumerator popup()
        {

            txt.fontSize = font_size_range.x;
            while (txt.fontSize < font_size_range.y)
            {

                txt.fontSize += popup_speed;

                yield return null;
            }
            txt.fontSize = font_size_range.y;


            Color alert_color = (e.Affected == AffectedTarget.ENEMY) ? alert_color_enemy : alert_color_friendly;
            for (int i = 0; i < 9; i++)
            {
                txt.color = (i % 2 == 0) ? alert_color : default_color;
                yield return new WaitForSeconds(0.15f);
            }


            while (txt.fontSize > font_size_range.x)
            {

                txt.fontSize -= popup_speed;

                yield return null;
            }

            txt.fontSize = font_size_range.x;


            UICommunication.CanPopup = true; 
            UICommunication.Dequeue_PopupCall();


         









        }




        UICommunication.CanPopup = false;
        txt.text = e.Message;
        StartCoroutine(popup());

    }


    







}








