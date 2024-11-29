using System.Collections;
using System.Collections.Generic;
using MainMenu.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }

    public void Set1920x1080()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Set1280x720()
    {
        Screen.SetResolution(1280, 720, false);
    }

    // 854x480
    public void Set854x480()
    {
        Screen.SetResolution(854, 480, false);
    }
}
