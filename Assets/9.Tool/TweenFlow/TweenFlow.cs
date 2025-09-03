using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tool.TweenFlow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ModuleMenuAttribute : Attribute
    {
        public string Path { get; }
        public ModuleMenuAttribute(string path) => Path = path;
    }

    public class TweenFlow : MonoBehaviour
    {
        [Title("Handle Settings")] [SerializeField]
        private bool playOnAwake = false;
        [SerializeField] private bool ignoreTimeScale = false;

        [SerializeField] private bool instant;

        private List<Sequence> _sequences = new List<Sequence>();
        private List<float> _delayTimes = new();
        
        private bool _isPlaying = false;

        [Title("Modules")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, ListElementLabelName = "DisplayName")]
        [SerializeReference]
        private List<ITweenModule> modules = new List<ITweenModule>();

        #region Editor選單
#if UNITY_EDITOR
        [Button("Add", ButtonSizes.Medium)]
        [HorizontalGroup("Row1")]
        private void ShowMenu()
        {
            var menu = new GenericMenu();
            foreach (var (path, type) in GetModuleMenuEntries())
                menu.AddItem(new GUIContent(path), false, OnPickModuleType, type);

            var rect = GUILayoutUtility.GetLastRect();
            var mouse = Event.current.mousePosition;
            menu.DropDown(new Rect(rect.x, mouse.y, 0, 0));
        }

        private void OnPickModuleType(object tObj)
        {
            var t = (Type)tObj;
            var inst = (ITweenModule)Activator.CreateInstance(t);
            modules.Add(inst);
        }

        private IEnumerable<(string path, Type type)> GetModuleMenuEntries()
        {
            foreach (var t in UnityEditor.TypeCache.GetTypesDerivedFrom<ITweenModule>())
            {
                if (t.IsAbstract) continue;
                if (t.GetConstructor(Type.EmptyTypes) == null) continue;

                var attr = (ModuleMenuAttribute)Attribute.GetCustomAttribute(t, typeof(ModuleMenuAttribute));
                if (attr == null) continue; // 沒標就不顯示

                yield return (attr.Path, t);
            }
        }

        [Button("Clear", ButtonSizes.Medium)]
        [HorizontalGroup("Row1", Width = 60)]
        public void ClearDynamic()
        {
            modules.Clear();
        }
#endif
        #endregion

        public event Action OnComplete;
        public void CompleteCallback(Action callback) => OnComplete = callback;

        private void Awake()
        {
            if (playOnAwake) Play();
        }

        private void Init()
        {
            _sequences = new();
            _delayTimes = new();

            int index = 0;
            float delayTime = 0;

            _sequences.Add(DOTween.Sequence());
            _sequences[index].SetUpdate(ignoreTimeScale)
                .Pause()
                .SetAutoKill(false);

            foreach (var module in modules)
            {
                if (module is DelayTweenModule delayModule)
                {
                    index++;
                    delayTime += delayModule.DelayTime;
                    _delayTimes.Add(delayTime);
                    
                    _sequences.Add(DOTween.Sequence().SetUpdate(ignoreTimeScale)
                        .Pause()
                        .SetAutoKill(false));
                    continue;
                }
                else
                {
                    foreach (var tween in module.BuildTweens())
                    {
                        _sequences[index].Join(tween);
                    }
                }
            }
        }

        [Button(ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
        public void Play()
        {
            Init();

            if (instant)
            {
                
                foreach (var t in _sequences)
                {
                    t.Complete();
                }
            }

            _isPlaying = true;
            
            _sequences[0].Play();

            for (int i = 1; i < _sequences.Count; i++)
            {
                _sequences[i].SetDelay(_delayTimes[i - 1]).Play().SetUpdate(ignoreTimeScale);
            }
            
            _sequences.Last().OnComplete(() =>
            {
                OnComplete?.Invoke();
                _isPlaying = false;
            });
        }

        [Button, HorizontalGroup("Buttons", 0.33f)]
        public void Pause()
        {
            if (!_isPlaying) return;

            foreach (var t in _sequences)
            {
                t.Pause();
            }
        }

        [Button, HorizontalGroup("Buttons", 0.33f)]
        public void Restart()
        {
            if (!_isPlaying) return;

            foreach (var t in _sequences)
            {
                t.Play();
            }
        }

        [Button, HorizontalGroup("Buttons", 0.33f)]
        public void Complete()
        {
            foreach (var t in _sequences)
            {
                t.Complete();
            }
        }
    }
}