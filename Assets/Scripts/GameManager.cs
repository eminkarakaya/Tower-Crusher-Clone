using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using PathCreation.Examples;
using PathCreation;
using System.Linq;
public class GameManager : Singleton<GameManager>
{
    public List<Transform> paths; // kullanýlan
    [SerializeField] private List<Transform> allPaths;
    [SerializeField] private int _gold;
    [Header("Golds")]
    [SerializeField] private Gold _turretGold;
    [SerializeField] private Gold _routeGold, _mergeGold, _incomeGold;

    [Header("Prefabs")]
    [SerializeField] private GameObject _level1Turret;
    [SerializeField] private GameObject _level2Turret,_level3Turret;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _goldText;
    [HideInInspector] private TextParse[] allTextParse;
    private int routeCount = 1;
    public int Gold => _gold;

    private void Start()
    {
        allTextParse = FindObjectsOfType<TextParse>();
    }
    public void AddTurret()
    {
        if (_turretGold.GetGold() > Gold)
            return;
        var obj = Instantiate(_level1Turret, Vector3.zero, Quaternion.identity);
        PathFollower pathFollower = obj.GetComponent<PathFollower>();
        SetGold(-_turretGold.GetGold());
    }

    public void AddRoute()
    {
        if (_routeGold.GetGold() > Gold)
            return;
        allPaths[paths.Count].gameObject.SetActive(true);
        allPaths[paths.Count].GetComponent<RoadMeshCreator>().meshHolder.SetActive(true);
        routeCount++;
        for (int i = 0; i < allPaths[routeCount-1].childCount; i++)
        {
            paths.Add(allPaths[routeCount - 1].GetChild(i).transform);
        }
    }

    public void SetGold(int value)
    {
        _gold += value;
        _goldText.text = CaclText(_gold);
        
        for (int i = 0; i < allTextParse.Length; i++)
        {
            allTextParse[i].Check(allTextParse[i].GetComponent<Gold>().GetGold());
        }
    }





    public static string CaclText(float value)
    {
        if (value == 0)
        {
            return "$" + "0";
        }
        if (value < 1000)
        {
            return "$"+ String.Format("{0:0.0}", value);
        }
        else if (value >= 1000 && value < 1000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000) + "k";
        }
        else if (value >= 1000000 && value < 1000000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000000) + "m";
        }
        else if (value >= 1000000000 && value < 1000000000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000000000) + "b";
        }
        else if (value >= 1000000000000 && value < 1000000000000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000000000000) + "t";
        }
        else if (value >= 1000000000000000 && value < 1000000000000000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000000000000000) + "aa";
        }
        else if (value >= 1000000000000000000)
        {
            return "$" + String.Format("{0:0.0}", value / 1000000000000000) + "ab";
        }
        return value.ToString();
    }
}
