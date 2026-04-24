using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tool.TweenFlow
{
    [ModuleMenu("Delay")]
    public class DelayTweenModule : ITweenModule
    {
        public List<Tween> Tweens { get; set; } =  new List<Tween>();
        
        [SerializeField] protected string m_DisplayName = "";
        public string DisplayName => "Delay";
        public float DelayTime = 0;

        public List<Tween> BuildTweens()
        {
            return Tweens;
        }
    }
}