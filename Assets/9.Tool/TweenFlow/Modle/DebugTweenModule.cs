using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tool.TweenFlow
{
    [ModuleMenu("Debug / Log")]
    public class DebugTweenModule : ITweenModule
    {
        public List<Tween> tweens { get; set; } =   new List<Tween>();
        public string DisplayName => "Debug Log";
        public string Log;
        
        public List<Tween> BuildTweens()
        {
            float t = 0;
            tweens.Add(DOTween.To(() => t, x => t = x, 0, 0)
                .OnStart(() => UnityEngine.Debug.Log(Log)));
            return tweens;
        }
    }
}