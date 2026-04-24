using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tool.TweenFlow
{
    [ModuleMenu("Image / Color")]
    public class ImageColorTweenModule : ImageBaseTweenModule
    {
        public Gradient ColorGradient;
        
        public override List<Tween> BuildTweensCore()
        {
            float t = 0;
            Tweens.Add(DOTween.To(() => t, x => t = x, 1, Duration)
                .OnUpdate(() => Image.color = ColorGradient.Evaluate(t)));
            
            return Tweens;
        }
    }
}