using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tool.TweenFlow
{
    public interface ITweenModule
    {
        List<Tween> tweens { get; set; }

        string DisplayName { get; }
        List<Tween> BuildTweens();
    }
}