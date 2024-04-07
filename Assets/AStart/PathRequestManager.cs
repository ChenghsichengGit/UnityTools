using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AStar
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequst;

        static PathRequestManager instance;
        Pathfinding pathfinding;

        bool isProcessingPath;

        void Awake()
        {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
        }

        public static void RequsetPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newPathRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequestQueue.Enqueue(newPathRequest);
            instance.TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequst = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequst.pathStart, currentPathRequst.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequst.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }
        }
    }
}
