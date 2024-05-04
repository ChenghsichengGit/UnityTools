using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

public enum ColliderType
{
    Box, Capsule, Sphere
}

public class CharacterEditorWindow : EditorWindow
{
    [MenuItem("EditorWindow/CharacterEditorWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CharacterEditorWindow));
    }

    private void CreateGUI()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }
    private void OnGUI()
    {
        if (!Selection.activeGameObject?.GetComponent<CharacterEditer>())
        {
            EditorGUILayout.ObjectField(null, typeof(GameObject), true);
            return;
        }

        CharacterEditer character;

        character = Selection.activeGameObject.GetComponent<CharacterEditer>();
        Selection.activeGameObject = EditorGUILayout.ObjectField(Selection.activeGameObject, typeof(CharacterEditer), true) as GameObject;
        GameObject model = EditorGUILayout.ObjectField("Image: ", character.model, typeof(GameObject), true) as GameObject;
        float radius = EditorGUILayout.FloatField("Radius:", character.radius);
        float height = EditorGUILayout.FloatField("Height:", character.height);
        Vector3 modelPos = EditorGUILayout.Vector3Field("ModelPos", character.modelPos);
        ColliderType colliderType = (ColliderType)EditorGUILayout.EnumPopup("", character.colliderType);

        character.SetProperties(model, radius, height, colliderType, modelPos);

        if (GUILayout.Button("SetObject"))
        {
            character.SetObject();
        }

    }

    private void OnSelectionChanged()
    {
        Selection.selectionChanged -= OnSelectionChanged;
        Repaint();
        Selection.selectionChanged += OnSelectionChanged;
    }
}
