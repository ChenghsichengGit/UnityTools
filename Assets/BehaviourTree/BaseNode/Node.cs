using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public string nodeName;

    [HideInInspector] public State state = State.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Variables variables;
    [HideInInspector] public Animator animator;
    [TextArea] public string description;

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.Failure || state == State.Success)
        {
            StopNode();
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();

    public void StopNode()
    {
        OnStop();
        started = false;
        OnStopNode();
    }

    protected virtual void OnStopNode() { }

    protected float GetNormalizedTime(Animator animator)
    {
        return GetNormalizedTime(animator, "");
    }

    protected float GetNormalizedTime(Animator animator, string animatorName)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsName(animatorName))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsName(animatorName))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }
}
