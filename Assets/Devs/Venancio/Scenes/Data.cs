using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Data
{
    [Serialize]
    public List<GameCondition> Conditions;
}       
