using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    [SerializeField] private ProjectileLauncher projectileLauncher;
    private ProjectileLauncher _projectileLauncher;

    public override void UseSkill()
    {
        _projectileLauncher = Instantiate(projectileLauncher, StartPos, Quaternion.LookRotation(TargetPos - StartPos));
        _projectileLauncher.SetTarget(TargetPos);
    }
}
