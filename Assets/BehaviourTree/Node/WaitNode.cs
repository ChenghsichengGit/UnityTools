using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1;
    float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Debug.Log(Time.time - startTime);
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
