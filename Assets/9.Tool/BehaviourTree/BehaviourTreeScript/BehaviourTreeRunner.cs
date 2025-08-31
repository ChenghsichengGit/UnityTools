using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    public Animator animator;

    void Start()
    {
        tree = tree.Clone();
        tree.animator = this.animator;
        tree.Bind();

        OnStart();
    }

    void Update()
    {
        tree.Update();
        OnUpdate();
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
}
