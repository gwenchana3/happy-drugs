using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider VolumeSlider;
    public Toggle FullscreenToggle;

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = FullscreenToggle.isOn;
    }
}
