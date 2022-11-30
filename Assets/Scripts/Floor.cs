using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class Floor : MonoBehaviour
{
    float  maxX = 10,maxY = 10;
    [SerializeField] private List<Brick> bricks;
    public int brickCount;
    const float yMultipler = .5F;
    private void Start()
    {
        bricks = GetComponentsInChildren<Brick>().Select(x=>x).ToList();
        for (int i = 0; i < bricks.Count; i++)
        {
            float randomX = Random.Range(-maxX, maxX);
            float randomY = Random.Range(-maxY, maxY);
            Vector3 oldPos = bricks[i].transform.eulerAngles;
            bricks[i].transform.rotation = Quaternion.Euler(new Vector3(randomX, randomY, 0)+ oldPos);
            
        }
        brickCount = bricks.Count;
    }
    public List<Brick> GetBricks()
    {
        return bricks;
    }
    public void  CheckFloor(Floor deletedFloor)
    {
        if (brickCount == 0)
        {
            // towerin ustune yeni kat ekleme
            Floor floor = ObjectPool.Instance.GetPooledObject(1).GetComponent<Floor>();
            floor.gameObject.SetActive(true);
            floor.transform.SetParent(FloorManager.Instance.transform);
            for (int i = 0; i <floor.GetBricks().Count ; i++)
            {
                floor.GetBricks()[i].GetComponent<Collider>().enabled = true;
                floor.GetBricks()[i].gameObject.SetActive(true);
            }
            floor.transform.position = FloorManager.Instance.Floors[FloorManager.Instance.Floors.Count - 1] .transform.position;
            //this.transform.parent = FloorManager.Instance.transform;
            List < Floor > floors = FloorManager.Instance.GetUpperFloors(this);
            for (int i = 0; i < floors.Count; i++)
            {
                floors[i].transform.DOLocalMove(floors[i].transform.localPosition + new Vector3(0, -yMultipler, 0),.5f).SetEase(FloorManager.Instance.ease);
            }
            
            ObjectPool.Instance.SetPooledObject(this.gameObject, 1);
            FloorManager.Instance.Floors.Add(floor);
            FloorManager.Instance.Floors.Remove(deletedFloor);
            deletedFloor.brickCount = 37;
        }
    }
}
