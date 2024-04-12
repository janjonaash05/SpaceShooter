using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}



public record SoundSettings(float Volume, float Pitch) { };






public class AudioManager : MonoBehaviour
{
    [SerializeField] List<ActivityType> keys_activities;
    [SerializeField] List<AudioClip> values_clips;



   public static Dictionary<ActivityType, AudioClip> ACTIVITY_CLIP_DICT { get; private set; }


    public static Dictionary<ActivityType, SoundSettings> ACTIVITY_SOUND_SETTINGS_DICT { get; private set; } = new();

    private void Awake()
    {
        ACTIVITY_CLIP_DICT = new();
        for (int i = 0; i < keys_activities.Count; i++)
        {
            ACTIVITY_CLIP_DICT.Add(keys_activities[i], values_clips[i]);
        
        
        }


        SoundSettings control_click_settings = new(0.75f,0.47f);
        SoundSettings target_bomb_settings = new(1f, 1f);
        
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_1, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_2, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_CONTROL_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_TARGET_BOMB_1, target_bomb_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_TARGET_BOMB_2, target_bomb_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_BOLT_SHOT, new(1,0.75f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_FULL_AUTO_SHOT, new(0.75f, 1f));


        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.HARPOON_CONTROL_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.UPGRADE_STATION_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.HELPER_STATION_CLICK, control_click_settings);

        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.UPGRADE_STATION_UPGRADE_CLICK, new(1, 1)); 
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.UPGRADE_STATION_FINAL_UPGRADE_CLICK, new(1, 1));

        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.BOMB_EXPLOSION, new(0.25f, 1f));



    }





    public enum ActivityType 
    {
        TURRET_CONTROL_CLICK_1, TURRET_CONTROL_CLICK_2, 
        SLIDER_CONTROL_CLICK, // SLIDER_BOLT_CLICK, SLIDER_FULL_AUTO_CLICK,
        HARPOON_CONTROL_CLICK,
       UPGRADE_STATION_CLICK, UPGRADE_STATION_UPGRADE_CLICK, UPGRADE_STATION_FINAL_UPGRADE_CLICK,
        HELPER_STATION_CLICK, 

        TURRET_TARGET_BOMB_1, TURRET_TARGET_BOMB_2,
        SLIDER_BOLT_SHOT, SLIDER_FULL_AUTO_SHOT,

        BOMB_EXPLOSION



    }








    public static event Action<ActivityType> OnActivitySoundPlay;






    public static void PlayActivitySound(ActivityType type) 
    {


        OnActivitySoundPlay?.Invoke(type);



    }









    void Start()
    {
        




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
