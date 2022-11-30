using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public List<int> turretLevels;
    public int gold,routeCount,addTurretCost,mergeCost,addRouteCost;
    public GameData()
    {
        turretLevels = new List<int>();
        gold = 0;
        routeCount = 1;
        addTurretCost = 100;
        mergeCost = 200;
        addRouteCost = 5000;
    }
}