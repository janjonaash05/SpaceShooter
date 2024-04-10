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

    public readonly Dictionary<ActivityType, SoundSettings> ACTIVITY_SOUND_SETTINGS_DICT= new() 
    {
        { ActivityType. },
    
    
    
    };


    private void Awake()
    {
        ACTIVITY_CLIP_DICT = new();
        for (int i = 0; i < keys_activities.Count; i++)
        {
            ACTIVITY_CLIP_DICT.Add(keys_activities[i], values_clips[i]);
        
        
        }




    }





    public enum ActivityType 
    {
        TURRET_CONTROL_CLICK_1, TURRET_CONTROL_CLICK_2, 
        SLIDER_CONTROL_CLICK, SLIDER_BOLT_CLICK, SLIDER_FULL_AUTO_CLICK,
        HARPOON_CONTROL_CLICK,
        UPGRADE_STATION_ARROW_UP_CLICK, UPGRADE_STATION_ARROW_DOWN_CLICK, UPGRADE_STATION_FACE_CLICK,
        HELPER_STATION_ARROW_UP_CLICK, HELPER_STATION_ARROW_DOWN_CLICK, HELPER_STATION_FACE_CLICK,


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
