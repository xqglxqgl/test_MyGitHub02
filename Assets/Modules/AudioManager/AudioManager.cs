using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private List<AudioSource> audioList;

    private void Awake()
    {
        this.audioList = new();
    }

    public AudioSource CreateAudioSource(string ASPrefabPath,string ACPath)
    {
        var ASPrefab = Pool.Instance.Spawn(ASPrefabPath);
        var audioSource = ASPrefab.GetComponent<AudioSource>();
        audioSource.clip = AssetManager.Instance.LoadAsset<AudioClip>(ACPath);
        audioList.Add(audioSource);
        return audioSource;
    }

}
