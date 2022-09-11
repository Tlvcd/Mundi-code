using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Axis Mundi/Internal/Settings asset")]
public class SettingsContainer : ScriptableObject
{
    [SerializeField] private AudioMixer mix;

    public SettingValues Settings { get; private set; }
    public void ImportValues(SettingValues values)
    {
        
        Settings = values;

    }

    public event Action OnSettingsChange;

    private const string VOL_MAIN_VAR = "Volume";
    private const string VOL_MUSIC_VAR = "Music";
    private const string VOL_SFX_VAR = "SFX";
    private const string VOL_UI_VAR = "Interface";

    public void RefreshSettings()
    {
        if (Settings==null) return;

        SetResolution(Settings.Res_W, Settings.Res_H, Settings.Fullscreen);
        SetQuality(Settings.Quality);

        SetRefreshRate(Settings.Refresh);
        SetVSync(Settings.VSync);

        //volumes
        SetMainVolume(Settings.VolumeMain);
        SetMusicVolume(Settings.VolumeMusic);
        SetSfxVolume(Settings.VolumeSfx);
        SetUiVolume(Settings.VolumeUi);

    }


    private void SetVolume(string varName,float volume)
    {
        mix.SetFloat(varName, Mathf.Log10(volume) * 20);
    }

    public void SetMainVolume(float vol)
    {
        SetVolume(VOL_MAIN_VAR, vol);
        Settings.VolumeMain = vol;

        OnSettingsChange?.Invoke();
    }

    public void SetMusicVolume(float vol)
    {
        SetVolume(VOL_MUSIC_VAR, vol);
        Settings.VolumeMusic = vol;

        OnSettingsChange?.Invoke();
    }

    public void SetSfxVolume(float vol)
    {
        SetVolume(VOL_SFX_VAR, vol);
        Settings.VolumeSfx = vol;

        OnSettingsChange?.Invoke();
    }

    public void SetUiVolume(float vol)
    {
        SetVolume(VOL_UI_VAR, vol);
        Settings.VolumeUi = vol;

        OnSettingsChange?.Invoke();
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, Screen.currentResolution.refreshRate);
        Settings.Res_H = resolution.height;
        Settings.Res_W = resolution.width;

        OnSettingsChange?.Invoke();
    }

    public void SetResolution(int width, int height, FullScreenMode screen)
    {
        Screen.SetResolution(width,height,screen, 0);

        Settings.Res_W = width;
        Settings.Res_H = height;
        Settings.Fullscreen = screen;

        OnSettingsChange?.Invoke();
    }

    public void SetFullscreen(FullScreenMode screen)
    {
        Screen.fullScreenMode = screen;

        Settings.Fullscreen = screen;
    }

    public void SetRefreshRate(int rate)
    {
        Application.targetFrameRate = rate;
        Settings.Refresh = rate;

        OnSettingsChange?.Invoke();
    }

    public void SetVSync(int mode)
    {
        QualitySettings.vSyncCount = mode;


        //if(mode==1) SetRefreshRate(Screen.currentResolution.refreshRate);

        Settings.VSync = mode;

        OnSettingsChange?.Invoke();
    }
}

[System.Serializable]
public class SettingValues
{
    //quality
    public int Quality, VSync;

    //screen
    public int Res_W, Res_H, Refresh;
    public FullScreenMode Fullscreen;

    //volumes
    public float VolumeMain, VolumeMusic, VolumeSfx, VolumeUi;

    public SettingValues()
    {
        Quality = QualitySettings.GetQualityLevel();
        VSync = QualitySettings.vSyncCount;
        Refresh = Application.targetFrameRate;
        Res_W = Screen.currentResolution.width;
        Res_H = Screen.currentResolution.height;
        Fullscreen = Screen.fullScreenMode;

        VolumeMain = VolumeMusic = VolumeSfx = VolumeUi = 1;
    }


}
