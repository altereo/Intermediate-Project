using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;

public class BeatmapAudioLoader : MonoBehaviour
{
    public AudioSource m_MusicSource;
    public PatternManager m_PatternManager;
    public NAudioImporter m_NAudioImporter;

    public AudioClip m_AudioFile;

    public bool m_CanPlayAudio = true;

    private void Awake()
    {
        // Setup the importer's callback.
        m_NAudioImporter.Loaded += OnAudioLoaded;
    }

    private void Update()
    {
       if (m_CanPlayAudio && m_PatternManager.m_PatternLoaded && m_PatternManager.m_AudioFileLoaded) {
            m_CanPlayAudio = false;
            PlayAudio();
       } 
    }

    public void PlayAudio()
    {
        // Play the audio if it exists.
        if (m_AudioFile) {
            m_MusicSource.clip = m_AudioFile;
            m_MusicSource.Play();
        }
    }

    // Begin loading of a provided sound file. Let's just pretend
    // race conditions don't exist right now... it's easier that way.
    public void PrepareAudio(string fileName, string dataPath)
    {
        var filePath = dataPath + "/" + fileName;
        // Remove any invalid characters from the path... even though
        // it really *should* be able to work without this, it kind of
        // just *doesn't*. WHY? I don't know. I checked the thing earlier for
        // invalid characters... the beatmap part of the code uses this same path
        // and works fine. WHY.
        foreach (char c in Path.GetInvalidPathChars()) {
            filePath = filePath.Replace(c.ToString(), "");
        }
        Debug.Log("Checking for \"" + filePath + "\"");
        if (File.Exists(filePath)) {
            Debug.Log("Good news... it exists!");
            // Make sure it exists first.
            m_NAudioImporter.Import(filePath);
        } else {
            Debug.Log("Bad news... I couldn't find it.");
        }
    }

    private void OnAudioLoaded(AudioClip clip)
    {
        m_AudioFile = clip;
        m_PatternManager.m_AudioFileLoaded = true;
    }

    public void StopClip()
    {
        m_MusicSource.Stop();
    }
}
