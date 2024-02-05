using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataManager
{
    public bool CanLoadAGame();
    public void LoadAGame();
    public void SaveAGame();
}
