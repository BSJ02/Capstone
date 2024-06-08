using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider backgroundMusicSlider;
    public Slider soundEffectsSlider;

    void Start()
    {
        backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        soundEffectsSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
    }

    void SetBackgroundMusicVolume(float volume)
    {
        SoundManager.instance.SetBackgroundMusicVolume(volume);
    }

    void SetSoundEffectsVolume(float volume)
    {
        SoundManager.instance.SetSoundEffectsVolume(volume);
    }
}
