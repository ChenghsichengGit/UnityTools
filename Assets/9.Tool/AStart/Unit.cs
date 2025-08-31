using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

namespace AStar
{
    // 負責處理單位移動
    public class Unit : MonoBehaviour
    {
        // 最小更新路徑的時間間隔
        const float minPathUpdateTime = .2f;
        // 移動距離閾值，超過此閾值時觸發路徑更新
        const float pathUpdateMoveThreshold = .5f;
        public Transform target;
        public float speed;
        public float turnSpeed = 3;
        public float turnDst = 5;
        public float stoppingDst = 10;

        Path path;

        private void Start()
        {
            StartCoroutine(UpdatePath());
        }

        /// <summary>
        /// 當找到新路徑時的回調函數
        /// </summary>
        public void OnPathFound(Vector3[] wawypoints, bool pathSusccessful)
        {
            if (pathSusccessful)
            {
                path = new Path(wawypoints, transform.position, turnDst, stoppingDst);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        /// <summary>
        /// 路徑更新
        /// </summary>
        IEnumerator UpdatePath()
        {
            if (Time.timeSinceLevelLoad < .3f)
            {
                yield return new WaitForSeconds(.3f);
            }
            // 請求新的路徑
            PathRequestManager.RequsetPath(new PathRequest(transform.position, target.position, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = target.position;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);

                // 如果目標位置的移動距離超過閾值，則重新請求路徑
                if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequsetPath(new PathRequest(transform.position, target.position, OnPathFound));
                    targetPosOld = target.position;
                }
            }
        }

        /// <summary>
        /// 跟隨路徑並面向移動方向
        /// </summary>
        IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            transform.LookAt(path.lookPoints[0]);

            float speedPercent = 1;

            while (followingPath)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {
                    if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                    {
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                        if (speedPercent < 0.01f)
                        {
                            followingPath = false;
                        }
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                }

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
