using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Animations_Circle;
using PathCreation.Examples;
using PathCreation;
using System.Linq;
using DG.Tweening;
public class GameManager : Singleton<GameManager> , IDataPersistence
{
    bool isMerging;
    Sequence seq;
    public static System.Action OnMerge;
    [SerializeField] private List<Transform> animationTransforms;
    public List<Transform> paths = new List<Transform>(); // kullanýlan
    [SerializeField] private List<Road> roads   ;
    [SerializeField] private int _gold;
    private int mergeCount = 3;
    [Header("Golds")]
    [SerializeField] private Gold _turretGold;
    [SerializeField] private Gold _routeGold, _mergeGold, _incomeGold;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> _turrets = new List<GameObject>();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _goldText;
    [HideInInspector] private TextParse[] allTextParse;
    private int routeCount = 1;
    private List<List<Gun>> gunLevels = new List<List<Gun>>() {new List<Gun>()};  //[leveller,turretler]
    public int Gold => _gold;

    [SerializeField] List<Transform> transforms;
    [SerializeField] Transform parent;

    private void Awake()
    {
        //for (int j = 0; j < routeCount; j++)
        //{
        //    for (int i = 0; i < roads[routeCount-1].holder.childCount; i++)
        //    {
        //        paths.Add(roads[routeCount-1].holder.GetChild(i));
        //    }
        //}
        //allTextParse = FindObjectsOfType<TextParse>();
    }
    public void LoadData(GameData data)
    {
        routeCount = data.routeCount;
        for (int j = 0; j < routeCount; j++)
        {
            for (int i = 0; i < roads[routeCount - 1].holder.childCount; i++)
            {
                paths.Add(roads[routeCount - 1].holder.GetChild(i));
            }
        }
        allTextParse = FindObjectsOfType<TextParse>();
        for (int i = 0; i < data.turretLevels.Count; i++)
        {
            LoadTurrets(data.turretLevels[i]);
        }
        //_gold = data.gold;
    }
    public void SaveData(GameData data)
    {
        
        data.gold = Gold;
        data.routeCount = routeCount;
        data.turretLevels.Clear();
        for (int i = 0; i < gunLevels.Count; i++)
        {
            for (int j = 0; j < gunLevels[i].Count; j++)
            {
                data.turretLevels.Add(i); 
            }
        }
    }
    public void AddTurret()
    {
        if (_turretGold.GetGold() > Gold )
            return ;
        
        var obj = Instantiate(_turrets[0], paths[0].position, Quaternion.identity);
        obj.GetComponent<Gun>().enabled = false;
        SetGold(-_turretGold.GetGold());
        gunLevels[0].Add(obj.GetComponent<Gun>());
    }
    void LoadTurrets(int level)
    {
        if (gunLevels.Count <= level )
        {
            gunLevels.Add(new List<Gun>());
        }
        var obj = Instantiate(_turrets[level], paths[0].position, Quaternion.identity);
        obj.GetComponent<Follower>().enabled = false;
        StartCoroutine(EnableFollower(obj.GetComponent<Follower>()));   
        obj.GetComponent<Gun>().enabled = false;
        gunLevels[level].Add(obj.GetComponent<Gun>());
    }
    IEnumerator EnableFollower(Follower follower)
    {
        yield return new WaitForSeconds(1f);
        follower.enabled = true;
    }
    public void AddRoute()
    {
        if (_routeGold.GetGold() > Gold)
            return;
        if (roads.Count == routeCount)
            return;
        roads[routeCount].roadMesh.SetActive(true);
        routeCount++;
        for (int i = 0; i < roads[routeCount-1].holder.childCount; i++)
        {
            paths.Add(roads[routeCount - 1].holder.GetChild(i).transform);
        }
    }
    public bool GetAvailableIndex(Follower follower , int index)
    {
        if (gunLevels.Count == 0)
            return true;
        for (int i = 0; i < gunLevels.Count; i++)
        {
            if (gunLevels[i].Count == 0)
                return true;
            for (int j = 0; j < gunLevels[i].Count; j++)
            {
                for (int k = index-2; k < index+3; k++)
                {
                    if (gunLevels[i][j].GetComponent<Follower>() == follower)
                        continue;
                    if (gunLevels[i][j].GetComponent<Follower>().CurrentIndex == (paths.Count + k) % paths.Count && gunLevels[i][j].GetComponent<Follower>().isReady)
                        return false;
                }
            }
        }
        return true;
    }

    public void Merge()
    {
        if (isMerging)
            return;

        isMerging = true;
        for (int i = 0; i < gunLevels.Count; i++)
        {
            
            if (gunLevels[i].Count >= mergeCount)
            {
                var obj = Instantiate(gunLevels[i][0].nextGun, parent.position, Quaternion.identity);
                if(gunLevels.Count >= i + 1)
                {
                    gunLevels.Add(new List<Gun>()); 
                }
                gunLevels[i + 1].Add(obj.GetComponent<Gun>());
                obj.gameObject.SetActive(false);
                obj.GetComponent<Gun>().enabled = false;
                obj.GetComponent<Follower>().enabled = false;
                for (int k = 0; k < mergeCount; k++)
                {
                    Transform child = gunLevels[i][0].transform.GetChild(0);
                    child.SetParent(null);
                    DestroyAndRemove(gunLevels[i],gunLevels[i][0]);
                    child.transform.DOLookAt(Camera.main.transform.position, 2f);
                    child.transform.DOMove(transforms[k].position, 2f).OnComplete(() =>
                          child.transform.DOMove(parent.position, 1f).OnComplete(() => child.gameObject.SetActive(false)));//.); 
                    obj.transform.DOMove(parent.position,.00001f).SetDelay(3).OnComplete(()=>obj.gameObject.SetActive(true));
                }
                obj.transform.DOMove(paths[5].transform.position, 2f).SetDelay(3).OnComplete(() =>
                {
                    obj.GetComponent<Gun>().enabled = true;
                    obj.GetComponent<Follower>().enabled = true;
                    obj.GetComponent<Follower>().CurrentIndex = 5;
                    isMerging = false;
                });
                
                break;
            }
            
        }
    }
    public void DestroyAndRemove(List<Gun> list , Gun gun)
    {
        list.Remove(gun);
        Destroy(gun.gameObject);
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
    [System.Serializable] class Road
    {
        public Transform holder;
        public GameObject roadMesh;
    }
}
