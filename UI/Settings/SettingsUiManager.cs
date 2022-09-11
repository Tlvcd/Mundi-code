using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUiManager : MonoBehaviour
{
    [SerializeField] private SettingsContainer container;


    [SerializeField]
    private TMP_Dropdown resDropdown, vsyncDropdown, qualityDropdown, screenDropdown;

    private Dictionary<int, Resolution> resDictionary = new Dictionary<int, Resolution>();

    [SerializeField]
    private Slider volumeMain, volumeInterface, volumeSFX, volumeMusic, frameRateSlider;
    [SerializeField]
    private TMP_Text frameRateDisplay;


    private void Awake()
    {
        DisplayResolutions();
        RefreshValues();
    }


    private void DisplayResolutions()
    {

        var resolutions =
            Screen.resolutions
                .Select(resolution => 
                        new Resolution
                        {
                            width = resolution.width, 
                            height = resolution.height
                        })
                .Distinct()
                .ToArray();

        var resNamesList = new List<string>();

        for (var i = 0; i < resolutions.Length; i++)
        {
            var current = resolutions[i];
            resNamesList.Add($"{current.width}x{current.height}");
            resDictionary.Add(i, current);
        }

        resDropdown.AddOptions(resNamesList);

        resDropdown.RefreshShownValue();
    }

    public void ChangeVsync(int index)
    {
        container.SetVSync(index);
        if (index == 1)
        {
            RefreshFrameSlider();
            frameRateSlider.interactable = false;
            return;
        }

        frameRateSlider.interactable = true;

    }

    public void ChangeResolution(int index)
    {
        container.SetResolution(resDictionary[index]);
    }

    public void ChangeRefreshRate(float value)
    {
        value *= 10;
        frameRateDisplay.text = value.ToString();
        container.SetRefreshRate((int)value);
    }

    private void RefreshFrameSlider()
    {
        var set = container.Settings;
        frameRateSlider.value = set.Refresh / 10;
        frameRateDisplay.text = set.Refresh.ToString();
    }


    public void ChangeScreenMode(int index)
    {

        switch (index)
        {
            case 0:
                container.SetFullscreen(FullScreenMode.Windowed);
                break;

            case 1:
                container.SetFullscreen(FullScreenMode.FullScreenWindow);
                break;
            default:
                container.SetFullscreen(FullScreenMode.FullScreenWindow);
                break;
        }
        
    }

    private void ScreenModeDropdown(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.FullScreenWindow:
                screenDropdown.value = 1;
                break;

            case FullScreenMode.Windowed:
                screenDropdown.value = 0;
                break;

            default:
                screenDropdown.value = 1;
                break;
        }
        screenDropdown.RefreshShownValue();
    }

    private void RefreshValues()
    {
        var set = container.Settings;

        RefreshFrameSlider();

        volumeMain.value = set.VolumeMain;
        volumeInterface.value = set.VolumeUi;
        volumeSFX.value = set.VolumeSfx;
        volumeMusic.value = set.VolumeMusic;

        resDropdown.value =
            resDictionary
                .FirstOrDefault(x => 
                    x.Value.height == set.Res_H 
                    && x.Value.width == set.Res_W).Key;
        resDropdown.RefreshShownValue();

        ScreenModeDropdown(container.Settings.Fullscreen);


        vsyncDropdown.value = set.VSync;
        vsyncDropdown.RefreshShownValue();

        qualityDropdown.value = set.Quality;
        qualityDropdown.RefreshShownValue();
    }

}
