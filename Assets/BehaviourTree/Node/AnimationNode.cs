using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNode : ActionNode
{
    public string animationName;
    public float exitTime = 1;

    protected override void OnStart()
    {
        animator.CrossFadeInFixedTime(animationName, 0.1f);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (GetNormalizedTime(animator, animationName) >= exitTime)
        {
            return State.Success;
        }

        return State.Running;
    }
}
