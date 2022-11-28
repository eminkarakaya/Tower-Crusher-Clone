using UnityEngine;
using System.Collections.Generic;
namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] bool isEndPath;
        public PathCreator currPath;
        public int currentPathIndex = 0;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        void Start() {
            //currPath = GameManager.Instance.paths[currentPathIndex];
            if (currPath != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                currPath.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (currPath != null)
            {
                NextPath();
                distanceTravelled += speed * Time.deltaTime;
                transform.position = currPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = currPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = currPath.path.GetClosestDistanceAlongPath(transform.position);
        }
        void NextPath()
        {
                //Debug.Log(currPath.path.GetPercentOnPath + " " + currPath + " "+ currentPathIndex);
            if(currPath.path.GetPercentOnPath >= 1)
            {
                isEndPath = true;
            }
            else if(currPath.path.GetPercentOnPath < 1 && isEndPath)
            {
                isEndPath = false;
                currentPathIndex++;
                if(GameManager.Instance.paths.Count <= currentPathIndex)
                {
                    currentPathIndex = 0;
                }
                //currPath = GameManager.Instance.paths[currentPathIndex];
            }
        }
    }
}