using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AmbienceChange : MonoBehaviour
{
    


    AudioSource src;


    private void OnDestroy()
    {
        CoreCommunication.OnParentValueChangedCore -= SetPitch;
    }



    readonly Dictionary<int, float> degree_pitch_dict = new()
    {
        {5,1f },
        {4,0.8f },
        {3,0.6f },
        {2,0.4f },
        {1,0.2f },
        {0,0f },

    };




    const float DEFAULT_VOLUME = 0.386f;



    /// <summary>
    /// Sets the volume to a default constant multiplied by UserData volume multiplier, if no such object exists then by 1
    /// </summary>
    void Start()
    {
        src = GetComponent<AudioSource>();

        src.volume = DEFAULT_VOLUME * ( UserDataManager.CURRENT_DATA?.VolumeMultiplier ?? 1);






        CoreCommunication.OnParentValueChangedCore += SetPitch;
    }



    void SetPitch() 
    {
        src.pitch = degree_pitch_dict[ CoreCommunication.CORE_INDEX_HOLDER.Parent];
    }

    
    
}
