using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool.TweenFlow
{
    public abstract class BaseTweenModule : ITweenModule
    {
        [SerializeField] protected string m_DisplayName = "";
        public virtual string DisplayName => string.IsNullOrWhiteSpace(m_DisplayName) ? GetType().Name : m_DisplayName;
        public float Duration = 0f;
        public List<Tween> Tweens { get; set; } = new List<Tween>();

        public bool UseAnimationCurve;
        [HideIf("UseAnimationCurve")] public Ease Ease;

        [ShowIf("UseAnimationCurve")]
        public AnimationCurve EaseCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public virtual List<Tween> BuildTweensCore()
        {
            return Tweens;
        }
        
        public List<Tween> BuildTweens()
        {
            var tweens = BuildTweensCore();
                
            foreach (var tween in tweens)
            {
                if (UseAnimationCurve)
                {
                    tween.SetEase(EaseCurve);
                }
                else
                {
                    tween.SetEase(Ease);
                }
            }

            return tweens;
        }
    }
}