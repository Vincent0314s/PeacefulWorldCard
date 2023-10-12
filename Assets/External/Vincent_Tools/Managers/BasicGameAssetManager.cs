using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class V_Effect
{
    public VisualEffect effect;
    public GameObject effectObject;
}

[System.Serializable]
public class S_Effect
{
    public SoundEffect effect;
    public AudioClip soundClip;
}

[System.Serializable]
public class S_UI_Effect
{
    public UI_SoundEffects effect;
    public AudioClip soundClip;
}

public class BasicGameAssetManager : Singleton<BasicGameAssetManager>
{
    [Header("List")]
    [ArrayElementTitle("effect")]
    public List<V_Effect> visualEffects;
    [ArrayElementTitle("effect")]
    public List<S_Effect> soundEffects;
    [ArrayElementTitle("effect")]
    public List<S_UI_Effect> UI_SoundClipArray;

    public SettingManager settingManager;
}
