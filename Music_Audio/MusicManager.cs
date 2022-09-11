using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource firstSound, secondSound;
    [SerializeField] private List<RegionMusic> soundtrackList;
    private RegionMusic currentRegion;

    public string currRegionName { get { if (currentRegion == null) { return string.Empty; } return currentRegion.RegionName; } }

    public static MusicManager instance;
    [SerializeField] private AudioMixerGroup DEFAULT_MUSIC_MIXER=null; //Nie moze byc ustawione jako const.
    [SerializeField] float customTime;
    WaitUntil _songEnd;
    private int currentSongIndex = 0;
    public bool LoopAfterEnd;
    private delegate void CurrentIteration();
    CurrentIteration currentIterationMethod;
    void Awake(){

        if (instance != null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance= this;
        DontDestroyOnLoad(gameObject);

        firstSound = gameObject.AddComponent<AudioSource>();
        secondSound = gameObject.AddComponent<AudioSource>();

        firstSound.playOnAwake = secondSound.playOnAwake = false;
        firstSound.outputAudioMixerGroup = secondSound.outputAudioMixerGroup = DEFAULT_MUSIC_MIXER;
    }
    public void SwitchRegion(string region){

        if (region == currRegionName) return;

        foreach(RegionMusic obj in soundtrackList){
            if(obj.RegionName == region){
                currentRegion = obj;
                SendClipToAvaibleSource(currentRegion.RegionSoundtracks[0]);
                return;
            }
        }
        SendClipToAvaibleSource(currentRegion.RegionSoundtracks[0]);
        currentRegion = soundtrackList[0];
    }
    private AudioClip RandomClipPicker() => currentRegion?.RegionSoundtracks[Random.Range(0, soundtrackList.Count)];

    
    private AudioClip GetNextSongInList(){
        currentSongIndex +=1;
        if(currentSongIndex>soundtrackList.Count-1){
            currentSongIndex = 0;
        }
        return currentRegion?.RegionSoundtracks[currentSongIndex];
    }
    private void FadeBetweenTracks(AudioSource trackToDecreaseVolume, AudioSource trackToIncreaseVolume){
        StartCoroutine(Co_DecreaseSourceVolume(trackToDecreaseVolume));
        StartCoroutine(Co_IncreaseSourceVolume(trackToIncreaseVolume));
    }

    public void StopPlaying()
    {
        StartCoroutine(Co_DecreaseSourceVolume(firstSound));
        StartCoroutine(Co_DecreaseSourceVolume(secondSound));
    }
    public void SendClipToAvaibleSource(AudioClip track){
        if(firstSound.isPlaying){
            secondSound.clip = track;
            StartCoroutine(Co_SongTimeMeasurer(secondSound));
            FadeBetweenTracks(firstSound, secondSound);
        }
        else{
            firstSound.clip = track;
            StartCoroutine(Co_SongTimeMeasurer(firstSound));
            FadeBetweenTracks(secondSound, firstSound);
        }
    }
    public void SwitchToRandomSoundtrack(){
        SendClipToAvaibleSource(RandomClipPicker());
        currentIterationMethod = SwitchToRandomSoundtrack;
    }
    public void PlayNextSoundtrack(){
        SendClipToAvaibleSource(GetNextSongInList());
        currentIterationMethod = PlayNextSoundtrack;
    }
    private IEnumerator Co_IncreaseSourceVolume(AudioSource source){

        float time = 0f;

        source.volume = 0f;
        source.Play();
        while(time<2f){

            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, 1f, time/2);

            yield return null; //weird coroutine syntax
        }
    }
    private IEnumerator Co_DecreaseSourceVolume(AudioSource source){
        
        float time = 0f;

        while(time<2f){

            time += Time.deltaTime;
            source.volume = Mathf.Lerp(1f, 0f, time/2);

            yield return null; //weird coroutine syntax
        }
        source.clip = null;
        source.Stop();
    }

    private IEnumerator Co_SongTimeMeasurer(AudioSource source){

        if (source == null)
        {
            if (LoopAfterEnd)
            {
                currentIterationMethod?.Invoke();
            }
            yield break;
        }
        _songEnd = new WaitUntil(() => source.time >= source.clip.length);

        yield return _songEnd;
        if(LoopAfterEnd){
            currentIterationMethod?.Invoke();
        }
    }
}

[System.Serializable]
public class RegionMusic{
    public string RegionName;
    public List<AudioClip> RegionSoundtracks;


}