using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    AudioSource src;

   [SerializeField] ActivityType[] activity_types;


    [SerializeField] Dictionary<ActivityType, AudioClip> activity_types_clip_dict;

    private void Awake()
    {
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
                src.volume = settings.Volume * ( UserDataManager.CURRENT_DATA?.VolumeMultiplier ?? 1);
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




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
