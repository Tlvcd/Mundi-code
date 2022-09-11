using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class RandomClipPlayback : MonoBehaviour
{
    [SerializeField] private ClipsRegion[] regions;

    [SerializeField] private AudioMixerGroup defaultMixer;

    private List<AudioSource> sources;

[SerializeField]
    private bool canRepeatClips;

    private int prevID = -1;

    private void Awake()
    {
        sources = GetComponents<AudioSource>().ToList();
        if (sources.Count == 0)
        {
            CreateNewSource();
        }
    }

    private AudioSource CreateNewSource()
    {
        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = defaultMixer;

        sources.Add(newSource);

        return newSource;
    }

    public void PlayRandomClip(int regionIndex =0)
    {
        var currRegion = regions[regionIndex];
        var currLength = currRegion.Clips.Length;

        if (currLength ==0 ) return;

        PlayClip(GetRandomClip(), currRegion.regionVolume);

        AudioClip GetRandomClip()
        {

            int index= Random.Range(0, currLength - 1);

            if (!canRepeatClips)
            {
                var crashProtecc = 0;
                while (index == prevID && crashProtecc <10)
                {
                    index = Random.Range(0, currLength - 1);
                    crashProtecc++;

                }
            }

            prevID = index;
            return currRegion.Clips[index];
        }
    }

    public void PlayClip(AudioClip clip, float vol = 1)
    {
        foreach (var source in sources)
        {
            if (source.isPlaying) continue;

            source.PlayOneShot(clip, vol);
            return;
        }

        var newSource = CreateNewSource();

        newSource.PlayOneShot(clip, vol);
    }

    [System.Serializable]
    private class ClipsRegion
    {
        [field: SerializeField, Range(0,1)] public float regionVolume = 1;

        [field: SerializeField]
        public AudioClip[] Clips { get; private set; }
    }
}
