using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;






public class SettingsData
{
    public float Volume; public int[] Resolution; public bool Fullscreen;






    public SettingsData(float Volume, int[] Resolution, bool Fullscreen)
    {
        this.Volume = Volume;
        this.Resolution = Resolution;
        this.Fullscreen = Fullscreen;
    }

    public void SetVolume(float val) => Volume = val;
    public void SetResolution(int[] val) => Resolution = val;
    public void SetFullscreen(bool val) => Fullscreen = val;



    public override string ToString()
    {
        return "SettingsData: VOL " + Volume + ", RES [" + Resolution[0] + "x" + Resolution[1] + "] FULL " + Fullscreen;
    }


}


public class SettingsManager : MonoBehaviour
{
    




    [SerializeField] TextMeshProUGUI volume_label;
    [SerializeField] Slider volume_slider;

    [SerializeField] TMP_Dropdown resolutions_dropdown;
    [SerializeField] Toggle fullscreen_toggle;






    Resolution[] resolutions;


    AudioSource src;









    public static SettingsData LoadSettings { get; private set; }
    public static SettingsData NewSettings { get; private set; }


    public static void DiscardSettings()
    {
        UserDataManager.CURRENT_DATA.SetVolumeMultiplier(LoadSettings.Volume);
        NewSettings = new(LoadSettings.Volume, LoadSettings.Resolution, LoadSettings.Fullscreen);


    }






    void Load()
    {


        Debug.Log(UserDataManager.CURRENT_DATA + " UserData on Settings load");


        var data = UserDataManager.CURRENT_DATA;
        LoadSettings = new(data.VolumeMultiplier, data.Resolution, data.Fullscreen);
        NewSettings = new(LoadSettings.Volume, LoadSettings.Resolution, LoadSettings.Fullscreen); ;


        volume_slider.value = data.VolumeMultiplier;
        SetVolume(data.VolumeMultiplier);

        
        fullscreen_toggle.isOn = data.Fullscreen;











        resolutions = Screen.resolutions;

        resolutions_dropdown.ClearOptions();

        List<string> options = resolutions.ToList().Select(x => x.width + " x " + x.height).ToList();

        int current_res_index = Array.IndexOf(options.ToArray(), data.Resolution[0]+" x "+ data.Resolution[1]);

        resolutions_dropdown.AddOptions(options);

        resolutions_dropdown.value = current_res_index;




        resolutions_dropdown.RefreshShownValue();




        Console.WriteLine("OnSettingsLoad "+ LoadSettings);

    }



    private void Start()
    {
        src = GetComponent<AudioSource>();
        Load();



       



      


        


    }



    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, NewSettings.Fullscreen);

        NewSettings.SetResolution(new int[] { resolutions[index].width, resolutions[index].height });
        //Debug.LogError("SET res to " + NewSettings.Resolution[0]+" "+ NewSettings.Resolution[1]);


    }





    public void ChangeFullScreen(bool val)
    {
        Screen.fullScreen = val;
        NewSettings.SetFullscreen(val);
      //  Debug.LogError("SET fs to " + NewSettings.Fullscreen);

    }

    public void SetVolume(float val)
    {
        volume_label.text = "Volume: " + Mathf.RoundToInt(val * 100);



        src.volume = 0.5f * val;
        src.Play();

        NewSettings.SetVolume(val);


        UserDataManager.CURRENT_DATA.SetVolumeMultiplier(NewSettings.Volume);
      //  Debug.LogError("SET vol to val:"+val +" newsettings: "+ NewSettings.Volume+ " userdata: "+ UserDataManager.CURRENT_DATA.VolumeMultiplier);


    }





}
