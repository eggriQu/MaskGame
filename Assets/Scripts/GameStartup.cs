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
 
 
 

 
 





}
