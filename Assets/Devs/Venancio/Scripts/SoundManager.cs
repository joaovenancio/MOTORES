using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Background Musics Configuration")]

    [SerializeField]
    private AudioClipDictionaryPage[] m_backgroundMusicsData;
    public float BackgroundVolume = 1f;
    



    [Serializable]
    public struct AudioClipDictionaryPage
    {
        public string Name;
        public AudioClip AudioClip;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
