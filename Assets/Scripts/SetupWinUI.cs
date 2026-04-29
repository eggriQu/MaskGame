using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupWinUI : MonoBehaviour
{
   private float endTime;
   private int collectableCount;

   [SerializeField] private TextMeshProUGUI clearTimeTime;
   [SerializeField] private TextMeshProUGUI bestTimeTime;
   [SerializeField] private Transform[] jewels;


   private void OnEnable()
   {
      collectableCount = LevelManager.Instance.GetCurrentCollectableCount();
      var loadedBestTime = BestTimeSaveSystem.Instance.GetBestTime(SceneManager.GetActiveScene().name);

      if (collectableCount < 3 && loadedBestTime == 1000)
      {
         bestTimeTime.text = "--:--";
      }
      else
      {
         Debug.Log(loadedBestTime);
         var bestTimespan = TimeSpan.FromSeconds(loadedBestTime);
         bestTimeTime.text = ($"{bestTimespan.TotalMinutes:00}:{bestTimespan.Seconds:00}");
      }
      
     
     
     
      endTime = LevelManager.Instance.GetCurrentLevelTime();
      
      var endTimespan = TimeSpan.FromSeconds(endTime);
      clearTimeTime.text = ($"{endTimespan.TotalMinutes:00}:{endTimespan.Seconds:00}");

      for (int i = 0; i < collectableCount; i++)
      {
         jewels[i].gameObject.SetActive(true);
      }
   }
}
