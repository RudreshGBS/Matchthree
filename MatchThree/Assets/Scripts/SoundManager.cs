using rudscreation.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    TypeSelect,
    TypeMove,
    TypePop,
    TypeGameOver,
    MainMenu,
    GamePlay
};

public class SoundManager : Singleton<SoundManager>
{
    public List<AudioClip> clips;
    public AudioClip GamePlayClip;
    public AudioClip MainMenuClip;
    //private SoundType type;
    AudioSource Source;

    protected override void Awake()
    {
        base.Awake();
        gameObject.AddComponent<AudioSource>();
        Source = GetComponent<AudioSource>();
    }

    public void PlaySoundOneShot(SoundType clipType)
    {
        var clipID = (int)clipType;
        Source.PlayOneShot(clips[clipID]);
        
    }
    public void PlayMusic(SoundType clipType) 
    {
        Source.Stop();
        if (clipType == SoundType.MainMenu)
        {
            Source.clip = MainMenuClip;
        }
        else if (clipType == SoundType.GamePlay) 
        { 
            Source.clip = GamePlayClip;
        }
        Source.loop = true;
        Source.Play();
    }
}