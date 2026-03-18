using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance; 
    public static LevelManager Instance {get{return _instance;}}

    private int CurrentCollectables = 0;
    
    private float CurrentLevelTime;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        StageCollectable.CollectableCollected += IncrementCollectableCounter;
    }

    private void OnDisable()
    {
        StageCollectable.CollectableCollected -= IncrementCollectableCounter;
    }

    private void Update()
    {
        CurrentLevelTime += Time.deltaTime;
    }

    private void IncrementCollectableCounter()
    {
        CurrentCollectables++;
        //Update collectable UI?
    }

    public int GetCurrentCollectableCount()
    {
        return CurrentCollectables;
    }

    public float GetCurrentLevelTime()
    {
        return CurrentLevelTime;
    }

    public void SaveLevelData(ref LevelSaveData saveData)
    {
       

    }

    public float GetBestTime(LevelSaveData saveData)
    {
        return saveData.bestTime;
    }
    
    
    
    
}


