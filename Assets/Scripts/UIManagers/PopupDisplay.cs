using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class PopupDisplay : MonoBehaviour
{

    [SerializeField]  TextMeshProUGUI txt;
    [SerializeField] Vector2 font_size_range;
    [SerializeField] float popup_speed;
    Color32 default_color = new(255, 255, 225, 255); //, alert_color_friendly = new(15, 129, 32, 255), alert_color_enemy = new(158, 12, 26, 255);
    [SerializeField] Color alert_time, alert_token;





    void Start()
    {

        GetComponent<RectTransform>().anchoredPosition = new Vector3(-122, -80, 0);
        txt = GetComponent<TextMeshProUGUI>();
        txt.color = default_color;
        txt.text = "";

        UICommunication.OnDifficultyValueChange += Popup;

    }





    private void OnDestroy()
    {
        UICommunication.OnDifficultyValueChange -= Popup;
    }



    /// <summary>
    /// Assigns the text and starts the PopupProcess coroutine.
    /// </summary>
    /// <param name="e"></param>
    void Popup(DifficultyEventArgs e)
    {
       




       
        txt.text = e.Message;
        StartCoroutine(PopupProcess(e));

    }



    /// <summary>
    /// <para>Sets CanPopup to false.</para>
    /// <para>Sets the font size to min, keeps increasing it by popup_speed until it's near max, then sets it to max. </para>
    /// <para>Assigns the alert color based on the AffectedFeatureCircumstance.</para>
    /// <para>Alternates between the alert and default color with a time delay.</para>
    /// <para>Keeps decreasing the font size by popup_speed until it's near min, then sets it to min. </para>
    /// <para>Sets CanPopup to true and calls Dequeue_PopupCall.</para>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    IEnumerator PopupProcess(DifficultyEventArgs e)
    {
        UICommunication.CanPopup = false;

        txt.fontSize = font_size_range.x;
        while (txt.fontSize < font_size_range.y)
        {

            txt.fontSize += popup_speed;

            yield return null;
        }
        txt.fontSize = font_size_range.y;


        Color alert_color = (e.Target == DifficultyManager.AffectedFeatureCircumstance.TOKEN) ? alert_token : alert_time;
        for (int i = 0; i < 19; i++)
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







}








