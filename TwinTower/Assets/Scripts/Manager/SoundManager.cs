using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinTower;
using UnityEngine;

public class SoundManager : Manager<SoundManager> {
    private AudioSource bgmSource;
    private List<AudioSource> effectSources;
    private float reducevolume = -1;
    private float[] soundvolume = new float[5]
        { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
    protected override void Awake() {
        base.Awake();

        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
        effectSources = GetComponents<AudioSource>().ToList();
        if (effectSources.Count == 0) effectSources.Add(gameObject.AddComponent<AudioSource>());
        
        effectSources.Remove(bgmSource);
        bgmSource.loop = true;
    }

    public void SetBGM(AudioClip bgm, int volume = 1) {
        bgmSource.volume = soundvolume[volume];
        if (bgm == bgmSource.clip) return;
        bgmSource.clip = bgm;
        if(!bgmSource.isPlaying) bgmSource.Play();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void ChangeBGM(AudioClip bgm)
    {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    public void SetReduceVolume()
    {
        if (reducevolume == -1)
        {
            reducevolume = bgmSource.volume;
            bgmSource.volume /= 2;
        }
        else
        {
            bgmSource.volume = reducevolume;
            reducevolume = -1;
        }
    }

    public void PlayEffect(AudioClip effect) {
        foreach (AudioSource source in effectSources) {
            if (!source.isPlaying) {
                source.clip = effect;
                source.Play();
                return;
            }
        }
        
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = effect;
        newSource.Play();
        effectSources.Add(newSource);
    }
}
