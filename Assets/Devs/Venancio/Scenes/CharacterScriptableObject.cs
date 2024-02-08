using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public string Name;
    public List<EmotionToSprite> EmotionList;

    [Serializable]
    public struct EmotionToSprite
    {
        public CharacterEmotion CharacterEmotion;
        public Sprite Sprite;
    }
}
