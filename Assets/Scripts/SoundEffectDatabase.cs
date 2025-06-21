using UnityEngine;
using System.Collections.Generic;
public class SoundEffectDatabase : MonoBehaviour
{
    public AudioSource sfxSource;
    public static SoundEffectDatabase Instance { get; private set; }

    [System.Serializable]
    public class SoundEffect
    {
        public int id;
        public AudioClip audio;
    }

    public List<SoundEffect> entries;

    private Dictionary<int, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            BuildDictionary();
        }
    }

    void BuildDictionary()
    {
        sfxDict = new Dictionary<int, AudioClip>();
        foreach (var entry in entries)
        {
            sfxDict[entry.id] = entry.audio;
        }
    }

    public AudioClip GetAudio(int id)
    {
        if (sfxDict.TryGetValue(id, out AudioClip audio))
        {
            return audio;
        }
        Debug.Log($"The audio id: {id} doesn't exist!");
        return null;
    }

    public void PlayAudio(int id)
    {
        sfxSource.clip = GetAudio(id);
        sfxSource.Play();
    }
}
