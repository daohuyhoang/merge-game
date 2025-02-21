using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVictory : MonoBehaviour
{
    private Unit unit;
    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void CheckForEnemyVictory()
    {
        if (ObjectPool.Instance.AreAllUnitsInactive())
        {
            unit.VictoryAnimation();
        }
    }
    
    public void CheckForPlayerVictory()
    {
        if (EnemyManager.Instance.AreAllEnemiesDead())
        {
            unit.VictoryAnimation();
            UIManager.Instance.ShowVictoryPanel();
        }
    }
}
