using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class BehaviourTreeUtility
{
    public static string BehaviourTreeEditorScriptFolderPath()
    {
        string[] scriptGuids = AssetDatabase.FindAssets("t:Script BehaviourTreeEditor");
        if (scriptGuids.Length > 0)
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuids[0]);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);

            if (script != null)
            {
                string scriptAssetPath = AssetDatabase.GetAssetPath(script);
                string scriptFolderPath = scriptAssetPath.Replace("BehaviourTreeEditor.cs", "");
                return scriptFolderPath;
            }
        }
        return "";
    }
}
