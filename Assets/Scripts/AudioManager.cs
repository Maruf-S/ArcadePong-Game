using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    //Use An audio manager from assetstore
    //Audio player on sound
    public Sound[] sounds;
    //AudioManager Singleton
    public static AudioManager instance;
    void Awake()
    {
        if(instance == null)
        instance=this;
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound i in sounds){
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;
            i.source.volume = i.volume;
            i.source.pitch = i.pitch;
            i.source.loop = i.loop;
            i.source.outputAudioMixerGroup = i.output;
        }
    }
    public void Play(String name,float pitch){
        Debug.Log("COmmand to play sound + " + name);
        Sound sound = Array.Find(sounds,clip => clip.name ==name);
        sound.source.pitch = pitch;
        sound.source.Play();
    }
    public void Resume(String name){
        Debug.Log("COmmand to resume sound + " + name);
        Sound sound = Array.Find(sounds,clip => clip.name ==name);
        sound.source.UnPause();
    }
    public void Pause(String name){
        Debug.Log("COmmand to pause sound + " + name);
        Sound sound = Array.Find(sounds,clip => clip.name ==name);
        sound.source.Pause();
    }
    public void Stop(String name){
        Debug.Log("COmmand to stop sound + " + name);
        Sound sound = Array.Find(sounds,clip => clip.name ==name);
        sound.source.Pause();
    }
    public void ThemeSong(string command){
        Sound sound = Array.Find(sounds,clip => clip.name =="Theme");
        if(!sound.source.isPlaying && command == "Play"){
            sound.source.loop = true; 
            sound.source.PlayDelayed(1);
        }
        else if(sound.source.isPlaying && command == "Pause"){
            sound.source.Pause();
        }
        else if(!sound.source.isPlaying && command == "Resume"){
            sound.source.UnPause();
        }
    }
    public void MultiplayerThemeSong(string command){
        Sound sound = Array.Find(sounds,clip => clip.name =="MultiplayerTheme");
        if(!sound.source.isPlaying && command == "Play"){
            sound.source.loop = true; 
            sound.source.PlayDelayed(1);
        }
        else if(sound.source.isPlaying && command == "Pause"){
            sound.source.Pause();
        }
        else if(!sound.source.isPlaying && command == "Resume"){
            sound.source.UnPause();
        }
    }
}
