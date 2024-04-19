using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : ActionNode
{
    private AnimatorClipInfo[] clipInfo;

    protected override void OnStart()
    {
        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            string currentAnimationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            if (currentAnimationName != "Idle")
            {
                animator.CrossFadeInFixedTime("Idle", 0.1f);
            }
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
