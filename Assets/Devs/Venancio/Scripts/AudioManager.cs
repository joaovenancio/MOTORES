using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Musics Configuration")]
    [Space]

    [SerializeField]
    private AudioClipDictionaryPage[] m_backgroundMusicsData;
    private Dictionary<string, AudioClip> _backgroundMusics = new Dictionary<string, AudioClip>();
    [Range(0, 1)] [SerializeField]
    private float m_backgroundVolume;
    public float BackgroundVolume {
        get {
            return m_backgroundVolume;
        }

        set { 
            m_backgroundVolume = value;
            BackgroundMusicAudioSource.volume = value;
        } 
    }
    [SerializeField]
    private bool _playOnStart;
    [SerializeField]
    private string _backgroundMusicToPlay;

    [Space][Space][Space]

    [Header("Audio Clips Configuration")]
    [Space]

    [SerializeField]
    private AudioClipDictionaryPage[] m_audioClipsData;
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    [Range(0, 1)] [SerializeField]
    private float _GlobalSoundEffectsVolume;
    public float GlobalSoundEffectsVolume {
        get {
            return _GlobalSoundEffectsVolume;
        }

        set
        {
            _GlobalSoundEffectsVolume = value;
            UpdateSoundEffectsVolume();
        }
    }
    [SerializeField]
    private List<AudioSource> _activeAudioSourceList = new List<AudioSource>();
    private Dictionary<AudioSource, float> _orignalVolumeDictionary = new Dictionary<AudioSource, float>();

    [Space]
    [Space]
    [Space]

    [Header("General Configuration")]
    [Space]

    public AudioSource BackgroundMusicAudioSource;

    [Serializable]
    public struct AudioClipDictionaryPage
    {
        public string Name;
        public AudioClip AudioClip;
    }

    private void Awake()
    {
        MakeDictionaries();
    }

    private void UpdateSoundEffectsVolume()
    {
        foreach (AudioSource audioSource in _activeAudioSourceList)
        {
            audioSource.volume = _orignalVolumeDictionary[audioSource] * GlobalSoundEffectsVolume;
        }
    }

    private void MakeDictionaries()
    {
        foreach (AudioClipDictionaryPage backgroundMusic in m_backgroundMusicsData)
        {
            _backgroundMusics.Add(backgroundMusic.Name, backgroundMusic.AudioClip);
        }

        foreach (AudioClipDictionaryPage audioClip in m_audioClipsData)
        {
            _audioClips.Add(audioClip.Name, audioClip.AudioClip);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        CheckVariables();
        
        if (_orignalVolumeDictionary.Count > 0)
            UpdateSoundEffectsVolume();

        if (_playOnStart)
            PlayBackgroundMusic(_backgroundMusicToPlay);
    }

    private void CheckVariables()
    {
        if (BackgroundMusicAudioSource == null)
        {
            BackgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        
    }

    public void PlayBackgroundMusic(string backgroundMusicName)
    {
        AudioClip audioClipToPlay = null;

        try
        {
            audioClipToPlay = _backgroundMusics[backgroundMusicName];

        } catch (KeyNotFoundException exception)
        {
            Debug.Log("AudioManager on PlayBackgroundMusic: no AudioClip with name [" + _backgroundMusicToPlay + "]. Exception: " + exception.Message);
            return;
        }

        BackgroundMusicAudioSource.volume = BackgroundVolume;
        BackgroundMusicAudioSource.clip = audioClipToPlay;

        if (!BackgroundMusicAudioSource.isPlaying)
            BackgroundMusicAudioSource.Play();
    }

    public void PlayingBackgroundMusic(bool reproduce)
    {
        if (reproduce)
        {
            BackgroundMusicAudioSource.Play();
        } else
        {
            BackgroundMusicAudioSource.Stop();
        }
    }

    public void Play(string audioClipName, GameObject targetObjectToPlayClip)
    {
        Debug.Log("BAGAS: " + GlobalSoundEffectsVolume);
        AudioClip audioClipToPlay = null;

        try
        {
            audioClipToPlay = _audioClips[audioClipName];
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log("AudioManager on Play: no AudioClip with name [" + audioClipName + "]. Exception: " + exception.Message);
            return;
        }

        AudioSource targetAudioSource = targetObjectToPlayClip.GetComponent<AudioSource>();

        if (targetAudioSource != null) {
            if (!_activeAudioSourceList.Contains(targetAudioSource))
            {
                _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
                targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
                _activeAudioSourceList.Add(targetAudioSource);
            }

        } else
        {
            targetAudioSource = targetObjectToPlayClip.AddComponent<AudioSource>();
            _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
            targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
            _activeAudioSourceList.Add(targetAudioSource);
        }

        Debug.Log(GlobalSoundEffectsVolume.ToString());
        
        targetAudioSource.clip = audioClipToPlay;
        targetAudioSource.Play();
    }

}
