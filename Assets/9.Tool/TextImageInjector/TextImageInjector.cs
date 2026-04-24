using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TextImageInjector : MonoBehaviour
{
    [Header("目標 Text 元件")]
    public Text targetText;

    [Header("關鍵字配對設定")]
    [ReadOnly]
    [SerializeField] KeywordMarkerSetting keywordMarkerSetting;

    [Header("Marker 設定")]
    public Vector2 markerDimensions = new Vector2(30, 30);
    public Vector2 markerOffset = new Vector2(0, 0);
    public string replacementChar = "＋";

    [SerializeField, ReadOnly] private string originalText = "";

    private readonly List<GameObject> _activeMarkers = new();

    Canvas _canvas;

    private void OnEnable()
    {
        _canvas = targetText.GetComponentInParent<Canvas>();
        InjectMarkers();
    }

    [Button]
    public void UpdateText()
    {
        originalText = targetText.text;
    }
    
    [Button]
    public void UpdateText(string text)
    {
        originalText = text;
    }

    [Button("Inject Markers")]
    public void InjectMarkers()
    {
        keywordMarkerSetting = Resources.Load<KeywordMarkerSetting>("KeywordMarkerSetting");

        if (targetText == null || keywordMarkerSetting == null || keywordMarkerSetting.keywordMarkers == null) return;

        Canvas canvas = targetText.GetComponentInParent<Canvas>();
        if (canvas == null) return;

        ClearMarkers();

        if (string.IsNullOrEmpty(originalText))
            UpdateText();
        
        originalText = originalText.Replace(" ", "\u00A0");
        string finalReplacedText = originalText;
        string whitespaceStrippedText = Regex.Replace(originalText, @"[ \t\r\n]+", "");

        List<(int index, KeywordMarkerSetting.KeywordMarkerPair pair)> matchedMarkers = new();

        foreach (var pair in keywordMarkerSetting.keywordMarkers)
        {
            if (pair == null || string.IsNullOrEmpty(pair.keyword) || pair.markerPrefab == null)
                continue;

            string keyword = pair.keyword;
            int searchIndex = 0;

            while ((searchIndex = whitespaceStrippedText.IndexOf(keyword, searchIndex, StringComparison.Ordinal)) != -1)
            {
                int length = whitespaceStrippedText.Length;

                finalReplacedText = ReplaceFirst(finalReplacedText, keyword, replacementChar);
                whitespaceStrippedText = ReplaceFirst(whitespaceStrippedText, keyword, replacementChar);

                length -= whitespaceStrippedText.Length;

                matchedMarkers.Add((searchIndex, pair));

                Debug.Log("InjectMarkers : " + searchIndex);

                for (int i = 0; i < matchedMarkers.Count; i++)
                {
                    if (matchedMarkers[i].index > searchIndex)
                    {
                        var m = matchedMarkers[i];
                        m.index -= length;
                        matchedMarkers[i] = m;
                    }
                }

                searchIndex += replacementChar.Length;
            }
        }

        targetText.text = finalReplacedText;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);

        StartCoroutine(CeateMarker(matchedMarkers));
    }

    [Button]
    void ClearMarkers()
    {
        foreach (var marker in _activeMarkers)
        {
            DestroyImmediate(marker.gameObject);
        }
        
        _activeMarkers.Clear();
    }

    string ReplaceFirst(string input, string search, string replace)
    {
        int idx = input.IndexOf(search, StringComparison.Ordinal);
        if (idx < 0) return input; // 找不到就原樣回傳

        return input.Substring(0, idx) + replace + input.Substring(idx + search.Length);
    }

    public IEnumerator CeateMarker(List<(int index, KeywordMarkerSetting.KeywordMarkerPair pair)> matchedMarkers)
    {
        yield return null;

        if(_canvas == null) _canvas = targetText.GetComponentInParent<Canvas>();
        
        foreach (var match in matchedMarkers)
        {
            Vector2 anchoredPos = GetCharAnchoredPosition(match.index, targetText, _canvas);
            RectTransform marker = Instantiate(match.pair.markerPrefab, _canvas.transform);
            marker.anchoredPosition = anchoredPos + markerOffset;
            marker.localScale = Vector3.one;
            marker.sizeDelta = markerDimensions;
            marker.SetParent(targetText.transform, true);
            _activeMarkers.Add(marker.gameObject);
        }
    }

    public static Vector2 GetCharAnchoredPosition(int charIndex, Text text, Canvas canvas)
    {
        if (text == null || string.IsNullOrEmpty(text.text) || charIndex < 0 || charIndex >= text.text.Length)
            return Vector2.zero;

        TextGenerator generator = text.cachedTextGenerator;
        generator.Invalidate();

        Vector2 textSize = text.rectTransform.rect.size;
        TextGenerationSettings settings = text.GetGenerationSettings(textSize);
        settings.scaleFactor = 1;

        if (!generator.Populate(text.text, settings))
        {
            Debug.LogWarning("Text generation failed.");
            return Vector2.zero;
        }

        int quadStart = charIndex * 4;
        if (quadStart + 3 >= generator.vertexCount)
            return Vector2.zero;

        var bl = generator.verts[quadStart].position;
        var tr = generator.verts[quadStart + 2].position;
        Vector2 localCenter = (bl + tr) * 0.5f;

        Camera cam = canvas != null ? canvas.worldCamera : null;
        Vector3 worldPos = text.transform.TransformPoint(localCenter);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(cam, worldPos),
            cam,
            out Vector2 anchored
        );

        return anchored;
    }
}
