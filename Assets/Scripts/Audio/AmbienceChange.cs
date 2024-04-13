using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AmbienceChange : MonoBehaviour
{
    // Start is called before the first frame update


    AudioSource src;


    private void OnDestroy()
    {
        CoreCommunication.OnParentValueChangedCore -= SetPitch;
    }



    Dictionary<int, float> degree_pitch_dict = new()
    {
        {5,1f },
        {4,0.8f },
        {3,0.6f },
        {2,0.4f },
        {1,0.2f },
        {0,0f },

    };




    const float DEFAULT_VOLUME = 0.386f;

    void Start()
    {
        src = GetComponent<AudioSource>();

        src.volume = DEFAULT_VOLUME * UserDataManager.VOLUME_MULTIPLIER;






        CoreCommunication.OnParentValueChangedCore += SetPitch;
    }



    void SetPitch() 
    {
        src.pitch = degree_pitch_dict[ CoreCommunication.CORE_INDEX_HOLDER.Parent];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
