using UnityEngine;
using System.Collections.Generic;
public class SoundEffectDatabase : MonoBehaviour, ISoundEffectDatabase
{
    public AudioSource sfxSource;
    public static SoundEffectDatabase Instance { get; private set; }

    // Simple class to store sound effect information
    [System.Serializable]
    public class SoundEffect
    {
        public int id;
        public string name = "Sound"; // storing name for easier understanding later
        public AudioClip audio;
    }

    // Unity changable instance
    public List<SoundEffect> entries;

    // Instance variable
    private Dictionary<int, AudioClip> _sfxDict;

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
        this._sfxDict = new Dictionary<int, AudioClip>();
        foreach (var entry in entries)
        {
            this._sfxDict[entry.id] = entry.audio;
        }
    }

    private AudioClip GetAudio(int id)
    {
        if (this._sfxDict.TryGetValue(id, out AudioClip audio))
        {
            return audio;
        }
        Debug.Log($"The audio id: {id} doesn't exist!");
        return null;
    }

    public void PlayAudio(int id, float volume = 1.0f)
    {
        AudioClip clip = GetAudio(id);
        sfxSource.PlayOneShot(clip, volume);
    }
}
