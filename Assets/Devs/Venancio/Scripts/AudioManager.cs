using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//TO-DO: No audioGroupList, fazer uma lista de um objeto proprio, onde se possa colocar um float e uma string).
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

    [Space][Space][Space]

    [Header("Custom Audio Groups")]
    [Space]

    [SerializeField]
    private List<string> _audioGroupList = new List<string>();
    private Dictionary<string, List<AudioSource>> _audioGroupDictionary = new Dictionary<string, List<AudioSource>>();


    [Space][Space][Space]

    [Header("General Configuration")]
    [Space]

    public AudioSource BackgroundMusicAudioSource;

    //[Serializable]
    //public struct AudioGroupVolumeDictionary
    //{
    //    public string AudioGroupName;
    //    public float Volume;
    //}

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

        if (_audioGroupList != null)
        {
            foreach (string audioGroup in _audioGroupList)
            {
                _audioGroupDictionary.Add(audioGroup, new List<AudioSource>());
            }
        } else
        {
            _audioGroupList = new List<string>();
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

    private List<AudioSource> RetrieveAudioSourceListByAudioGroup(string audioGroupName, string methodThatCalled)
    {
        List<AudioSource> audioGroupList = null;

        try
        {
            audioGroupList = _audioGroupDictionary[audioGroupName];
        }
        catch (KeyNotFoundException exception)
        {
            audioGroupList = new List<AudioSource>();
            _audioGroupDictionary.Add(audioGroupName, audioGroupList);
            _audioGroupList.Add(audioGroupName);

            Debug.Log("AudioManager on " + methodThatCalled + ": no Audio Group with name [" + audioGroupName + "]. Creating a new one...");
        }

        return audioGroupList;
    }

    public void Play(string audioClipName, GameObject targetObjectToPlayClip, string audioGroupName)
    {
        //Debug.Log("BAGAS: " + GlobalSoundEffectsVolume);
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

        List<AudioSource> audioGroupList = RetrieveAudioSourceListByAudioGroup(audioGroupName, "Play");

        if (targetAudioSource != null)
        {
            if (!_activeAudioSourceList.Contains(targetAudioSource))
            {
                _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
                targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
                _activeAudioSourceList.Add(targetAudioSource);
                audioGroupList.Add(targetAudioSource);
            } else if (!audioGroupList.Contains(targetAudioSource))
            {
                audioGroupList.Add(targetAudioSource);
            }

        }
        else
        {
            targetAudioSource = targetObjectToPlayClip.AddComponent<AudioSource>();
            _orignalVolumeDictionary.Add(targetAudioSource, targetAudioSource.volume);
            targetAudioSource.volume = targetAudioSource.volume * GlobalSoundEffectsVolume;
            _activeAudioSourceList.Add(targetAudioSource);
            audioGroupList.Add(targetAudioSource);
        }

        //Debug.Log(_orignalVolumeDictionary[targetAudioSource].ToString());

        targetAudioSource.clip = audioClipToPlay;
        targetAudioSource.Play();
    }

    public void ChangeVolume (float volume, string audioGroup)
    {
        List<AudioSource> audioGroupAudioSourceList = RetrieveAudioSourceListByAudioGroup(audioGroup, "ChangeVolume");

        if (audioGroupAudioSourceList.Count > 0)
        {
            foreach (AudioSource audioSource in audioGroupAudioSourceList)
            {
                //Debug.Log("UMA");
                audioSource.volume = _orignalVolumeDictionary[audioSource] * volume;
            }
        }
    }

    private int i = 0;

    public void DebugThis(string args)
    {
        Debug.Log(_audioGroupDictionary[args].Count);
        if (i > 0)
        {
            Debug.Log("AQ");
            ChangeVolume(0.5f, args);
            AudioSource audioSource = _audioGroupDictionary[args][0];
            Debug.Log(audioSource.volume);
            Debug.Log(GlobalSoundEffectsVolume);

        } else
        {
            i++;
        }
    }
}
