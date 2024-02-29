using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[LabelText("@$value.skillName")]
public class Skills
{
    [SerializeField] private string skillName;
    [field: SerializeField] public Skill skill { get; private set; }
}
