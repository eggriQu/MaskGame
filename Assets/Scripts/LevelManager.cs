using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance; 
    public static LevelManager Instance {get{return _instance;}}

    private bool HasSplashScreenPlayed;

    private int CurrentCollectables = 0;

    private bool currentLevelCompleted;
    
    private float CurrentLevelTime;
    
    private Camera MainCamera;
    public FadePostProcess FadePostProcess;

   // public Vector2 levelOrigin;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            FindPostProcess();
            ResetLevelManagerState();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    


    private void OnEnable()
    {
        StageCollectable.CollectableCollected += IncrementCollectableCounter;
        ReloadSceneButton.OnReloadSceneButtonPressed += ReloadScene;
        LoadSceneButton.OnLoadSceneButtonPressed += LoadScene;

        SceneManager.sceneLoaded += OnNewSceneLoaded;
    }

    private void OnDisable()
    {
        StageCollectable.CollectableCollected -= IncrementCollectableCounter;
        ReloadSceneButton.OnReloadSceneButtonPressed -= ReloadScene;
        LoadSceneButton.OnLoadSceneButtonPressed -= LoadScene;
        
        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }

    private void Update()
    {
        if (PauseManager.isGamePaused || PauseManager.isLevelPaused) return;
        CurrentLevelTime += Time.deltaTime;
        
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void ReloadScene()
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
        PauseManager.ResumeGame();
    
        yield return FadeToBlack(0.5f);
        
        ResetLevelManagerState();
        
        
        
        SceneManager.LoadScene(sceneName);
        UIManager.SetCursorState(true, CursorLockMode.None);

       
        
        
    }
    
    private IEnumerator ReloadSceneCoroutine()
    {
        PauseManager.ResumeGame();
     
        yield return FadeToBlack(0.5f);
        
        ResetLevelManagerState();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
       
        UIManager.SetCursorState(true, CursorLockMode.None);
        
    
    }

    private void ResetLevelManagerState()
    {
        CurrentCollectables = 0;
        CurrentLevelTime = 0.0f;
        SetCurrentLevelCompleted(false);
        PauseManager.PauseLevel();
    }


    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" && !HasSplashScreenPlayed) return;
        
        FindPostProcess();
                    
        StartCoroutine(FadeFromBlack(0.5f));
        
        
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

    public bool GetCurrentLevelCompleted()
    {
        return currentLevelCompleted;
    }

    public void SetCurrentLevelCompleted(bool completed)
    {
        currentLevelCompleted = completed;
    }

    public bool GetHasSplashScreenPlayed()
    {
        return HasSplashScreenPlayed;
    }

    public void SetHasSplashScreenPlayed(bool played)
    {
        HasSplashScreenPlayed = played;
    }

    public void FindPostProcess()
    {
              FadePostProcess = Camera.main.GetComponent<FadePostProcess>();
    }

    
    
    
    
}


