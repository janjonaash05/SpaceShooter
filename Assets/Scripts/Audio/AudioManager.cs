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



public record SoundSettings(float Volume, float Pitch) { public override string ToString() => "vol " + Volume + " pitch" + Pitch; };




[System.Serializable]
public class ActivityClipPair
{


    public AudioManager.ActivityType Activity { get; private set; }
    public AudioClip Clip { get; private set; }


}


//AudioClip audioClip = Resources.Load <AudioClip> ("Music/Song_Name");



public class AudioManager : MonoBehaviour
{
    [SerializeField] List<ActivityType> keys_activities;
    [SerializeField] List<AudioClip> values_clips;


    [SerializeField] List<ActivityClipPair> values_clip_pairs;


    //   public static Dictionary<ActivityType, AudioClip> ACTIVITY_CLIP_DICT { get; private set; }


    public static Dictionary<ActivityType, SoundSettings> ACTIVITY_SOUND_SETTINGS_DICT { get; private set; } = new();





    public static Dictionary<ActivityType, AudioClip> ACTIVITY_CLIP_DICT = new();


    const string path = "Sounds/";

    //AudioClip audioClip = Resources.Load <AudioClip> ("Music/Song_Name");
    public static void LoadResources()
    {

        AudioClip control_click = Resources.Load<AudioClip>(path + "Controls/clicking");
        AudioClip laser_blast = Resources.Load<AudioClip>(path + "Turret/turret_laser_blast");
        AudioClip upgrade = Resources.Load<AudioClip>(path + "Controls/upgrade");
        AudioClip upgrade_final = Resources.Load<AudioClip>(path + "Controls/upgrade_final");



        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_1, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_2, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.SLIDER_CONTROL_CLICK, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.HARPOON_CONTROL_CLICK, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.UPGRADE_STATION_CLICK, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.HELPER_STATION_CLICK, control_click);
        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_TARGET_BOMB_1, laser_blast);
        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_TARGET_BOMB_2, laser_blast);
        ACTIVITY_CLIP_DICT.Add(ActivityType.SLIDER_BOLT_SHOT, Resources.Load<AudioClip>(path + "Slider/slider_bolt_shot"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SLIDER_FULL_AUTO_SHOT, Resources.Load<AudioClip>(path + "Slider/slider_full_auto_shot"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.UPGRADE_STATION_UPGRADE_CLICK, upgrade);
        ACTIVITY_CLIP_DICT.Add(ActivityType.UPGRADE_STATION_FINAL_UPGRADE_CLICK, upgrade_final);
        ACTIVITY_CLIP_DICT.Add(ActivityType.HELPER_STATION_HELPER_SPAWN, upgrade_final);
        ACTIVITY_CLIP_DICT.Add(ActivityType.HELPER_STATION_HELPER_COUNTDOWN, upgrade);
        ACTIVITY_CLIP_DICT.Add(ActivityType.HARPOON_LAUNCH, Resources.Load<AudioClip>(path + "Harpoon/harpoon_launch"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.HARPOON_RETRACTION, Resources.Load<AudioClip>(path + "Harpoon/harpoon_retract"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SPINNER_CHARGE_UP, Resources.Load<AudioClip>(path + "Spinner/spinner_charge_up"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SPINNER_SHOOT, Resources.Load<AudioClip>(path + "Spinner/spinner_shoot"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.SHIELD_BLOCK, Resources.Load<AudioClip>(path + "Shield/shield_hit"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SHIELD_PASS, Resources.Load<AudioClip>(path + "Shield/shield_hit"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SHIELD_CHARGE_SPAWN, Resources.Load<AudioClip>(path + "Shield/shield_charge_recharge"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.SLIDER_FULL_AUTO_CHARGE_SPAWN, Resources.Load<AudioClip>(path + "Slider/slider_full_auto_charge"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SLIDER_BOLT_RECHARGE_START, Resources.Load<AudioClip>(path + "Slider/slider_bolt_recharge"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_CONTROLS_DISABLED, Resources.Load<AudioClip>(path + "Controls/controls_disabled"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_CHARGE_SPAWN_1, Resources.Load<AudioClip>(path + "Turret/turret_charge_recharge"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.TURRET_CHARGE_SPAWN_2, Resources.Load<AudioClip>(path + "Turret/turret_charge_recharge"));




        ACTIVITY_CLIP_DICT.Add(ActivityType.STAR_SPAWN, Resources.Load<AudioClip>(path + "Star/star_emerge"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.STAR_DESTROYED, Resources.Load<AudioClip>(path + "Star/star_emerge"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.STAR_FALL, Resources.Load<AudioClip>(path + "Star/star_fall"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.DISRUPTOR_SPAWN, Resources.Load<AudioClip>(path + "Disruptor/disruptor_ambient"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.DISRUPTOR_CHARGE_UP, Resources.Load<AudioClip>(path + "Disruptor/disruptor_charge_up"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.DISRUPTOR_SHOOT, Resources.Load<AudioClip>(path + "Disruptor/disruptor_shoot"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.DISRUPTOR_DESTROYED, Resources.Load<AudioClip>(path + "Disruptor/disruptor_explosion"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.SUPERNOVA_CHARGE_UP, Resources.Load<AudioClip>(path + "Supernova/supernova_charge_up"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.SUPERNOVA_SHOOT, Resources.Load<AudioClip>(path + "Supernova/supernova_shot"));


        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_CAUGHT_FRIENDLY, Resources.Load<AudioClip>(path + "Token/token_caught"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_CAUGHT_ENEMY, Resources.Load<AudioClip>(path + "Token/token_caught"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_DESTROYED_FRIENDLY, Resources.Load<AudioClip>(path + "Token/token_destroyed"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_DESTROYED_ENEMY, Resources.Load<AudioClip>(path + "Token/token_destroyed"));

        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_SPAWN, Resources.Load<AudioClip>(path + "Token/token_spawn"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.TOKEN_TRANSPORTED, Resources.Load<AudioClip>(path + "Token/token_transported"));






        ACTIVITY_CLIP_DICT.Add(ActivityType.BLACK_HOLE_SPAWN, Resources.Load<AudioClip>(path + "Helpers/black_hole"));
        ACTIVITY_CLIP_DICT.Add(ActivityType.EMP_SPAWN, Resources.Load<AudioClip>(path + "Helpers/EMP"));


        foreach (ActivityType activity in Enum.GetValues(typeof(ActivityType)))
        {
            if (!ACTIVITY_CLIP_DICT.ContainsKey(activity))
            {
                ACTIVITY_CLIP_DICT.Add(activity, Resources.Load<AudioClip>(path));
            }




        }





    }


    


    public static void LoadSettings()
    {




        SoundSettings control_click_settings = new(0.75f, 0.47f);
        SoundSettings target_bomb_settings = new(1f, 1f);



        if (ACTIVITY_SOUND_SETTINGS_DICT.Count != 0) return;

        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_1, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_CONTROL_CLICK_2, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_CONTROL_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_TARGET_BOMB_1, target_bomb_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_TARGET_BOMB_2, target_bomb_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_BOLT_SHOT, new(1, 0.75f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_FULL_AUTO_SHOT, new(0.75f, 1f));


        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.HARPOON_CONTROL_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.UPGRADE_STATION_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.HELPER_STATION_CLICK, control_click_settings);
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.BOMB_EXPLOSION, new(0.25f, 1f));


        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SPINNER_CHARGE_UP, new(0.85f, 0.5f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SPINNER_SHOOT, new(1f, 1.2f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SHIELD_PASS, new(1f, 0.5f));

        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.SLIDER_FULL_AUTO_CHARGE_SPAWN, new(0.75f, 0.5f));

        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.TURRET_CONTROLS_DISABLED, new(0.35f, 0.9f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.STAR_SPAWN, new(0.8f, 0.9f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.STAR_DESTROYED, new(0.5f, 0.5f));
        ACTIVITY_SOUND_SETTINGS_DICT.Add(ActivityType.STAR_FALL, new(1f, 0.75f));

        foreach (ActivityType activity in Enum.GetValues(typeof(ActivityType)))
        {
            if (!ACTIVITY_SOUND_SETTINGS_DICT.ContainsKey(activity))
            {
                ACTIVITY_SOUND_SETTINGS_DICT.Add(activity, new(1f, 1f));
            }




        }





    }

    private void Awake()
    {

        /*
        ACTIVITY_CLIP_DICT = new();
        for (int i = 0; i < keys_activities.Count; i++)
        {
            ACTIVITY_CLIP_DICT.Add(keys_activities[i], values_clips[i]);


        }
        */

        LoadResources();

        LoadSettings();



    }





    public enum ActivityType
    {
        TURRET_CONTROL_CLICK_1 = 0, TURRET_CONTROL_CLICK_2 = 1,
        SLIDER_CONTROL_CLICK = 2,
        HARPOON_CONTROL_CLICK = 3,
        UPGRADE_STATION_CLICK = 4, UPGRADE_STATION_UPGRADE_CLICK = 5, UPGRADE_STATION_FINAL_UPGRADE_CLICK = 6,
        HELPER_STATION_CLICK = 7, HELPER_STATION_HELPER_SPAWN = 8, HELPER_STATION_HELPER_COUNTDOWN = 9,

        TURRET_TARGET_BOMB_1 = 10, TURRET_TARGET_BOMB_2 = 11,
        SLIDER_BOLT_SHOT = 12, SLIDER_FULL_AUTO_SHOT = 13,

        BOMB_EXPLOSION = 14,

        HARPOON_LAUNCH = 15, HARPOON_RETRACTION = 16,

        SPINNER_CHARGE_UP = 17, SPINNER_SHOOT = 18,
        SHIELD_BLOCK = 19, SHIELD_PASS = 20,
        SLIDER_FULL_AUTO_CHARGE_SPAWN = 21, SLIDER_BOLT_RECHARGE_START = 22,
        TURRET_CHARGE_SPAWN_1 = 231, TURRET_CHARGE_SPAWN_2 = 232, SHIELD_CHARGE_SPAWN = 24,
        TOKEN_CAUGHT_FRIENDLY = 251, TOKEN_CAUGHT_ENEMY = 252, TOKEN_DESTROYED_FRIENDLY = 261, TOKEN_DESTROYED_ENEMY = 262, TOKEN_TRANSPORTED = 271, TOKEN_SPAWN = 272,
        DISRUPTOR_SPAWN = 28, DISRUPTOR_CHARGE_UP = 29, DISRUPTOR_DESTROYED = 30, DISRUPTOR_SHOOT = 31,
        STAR_SPAWN = 32, STAR_CHARGE_UP = 33, STAR_DESTROYED = 34, STAR_FALL = 35, SUPERNOVA_SHOOT = 36, SUPERNOVA_CHARGE_UP = 40,
        TURRET_CONTROLS_DISABLED = 37,
        BLACK_HOLE_SPAWN = 38, EMP_SPAWN = 39

    }








    public static event Action<ActivityType> OnActivitySoundPlay, OnActivitySoundStop;






    public static void PlayActivitySound(ActivityType type)
    {



        OnActivitySoundPlay?.Invoke(type);



    }



    public static void StopActivitySound(ActivityType type)
    {


        OnActivitySoundStop?.Invoke(type);



    }









    void Start()
    {





    }


    void Update()
    {

    }
}
