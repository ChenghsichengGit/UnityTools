using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BehaviourTreeRunner behaviourTreeRunner;

    public PlayerStateMachine player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        behaviourTreeRunner.tree.variables.SetFloat("PlayerDir", Vector3.Distance(this.transform.position, player.transform.position));
    }
}
