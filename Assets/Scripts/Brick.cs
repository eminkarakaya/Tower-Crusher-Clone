using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Floor floor;
    private void Start()
    {
        floor = GetComponentInParent<Floor>();
    }
    public void Destroy()
    {
        gameObject.SetActive(false);
        floor.brickCount--;
        floor.CheckFloor(floor);
        //FloorManager.Instance.Floors.Remove(floor);
        
    }
}
