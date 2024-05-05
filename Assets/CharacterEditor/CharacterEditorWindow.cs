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

    bool colliderFoldout = false;

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
        GameObject model = EditorGUILayout.ObjectField("Model: ", character.model, typeof(GameObject), true) as GameObject;
        Vector3 modelPos = EditorGUILayout.Vector3Field("ModelPos", character.modelPos);

        colliderFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(colliderFoldout, "Collider", null, FoldoutHeaderGroupMenuList);
        if (colliderFoldout)
        {
            ColliderType colliderType = (ColliderType)EditorGUILayout.EnumPopup("colliderType", character.colliderType);
            Vector3 colliderCenter = EditorGUILayout.Vector3Field("ColliderCenter", character.colliderCenter);

            switch (colliderType)
            {
                case ColliderType.Box:
                    Vector3 boxSize = EditorGUILayout.Vector3Field("Size", character.boxSize);
                    character.SetProperties(model, colliderType, colliderCenter, boxSize, modelPos);
                    break;
                case ColliderType.Capsule:
                    float radius = EditorGUILayout.FloatField("Radius:", character.capsuleRadius);
                    float height = EditorGUILayout.FloatField("Height:", character.capsuleHeight);
                    character.SetProperties(model, colliderType, colliderCenter, radius, height, modelPos);
                    break;
                case ColliderType.Sphere:
                    float sphereRadius = EditorGUILayout.FloatField("Radius", character.sphereRadius);
                    character.SetProperties(model, colliderType, colliderCenter, sphereRadius, modelPos);
                    break;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

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

    private void FoldoutHeaderGroupMenuList(Rect rect)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Test"), false, Test);
        menu.DropDown(rect);
    }

    private void Test()
    {
        Debug.Log("Test");
    }
}
