using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
   public static GameDataManager Instance;

   public List<UnitDataSave> playerUnitsData = new List<UnitDataSave>();

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void SavePlayerUnits()
   {
      playerUnitsData.Clear();

      Unit[] allUnits = FindObjectsOfType<Unit>();
      foreach (Unit unit in allUnits)
      {
         if (unit.CompareTag("Player"))
         {
            if (unit == null || unit.UnitHealth == null)
            {
               Debug.LogWarning("Unit or UnitHealth is null!");
               continue;
            }

            UnitDataSave data = new UnitDataSave
            {
               unitType = unit.UnitType,
               unitLevel = unit.UnitLevel,
               position = unit.transform.position,
               rotation = unit.transform.rotation,
            };
            playerUnitsData.Add(data);

            Debug.Log($"Saved unit: {unit.UnitType}, Level: {unit.UnitLevel}, Position: {unit.transform.position}");
         }
      }
   }
}
