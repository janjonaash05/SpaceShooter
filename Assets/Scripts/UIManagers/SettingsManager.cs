using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update




   [SerializeField] TextMeshProUGUI volume_label;

    [SerializeField] TMP_Dropdown resolutions_dropdown;



    Resolution[] resolutions;


    AudioSource src;





 
    




    private void Awake()
    {

        src = GetComponent<AudioSource>();

        

        resolutions = Screen.resolutions;

        resolutions_dropdown.ClearOptions();

        List<string> options = resolutions.ToList().Select(x => x.width+" x "+ x.height).ToList() ;

      int current_res_index = Array.IndexOf(resolutions, Screen.currentResolution);



        resolutions_dropdown.AddOptions(options);

        resolutions_dropdown.value = current_res_index;
        resolutions_dropdown.RefreshShownValue();


    }



    public void SetResolution(int index) => Screen.SetResolution(resolutions[index].width, resolutions[index].height, full_screen);




    bool full_screen;
    public void ChangeFullScreen(bool val) 
    {
        Screen.fullScreen = val;
        full_screen = val;
    
    }
    float volume_multiplier;
    public void SetVolume(float val)
    {
        volume_label.text = "Volume: " + Mathf.RoundToInt(val*100);
        volume_multiplier = val;


        src.volume = 0.5f * volume_multiplier;
        src.Play();




        UserDataManager.VOLUME_MULTIPLIER = volume_multiplier;



    }





}
