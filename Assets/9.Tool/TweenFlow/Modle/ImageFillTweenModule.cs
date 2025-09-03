using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool.TweenFlow
{
    [ModuleMenu("Image / Fill")]
    public class ImageFillTweenModule : ImageBaseTweenModule
    {
        [HorizontalGroup("Remap")]
        public float RemapZero, RemapOne;
        
        public override List<Tween> BuildTweensCore()
        {
            float t = RemapZero;
            Tweens.Add(DOTween.To(() => t, x => t = x, RemapOne, Duration)
                .OnUpdate(() => Image.fillAmount = t));

            return Tweens;
        }
    }
}