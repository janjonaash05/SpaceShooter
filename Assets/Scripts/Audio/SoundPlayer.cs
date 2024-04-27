using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class SoundPlayer : MonoBehaviour
{
    

    AudioSource src;

   [SerializeField] ActivityType[] activity_types;


    [SerializeField] Dictionary<ActivityType, AudioClip> activity_types_clip_dict;

    Dictionary<ActivityType, int> activity_muted_multiplier_dict;



    private void Awake()
    {
        activity_muted_multiplier_dict = new();
        foreach (var activityType in activity_types)
        {
            activity_muted_multiplier_dict.Add(activityType, 1);
        
        }




        src = GetComponent<AudioSource>();
        src.spatialBlend = 0.5f;




        OnActivitySoundPlay += VerifyAndPlaySound;
        OnActivitySoundStop += StopSound ;





    }




    private void OnDestroy()
    {

        OnActivitySoundPlay -= VerifyAndPlaySound;
        OnActivitySoundStop -= StopSound;
    }


    void VerifyAndPlaySound(ActivityType activity_type)
    {




        foreach (var activity in activity_types) 
        {
            if (activity == activity_type) 
            {
                src.clip = ACTIVITY_CLIP_DICT[activity_type];

                var settings = ACTIVITY_SOUND_SETTINGS_DICT[activity_type];

                src.pitch = settings.Pitch;
                src.volume = settings.Volume * ( UserDataManager.CURRENT_DATA?.VolumeMultiplier ?? 1) * activity_muted_multiplier_dict[activity_type];
                src.Play();
                
            } 
        }
        
    
    }






    void StopSound(ActivityType activity_type)
    {
        foreach (var activity in activity_types)
        {
            if (activity == activity_type)
            {
               src.Stop();

            }
        }


    }



   




    void MuteOrUnMuteSound(ActivityType activity_type, bool mute)
    {
        foreach (var activity in activity_types)
        {
            if (activity == activity_type)
            {
                activity_muted_multiplier_dict[activity_type] = mute? 0:1;

            }
        }


    }






   
}
