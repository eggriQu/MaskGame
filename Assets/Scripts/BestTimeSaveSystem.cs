using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;



public class BestTimeSaveSystem : MonoBehaviour
{
    private void OnEnable()
    {
       // LevelExit.OnLevelExit += TrySaveBestTime;
    }

    private void OnDisable()
    {
      //  LevelExit.OnLevelExit -= TrySaveBestTime;
    }


  

    public static void TrySaveBestTime(string desiredLevelID, float completionTime)
    {
        //Check if file exists -> MOVE TO GAME STARTUP!!!!!
        if (!File.Exists(GetSaveFileName()))
        {
            GameSaveData emptySaveData = new GameSaveData();
            File.WriteAllText(GetSaveFileName(), JsonUtility.ToJson(emptySaveData));
        }
        //Get save data
        string savedataJson = File.ReadAllText(GetSaveFileName());
        //Convert save data to list of levels
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(savedataJson);
        List<LevelSaveData> levels = saveData.Levels;

        if (levels == null)
        {
            return;
        }
        //Find desired level data index
        
        bool levelFound = false;

        
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelName == desiredLevelID)
            {
                levelFound = true;
                float storedBestTime = levels[i].bestTime;
                if (completionTime < storedBestTime)
                {
                    levels[i].bestTime = completionTime;
                }
                break;
            }
        }
        
        //If level not already in save data, create entry
        if (!levelFound)
        {
            LevelSaveData newLevel = new LevelSaveData
            {
                levelName = desiredLevelID,
                bestTime = completionTime
            };
            levels.Add(newLevel);
        }
        
        //write back to file
        saveData.Levels = levels;
        savedataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFileName(), savedataJson);
        
    }

    public static string GetSaveFileName()
    {
        return Application.persistentDataPath + "/GameSaveData" + ".save";
    }

   
    
    
    
}
