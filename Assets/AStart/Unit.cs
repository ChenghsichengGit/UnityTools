using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

namespace AStar
{
    public class Unit : MonoBehaviour
    {
        public Transform target;
        public float speed;
        public float turnDst = 5;

        Path path;

        private void Start()
        {
            PathRequestManager.RequsetPath(transform.position, target.position, OnPathFound);
        }

        public void OnPathFound(Vector3[] wawypoints, bool pathSusccessful)
        {
            if (pathSusccessful)
            {
                path = new Path(wawypoints, transform.position, turnDst);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            while (true)
            {
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (path != null)
            {
                path.DrawWithGizmos();
            }
        }
    }
}
