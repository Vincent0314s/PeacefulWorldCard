using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static GameObject audioObject;
    private static AudioSource audioSource;

    public static void PlaySound(SoundEffect sound)
    {
        if (audioObject == null)
        {
            audioObject = new GameObject("Sound");
            audioSource = audioObject.AddComponent<AudioSource>();
            if (BasicGameAssetManager.Instance.settingManager.soundEffectOutPutGroup != null)
            {
                audioSource.outputAudioMixerGroup = BasicGameAssetManager.Instance.settingManager.soundEffectOutPutGroup;
            }
        }
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void PlaySound(UI_SoundEffects sound)
    {
        if (audioObject == null)
        {
            audioObject = new GameObject("Sound");
            audioSource = audioObject.AddComponent<AudioSource>();
            if (BasicGameAssetManager.Instance.settingManager.soundEffectOutPutGroup != null)
            {
                audioSource.outputAudioMixerGroup = BasicGameAssetManager.Instance.settingManager.soundEffectOutPutGroup;
            }
        }
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(SoundEffect sound)
    {
        foreach (S_Effect soundClip in BasicGameAssetManager.Instance.soundEffects)
        {
            if (soundClip.effect == sound)
            {
                return soundClip.soundClip;
            }
        }
        Debug.LogError("AudioClip Not Found");
        return null;
    }

    private static AudioClip GetAudioClip(UI_SoundEffects sound)
    {
        foreach (S_UI_Effect soundClip in BasicGameAssetManager.Instance.UI_SoundClipArray)
        {
            if (soundClip.effect == sound)
            {
                return soundClip.soundClip;
            }
        }
        Debug.LogError("AudioClip Not Found");
        return null;
    }
}
