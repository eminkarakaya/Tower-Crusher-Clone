using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class Floor : MonoBehaviour
{
    [SerializeField] private List<Brick> bricks;
    public int brickCount;
    const float yMultipler = 0.42548f;
    private void Start()
    {
        bricks = GetComponentsInChildren<Brick>().Select(x=>x).ToList();
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
                floors[i].transform.DOMove(floors[i].transform.position + new Vector3(0, -yMultipler, 0),.5f).SetEase(FloorManager.Instance.ease);
            }
            
            ObjectPool.Instance.SetPooledObject(this.gameObject, 1);
            FloorManager.Instance.Floors.Add(floor);
            FloorManager.Instance.Floors.Remove(deletedFloor);
            deletedFloor.brickCount = 37;
        }
    }
}
