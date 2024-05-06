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
    bool characterControllerFoldout = false;

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
        character.SetModel(model, modelPos);

        colliderFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(colliderFoldout, "Collider");
        if (colliderFoldout)
        {
            ColliderType colliderType = (ColliderType)EditorGUILayout.EnumPopup("colliderType", character.colliderType);
            Vector3 colliderCenter = EditorGUILayout.Vector3Field("ColliderCenter", character.colliderCenter);

            switch (colliderType)
            {
                case ColliderType.Box:
                    Vector3 boxSize = EditorGUILayout.Vector3Field("Size", character.boxSize);
                    character.SetCollider(colliderType, colliderCenter, boxSize);
                    break;
                case ColliderType.Capsule:
                    float radius = EditorGUILayout.FloatField("Radius:", character.capsuleRadius);
                    float height = EditorGUILayout.FloatField("Height:", character.capsuleHeight);
                    character.SetCollider(colliderType, colliderCenter, radius, height);
                    break;
                case ColliderType.Sphere:
                    float sphereRadius = EditorGUILayout.FloatField("Radius", character.sphereRadius);
                    character.SetCollider(colliderType, colliderCenter, sphereRadius);
                    break;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        characterControllerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(characterControllerFoldout, "CharacterController");
        if (characterControllerFoldout)
        {
            Vector3 controllerCenter = EditorGUILayout.Vector3Field("Center", character.controllerCenter);
            float radius = EditorGUILayout.FloatField("Radius:", character.controllerRadius);
            float height = EditorGUILayout.FloatField("Height:", character.controllerHeight);
            character.SetController(controllerCenter, radius, height);
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
}
