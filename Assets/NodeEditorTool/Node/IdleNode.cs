using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : ActionNode
{
    protected override void OnStart()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Idle")
            animator.CrossFadeInFixedTime("Idle", 0.1f);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
