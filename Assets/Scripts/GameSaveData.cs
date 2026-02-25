using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public List<LevelSaveData> Levels = new List<LevelSaveData>();
}
