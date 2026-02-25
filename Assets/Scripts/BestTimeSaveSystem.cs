using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;



public class BestTimeSaveSystem : MonoBehaviour
{

    private static BestTimeSaveSystem _instance; 
    public static BestTimeSaveSystem Instance {get{return _instance;}}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    private GameSaveData _gameSaveData;
    private string _saveDataJSON;
    private List<LevelSaveData> _levelSaveDataList;
    private Dictionary<string,LevelSaveData> _bestTimesDictionary = new Dictionary<string, LevelSaveData>();



    ///Copies save data into variables
    private void LoadSaveData()
    {
        //Get save data
        _saveDataJSON = File.ReadAllText(GetSaveFileName());

        //Convert save data to list of levels
        _gameSaveData = JsonUtility.FromJson<GameSaveData>(_saveDataJSON);
        _levelSaveDataList = _gameSaveData.Levels;
        if (_levelSaveDataList == null)
        {
            throw new Exception();
        }
        

        _bestTimesDictionary = _levelSaveDataList.ToDictionary(t => t.levelName, t => t);
    }



    public void TrySaveBestTime(string desiredLevelID, float completionTime)
    {
        //Check if file exists -> MOVE TO GAME STARTUP!!!!!
        if (!File.Exists(GetSaveFileName()))
        {
            GameSaveData emptySaveData = new GameSaveData();
            File.WriteAllText(GetSaveFileName(), JsonUtility.ToJson(emptySaveData));
        }
        LoadSaveData();
       

       
        //Find desired level data index
        if (_bestTimesDictionary.TryGetValue(desiredLevelID, out LevelSaveData levelSaveData))
        {
            //If found and time is improved, update and save
            if (completionTime < levelSaveData.bestTime)
            {
                _bestTimesDictionary[desiredLevelID].bestTime = completionTime;
                SaveLevelData();
            }
        }
        //If not found, add entry to dictionary and save
        else
        {
            _bestTimesDictionary.Add(desiredLevelID, CreateLevelEntry(desiredLevelID, completionTime));
            SaveLevelData();
        }
        
       
       
        
       
      
        
      
    }

    private void SaveLevelData()
    {
        _levelSaveDataList = _bestTimesDictionary.Values.ToList();
        _gameSaveData.Levels = _levelSaveDataList;
        _saveDataJSON = JsonUtility.ToJson(_gameSaveData);
        File.WriteAllText(GetSaveFileName(), _saveDataJSON);
        _saveDataJSON = File.ReadAllText(GetSaveFileName());
    }

    private float GetBestTime(string levelname)
    {
        return _bestTimesDictionary[levelname].bestTime;
    }

 

    private static LevelSaveData CreateLevelEntry(string name, float time)
    {
        return new LevelSaveData()
        {
            levelName = name,
            bestTime = time
        };
    }

  

    public static string GetSaveFileName()
    {
        return Application.persistentDataPath + "/GameSaveData" + ".save";
    }

   
    
    
    
}
