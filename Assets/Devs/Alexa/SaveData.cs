using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]

public class SaveData
{

    [Serialize]
    
    //lista de vários objetos que tem nome e valor
    public List<GameCondition> ListOfConditions;

    //saber qual o capitulo que o user se encontra
    [Serialize]
    public string Chapter;


}
