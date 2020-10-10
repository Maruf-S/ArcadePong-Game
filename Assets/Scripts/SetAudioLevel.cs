using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetAudioLevel : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float level){
        if(gameObject.name=="Music"){
            mixer.SetFloat("MusicVolume",Mathf.Log10(level)*20);
        }
        else if(gameObject.name == "SoundEffects"){
            mixer.SetFloat("SoundEffectsVolume",Mathf.Log10(level)*20);            
        }
    }

}
