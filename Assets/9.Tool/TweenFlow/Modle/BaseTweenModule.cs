using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool.TweenFlow
{
    public abstract class BaseTweenModule : ITweenModule
    {
        [SerializeField] protected string displayName = "";
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? GetType().Name : displayName;
        public List<Tween> tweens { get; set; } =  new List<Tween>();

        public virtual List<Tween> BuildTweens()
        {
            return tweens;
        }
    }
}