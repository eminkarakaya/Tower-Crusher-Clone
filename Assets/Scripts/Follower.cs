using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private bool _isReady;
    public bool isReady
    {
        get => _isReady;
        set
        {
            _isReady = value;
            if(_isReady == true)
                turret.enabled = true;
        }
    }
    [SerializeField] private int currIndex = 0;
    [SerializeField] private Vector3 dir;
    [SerializeField] private float speed;
    Gun turret;
    public int CurrentIndex {
        get => currIndex;
        set { currIndex = value;}
    }
    private void Start()
    {
        turret = GetComponent<Gun>();
        transform.position = GameManager.Instance.paths[currIndex].position;
    }

    private void Update()
    {
        if (!GameManager.Instance.GetAvailableIndex(this,CurrentIndex) && !isReady)
        {
            return;
        }
        FollowPath();
    }

    private void FollowPath()
    {
        if (!turret.enabled)
            turret.enabled = true;
        isReady = true;
        if (Vector3.Distance(transform.position, GameManager.Instance.paths[currIndex + 1].position) < .2f)
        {
            if (currIndex >= GameManager.Instance.paths.Count - 2)
            {
                currIndex = 0;
                transform.position = GameManager.Instance.paths[0].position;
                return;
            }
            currIndex++;
        }
        transform.LookAt(GameManager.Instance.paths[currIndex + 1]);
        transform.Translate(Dir(currIndex, GameManager.Instance.paths) * speed * Time.deltaTime, Space.World);
    }
    
    Vector3 Dir(int currIndex, List<Transform> path)
    {
        //if (path.Count == currIndex)
        //{
        //    EndOfRoad();
        //}
        var next = path[currIndex + 1].position;
        dir = (path[currIndex + 1].position - transform.position).normalized;
        return (path[currIndex + 1].position - transform.position).normalized;
    }
}
