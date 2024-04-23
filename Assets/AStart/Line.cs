using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AStar
{
    public struct Line
    {
        // 定義垂直線的斜率，這裡設置為一個很大的值
        const float verticalLineGradient = 1e5f;

        // 直線的斜率、y截距、線上的兩個點
        float gradient;
        float y_intercept;
        Vector2 pointOnLine_1;
        Vector2 pointOnLine_2;

        // 垂直線的斜率的倒數，用於計算垂直於該直線的斜率
        float gradientPerpendicular;

        // 表示點在直線的哪一側
        bool approachSide;

        // 直線的構造函數，根據給定的兩個點初始化直線
        public Line(Vector2 poiintOnLine, Vector2 pointPerpendicularToLine)
        {
            // 計算給定點之間的差異
            float dx = poiintOnLine.x - pointPerpendicularToLine.x;
            float dy = poiintOnLine.y - pointPerpendicularToLine.y;

            // 如果兩個點的x坐標相等，則直線為垂直線，斜率設置為verticalLineGradient
            if (dx == 0)
            {
                gradientPerpendicular = verticalLineGradient;
            }
            else
            {
                // 計算垂直於該直線的斜率
                gradientPerpendicular = dy / dx;
            }

            // 如果垂直於該直線的斜率為0，則直線為垂直線，斜率設置為verticalLineGradient
            if (gradientPerpendicular == 0)
            {
                gradient = verticalLineGradient;
            }
            else
            {
                // 計算該直線的斜率
                gradient = -1 / gradientPerpendicular;
            }

            // 計算直線的y截距
            y_intercept = poiintOnLine.y - gradient * poiintOnLine.x;
            // 初始化直線上的兩個點
            pointOnLine_1 = poiintOnLine;
            pointOnLine_2 = poiintOnLine + new Vector2(1, gradient);

            // 初始化點在直線的哪一側
            approachSide = false;
            approachSide = GetSide(pointPerpendicularToLine);
        }

        // 判斷點在直線的哪一側
        bool GetSide(Vector2 p)
        {
            return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
        }

        // 判斷點是否越過直線
        public bool HasCrossedLine(Vector2 p)
        {
            return GetSide(p) != approachSide;
        }

        // 計算點到直線的距離
        public float DistanceFromPoint(Vector2 p)
        {
            float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
            float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
            float intersectY = gradient * intersectX + y_intercept;
            return Vector2.Distance(p, new Vector2(intersectX, intersectY));
        }

        // 使用Gizmos繪製直線
        public void DrawWithGizmos(float length)
        {
            Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
            Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y);
            Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
        }
    }

}
