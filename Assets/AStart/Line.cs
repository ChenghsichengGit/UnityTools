using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public struct Line
{
    const float verticalLineGradient = 1e5f;

    float gradient;
    float y_intercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    float gradientPerpendicular;

    bool approachSide;

    public Line(Vector2 poiintOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = poiintOnLine.x - pointPerpendicularToLine.x;
        float dy = poiintOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0)
        {
            gradientPerpendicular = verticalLineGradient;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }

        if (gradientPerpendicular == 0)
        {
            gradient = verticalLineGradient;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }

        y_intercept = poiintOnLine.y - gradient * poiintOnLine.x;
        pointOnLine_1 = poiintOnLine;
        pointOnLine_2 = poiintOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    public float DistanceFromPoint(Vector2 p)
    {
        float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
        float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
        float intersectY = gradient * intersectX + y_intercept;
        return Vector2.Distance(p, new Vector2(intersectX, intersectY));
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y);
        Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
    }
}
