using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public abstract class DecoratorNode : Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }

    protected override void OnStopNode()
    {
        child.StopNode();
    }
}
