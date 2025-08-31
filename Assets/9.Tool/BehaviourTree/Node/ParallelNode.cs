using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : CompositeNode
{

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].Update();
        }

        return State.Running;
    }
}
