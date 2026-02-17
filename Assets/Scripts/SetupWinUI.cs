using System;
using TMPro;
using UnityEngine;

public class SetupWinUI : MonoBehaviour
{
   private float endTime;
   private int collectableCount;

   [SerializeField] private TextMeshProUGUI clearTime;
   [SerializeField] private Transform[] jewels;


   private void OnEnable()
   {
      collectableCount = LevelManager.Instance.GetCurrentCollectableCount();
      endTime = LevelManager.Instance.GetCurrentLevelTime();
      
      var time = TimeSpan.FromSeconds(endTime);
      clearTime.text = ("Clear time: " + $"{time.TotalMinutes:00}:{time.Seconds:00}");

      for (int i = 0; i < collectableCount; i++)
      {
         jewels[i].gameObject.SetActive(true);
      }
   }
}
