using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    BehaviourTree tree;

    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        var log = ScriptableObject.CreateInstance<DebugLogNode>();
        log.message = "AAAAA";

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = log;

        tree.rootNode = loop;
    }

    void Update()
    {
        tree.Update();
    }
}
