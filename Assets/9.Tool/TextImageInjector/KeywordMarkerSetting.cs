using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "KeywordMarkerSetting", menuName = "TextTools/Keyword Marker Setting")]
public class KeywordMarkerSetting : ScriptableObject
{
    [System.Serializable]
    public class KeywordMarkerPair
    {
        public string keyword;
        public RectTransform markerPrefab;
    }

    [Header("關鍵字配對設定")]
    public List<KeywordMarkerPair> keywordMarkers = new();
}