using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tool.TweenFlow
{
    [ModuleMenu("Delay")]
    public class DelayTweenModule : ITweenModule
    {
        public List<Tween> tweens { get; set; } =   new List<Tween>();
        public string DisplayName => "Delay";
        public float DelayTime = 0;

        public List<Tween> BuildTweens()
        {
            return tweens;
        }
    }
}