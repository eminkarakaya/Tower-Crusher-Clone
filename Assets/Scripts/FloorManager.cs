using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class FloorManager : Singleton<FloorManager>
{
    [SerializeField] private List<Floor> floors;
    
    public Ease ease;
    public List<Floor> Floors => floors;
    float mouseX;
    private void Start()
    {
        floors = GetComponentsInChildren<Floor>().Select(x => x).ToList();
    }
    private void Update()
    {
        RotateScene();
    }
    //void GenerateTower()
    //{
    //    GameObject lastFloor = ObjectPool.Instance.GetPooledObject(1);
    //    lastFloor.SetActive(true);
    //    lastFloor.transform.position = Vector3.zero;
    //    for (int i = 0; i < towerHeight; i++)
    //    {
    //        var obj  = ObjectPool.Instance.GetPooledObject(1);
    //        obj.SetActive(true);
    //        obj.transform.position = lastFloor.transform.position + new Vector3(0, yMultipler, 0);
    //        lastFloor = obj;
    //    }
    //}
    public void RotateScene()
    {
        //if(Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if(touch.phase == TouchPhase.Moved)
        //    {
        //        mouseX = touch.deltaPosition.x;
        //    }
        //    Debug.Log("mousex");
        //    floorParent.transform.Rotate(0, -mouseX * 7, 0);
        //}
        if (Input.GetMouseButton(0))
        {
                mouseX = Input.GetAxis("Mouse X");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseX = 0;
        }
        transform.Rotate(0, -mouseX * 7, 0);
    }
    public List<Floor> GetUpperFloors(Floor currFloor)
    {
        List<Floor> empty = new List<Floor>();
        for (int i = floors.IndexOf(currFloor); i < floors.Count; i++)
        {       
            empty.Add(floors[i]);
        }
        return empty;
    }
}
