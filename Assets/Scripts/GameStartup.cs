using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class GameStartup : MonoBehaviour
{

 private Camera mainCamera;
 [SerializeField] private Transform cameraStart;
 [SerializeField] private Transform cameraEnd;
 [SerializeField] private FadePostProcess fadePostProcess;

 private void Awake()
 {
  SetupSaveFile();
  LoadPlayerPrefs();
  mainCamera = Camera.main;
  



 }

 private void Start()
 {
   if (!LevelManager.Instance.GetHasSplashScreenPlayed())
   {
    LevelManager.Instance.SetHasSplashScreenPlayed(true);

   
   
    PlayLogoSequence();
   }
  
 }


 private void SetupSaveFile()
 {
  if (!File.Exists(BestTimeSaveSystem.GetSaveFileName()))
  {
   GameSaveData emptySaveData = new GameSaveData();
   File.WriteAllText(BestTimeSaveSystem.GetSaveFileName(), JsonUtility.ToJson(emptySaveData));
  }
 }

 private void LoadPlayerPrefs()
 {
  bool fullscreen;
  if (PlayerPrefs.HasKey("Fullscreen"))
  {
   fullscreen = !PlayerPrefs.GetInt("Fullscreen").Equals(0);
  }
  else
  {
   fullscreen = true;
  }
 
  if (PlayerPrefs.HasKey("ResolutionX"))
  {
   int resolutionX = PlayerPrefs.GetInt("ResolutionX");
   int resolutionY = PlayerPrefs.GetInt("ResolutionY");
   Screen.SetResolution(resolutionX, resolutionY, fullscreen);
  }
 }

 private void PlayLogoSequence()
 {
  mainCamera.transform.position = cameraStart.position;
  fadePostProcess.fadeMult = 1.0f;
  StartCoroutine(InitialWait());

 }

 private IEnumerator InitialWait()
 {
  yield return new WaitForSeconds(0.5f);
  StartCoroutine(FadeInLogo());
 }

 private IEnumerator FadeInLogo()
 {
  float elapsedTime = 0.0f;
  float duration = 2.5f;

  while (elapsedTime < duration)
  {
   elapsedTime += Time.deltaTime;
   float t = elapsedTime / duration;
   fadePostProcess.fadeMult = Mathf.Lerp(1.0f, 0.0f, t);
   yield return null;
  }
  fadePostProcess.fadeMult = 0.0f;
  StartCoroutine(PauseForLogo());
 }

 private IEnumerator PauseForLogo()
 {
  yield return new WaitForSeconds(2.5f);
  StartCoroutine(FadeOutLogo());
 }

 private IEnumerator FadeOutLogo()
 {
  float elapsedTime = 0.0f;
  float duration = 2.5f;

  while (elapsedTime < duration)
  {
   elapsedTime += Time.deltaTime;
   float t = elapsedTime / duration;
   fadePostProcess.fadeMult = Mathf.Lerp(0.0f, 1.0f, t);
   yield return null;
  }
  fadePostProcess.fadeMult = 1.0f;
  StartCoroutine(FadeInMenu());
 }
 
 private IEnumerator FadeInMenu()
 {
 
  mainCamera.transform.position = cameraEnd.position;
  float elapsedTime = 0.0f;
  float duration = 2.5f;

  while (elapsedTime < duration)
  {
   elapsedTime += Time.deltaTime;
   float t = elapsedTime / duration;
   fadePostProcess.fadeMult = Mathf.Lerp(1.0f, 0.0f, t);
   yield return null;
  }
  fadePostProcess.fadeMult = 0.0f;
 }
 
 


 private IEnumerator SmoothCameraMove(float duration,Transform targetLocation)
 {
  float elapsedTime = 0;
  Vector3 startPos = mainCamera.transform.position;

  while (elapsedTime < duration)
  {
   elapsedTime += Time.deltaTime;
   float t = elapsedTime / duration;
   mainCamera.gameObject.transform.position = new Vector3(
    Mathf.Lerp(startPos.x, targetLocation.position.x, t),
    Mathf.Lerp(startPos.y, targetLocation.transform.position.y, t),
    Mathf.Lerp(startPos.z, targetLocation.transform.position.z, t));
   yield return null;
  }
  mainCamera.gameObject.transform.position = targetLocation.transform.position;
 }


}
