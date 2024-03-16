using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    public Animator animator;

    void Start()
    {
        tree = tree.Clone();
        tree.animator = this.animator;
        tree.Bind();
    }

    void Update()
    {
        tree.Update();
    }
}
