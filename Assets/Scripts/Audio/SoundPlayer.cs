using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    AudioSource src;

   [SerializeField] AudioManager.ActivityType[] activity_types;


    [SerializeField] Dictionary<AudioManager.ActivityType, AudioClip> activity_types_clip_dict;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        src.spatialBlend = 1f;




        AudioManager.OnActivitySoundPlay += VerifyAndPlaySound;





    }




    private void OnDestroy()
    {

        AudioManager.OnActivitySoundPlay -= VerifyAndPlaySound;
    }


    void VerifyAndPlaySound(AudioManager.ActivityType activity_type)
    {




        foreach (var activity in activity_types) 
        {
            if (activity == activity_type) 
            {
                src.clip = AudioManager.ACTIVITY_CLIP_DICT[activity_type];

                var settings = AudioManager.ACTIVITY_SOUND_SETTINGS_DICT[activity_type];

                src.pitch = settings.Pitch;
                src.volume = settings.Volume * UserDataManager.VOLUME_MULTIPLIER;
                src.Play();
                
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
