using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : CompositeNode
{
    int current;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];

        switch (child.Update())
        {
            case State.Running:
                current++;
                break;
            case State.Failure:
                current++;
                break;
            case State.Success:
                current++;
                break;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}
