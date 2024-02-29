using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public Vector3 TargetPos { get; set; }
    public Vector3 StartPos { get; set; }

    public abstract void UseSkill();
    public void SetStartPos(Vector3 StartPos) { this.StartPos = StartPos; }
    public void SetTargetPos(Vector3 TargetPos) { this.TargetPos = TargetPos; }
}
