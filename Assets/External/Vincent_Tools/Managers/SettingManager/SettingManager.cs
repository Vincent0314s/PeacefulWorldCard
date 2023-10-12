using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Linq;

public enum TextType
{
    Normal = 0,
    TMP = 1
}

public class SettingManager : MonoBehaviour
{
    public Resolution[] allResolutions { get; private set; }

    [HideInInspector] public TextType textType;

    [HideInInspector] public Dropdown normal_resolutionDisplay;
    [HideInInspector] public TMP_Dropdown tmp_resolutionDisplay;

    [HideInInspector] public Slider masterSlider;
    [HideInInspector] public Slider musicSlider;
    [HideInInspector] public Slider soundEffectSlider;

    [HideInInspector] public Toggle fullScreenToggle;
    private int currentResolutionIndex;

    [HideInInspector] public AudioMixer masterMixer;

    [HideInInspector] public AudioMixerGroup musicOutPutGroup;
    [HideInInspector] public AudioMixerGroup soundEffectOutPutGroup;

    //public SettingData m_settingData;

    private void Start()
    {
        InitResolution();
        InitToggle();
        InitSound();
    }

    private void InitResolution()
    {
        allResolutions = Screen.resolutions;
        allResolutions = allResolutions.OrderBy(x => x.width).ToArray();

        List<string> optionsText = new List<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            string resolutions = allResolutions[i].width + "x" + allResolutions[i].height + " Hz " + allResolutions[i].refreshRate;
            optionsText.Add(resolutions);

            if (allResolutions[i].width == Screen.currentResolution.width && allResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        ResetResolution();

        switch (textType)
        {
            case TextType.Normal:
                if (normal_resolutionDisplay != null)
                {
                    normal_resolutionDisplay.ClearOptions();
                    normal_resolutionDisplay.AddOptions(optionsText);
                    normal_resolutionDisplay.value = currentResolutionIndex;
                    normal_resolutionDisplay.RefreshShownValue();

                    normal_resolutionDisplay.onValueChanged.AddListener(delegate
                    {
                        SetResolution(normal_resolutionDisplay);
                    });
                }
                break;
            case TextType.TMP:
                if (tmp_resolutionDisplay != null)
                {
                    tmp_resolutionDisplay.ClearOptions();
                    tmp_resolutionDisplay.AddOptions(optionsText);
                    tmp_resolutionDisplay.value = currentResolutionIndex;
                    tmp_resolutionDisplay.RefreshShownValue();

                    tmp_resolutionDisplay.onValueChanged.AddListener(delegate
                    {
                        SetResolution(tmp_resolutionDisplay);
                    });
                }
                break;
        }
    }

    private void InitSound()
    {
        SettingSavedData savedData = LoadSettingData();

        if (masterSlider != null)
        {
            masterSlider.minValue = -60f;
            masterSlider.maxValue = 20;
            masterSlider.value = savedData.MasterVolume;

            masterSlider.onValueChanged.AddListener(delegate
            {
                SetMasterVolume(masterSlider);
            });
        }

        if (musicSlider != null)
        {
            musicSlider.minValue = -60f;
            musicSlider.maxValue = 20;
            musicSlider.value = savedData.MusicVolume;

            musicSlider.onValueChanged.AddListener(delegate
            {
                SetMusicVolume(musicSlider);
            });

        }
        if (soundEffectSlider != null)
        {
            soundEffectSlider.minValue = -60f;
            soundEffectSlider.maxValue = 20;
            soundEffectSlider.value = savedData.SoundEffectVolume;

            soundEffectSlider.onValueChanged.AddListener(delegate
            {
                SetSoundEffectVolume(soundEffectSlider);
            });
        }

        if (masterMixer != null)
        {
            masterMixer.SetFloat("MasterVolume", savedData.MasterVolume);
            masterMixer.SetFloat("MusicVolume", savedData.MusicVolume);
            masterMixer.SetFloat("SFXVolume", savedData.SoundEffectVolume);
        }
    }

    private void InitToggle()
    {
        if (fullScreenToggle != null)
        {
            fullScreenToggle.onValueChanged.AddListener(delegate
            {
                SetFullScreen(fullScreenToggle);
            });
        }
    }

    public void SetFullScreen(Toggle isFullScreen)
    {
        Screen.fullScreen = isFullScreen.isOn;
    }

    public void SetResolution(Dropdown rsIndex)
    {
        Resolution rs = allResolutions[rsIndex.value];
        Screen.SetResolution(rs.width, rs.height, fullScreenToggle.isOn);
    }

    public void SetResolution(TMP_Dropdown rsIndex)
    {
        Resolution rs = allResolutions[rsIndex.value];
        Screen.SetResolution(rs.width, rs.height, fullScreenToggle.isOn);
    }

    private void ResetResolution() {
        fullScreenToggle.isOn = true;
        Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,true);
    }

    public void SetMasterVolume(Slider slider)
    {
        if (masterMixer != null)
            masterMixer.SetFloat("MasterVolume", slider.value);
    }


    public void SetMusicVolume(Slider slider)
    {
        if (masterMixer != null)
            masterMixer.SetFloat("MusicVolume", slider.value);
    }

    public void SetSoundEffectVolume(Slider slider)
    {
        if (masterMixer != null)
            masterMixer.SetFloat("SFXVolume", slider.value);
    }

    public void SaveSettingData()
    {
        SettingSavedData saveObject = new SettingSavedData
        {
            MasterVolume = masterSlider.value,
            MusicVolume = musicSlider.value,
            SoundEffectVolume = soundEffectSlider.value
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveLoadSystem.Save(StringDefs.SaveData.settingSave, json);
    }

    private SettingSavedData LoadSettingData()
    {
        string saveString = SaveLoadSystem.Load(StringDefs.SaveData.settingSave);
        if (saveString != null)
        {
            SettingSavedData saveObject = JsonUtility.FromJson<SettingSavedData>(saveString);
            return saveObject;
        }
        else
        {
            SettingSavedData saveObject = new SettingSavedData
            {
                MasterVolume = 0,
                MusicVolume = 0,
                SoundEffectVolume = 0
            };
            Debug.LogError("NoSave");
            return saveObject;
        }
    }
}
