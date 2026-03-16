using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance; 
    public static LevelManager Instance {get{return _instance;}}

    private int CurrentCollectables = 0;
    
    private float CurrentLevelTime;
    
    private Camera MainCamera;
    private FadePostProcess FadePostProcess;

    public Vector2 levelOrigin;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            MainCamera = Camera.main;
            if (MainCamera != null) FadePostProcess = MainCamera.GetComponent<FadePostProcess>();
        }
    }

    private void OnEnable()
    {
        StageCollectable.CollectableCollected += IncrementCollectableCounter;
        ReloadSceneButton.OnReloadSceneButtonPressed += ReloadScene;
        LoadSceneButton.OnLoadSceneButtonPressed += LoadScene;
    }

    private void OnDisable()
    {
        StageCollectable.CollectableCollected -= IncrementCollectableCounter;
        ReloadSceneButton.OnReloadSceneButtonPressed -= ReloadScene;
        LoadSceneButton.OnLoadSceneButtonPressed -= LoadScene;
    }

    private void Update()
    {
        if (PauseManager.isGamePaused) return;
        CurrentLevelTime += Time.deltaTime;
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private void ReloadScene()
    {
        StartCoroutine(ReloadSceneCoroutine());
    }
    
    private IEnumerator FadeToBlack(float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            FadePostProcess.fadeMult = Mathf.Lerp(0.0f, 1.0f, t);
            yield return null;
        }
        FadePostProcess.fadeMult = 1.0f;
        
    }
    
    private IEnumerator FadeFromBlack(float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            FadePostProcess.fadeMult = Mathf.Lerp(1.0f, 0.0f, t);
            yield return null;
        }
        FadePostProcess.fadeMult = 0.0f;
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return FadeToBlack(0.5f);
        
        SceneManager.LoadScene(sceneName);
        
        yield return FadeFromBlack(0.5f);
    }
    
    private IEnumerator ReloadSceneCoroutine()
    {
        yield return FadeToBlack(0.5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        yield return FadeFromBlack(0.5f);
    }
    
    
    
    
    

    private void IncrementCollectableCounter()
    {
        CurrentCollectables++;
    }

    public int GetCurrentCollectableCount()
    {
        return CurrentCollectables;
    }

    public float GetCurrentLevelTime()
    {
        return CurrentLevelTime;
    }

    
    
    
    
}


