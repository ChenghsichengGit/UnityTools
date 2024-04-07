using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

namespace AStar
{
    public class Unit : MonoBehaviour
    {
        public Transform target;
        [SerializeField] float speed;
        Vector3[] path;
        int targetIngdex;

        private void Start()
        {
            PathRequestManager.RequsetPath(transform.position, target.position, OnPathFound);
        }

        public void OnPathFound(Vector3[] newPath, bool pathSusccessful)
        {
            if (pathSusccessful)
            {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIngdex++;
                    if (targetIngdex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIngdex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIngdex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIngdex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}
