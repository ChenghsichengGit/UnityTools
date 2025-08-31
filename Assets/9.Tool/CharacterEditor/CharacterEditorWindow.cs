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
        OnSelectionChanged();
    }
    CharacterEditer selectionObj;

    bool colliderFoldout = false;
    bool characterControllerFoldout = false;
    bool useControllerToggle = false;

    private void OnGUI()
    {
        selectionObj = EditorGUILayout.ObjectField("", selectionObj, typeof(CharacterEditer), true) as CharacterEditer;

        if (!selectionObj)
        {
            EditorGUILayout.LabelField("No object selected.", EditorStyles.boldLabel);
            return;
        }

        if (!selectionObj.gameObject.scene.IsValid())
        {
            EditorGUILayout.LabelField(selectionObj.name + " is in the Project.", EditorStyles.boldLabel);
            return;
        }

        CharacterEditer character;
        character = selectionObj.GetComponent<CharacterEditer>();
        GameObject model = EditorGUILayout.ObjectField("Model: ", character.model, typeof(GameObject), true) as GameObject;
        Vector3 modelPos = EditorGUILayout.Vector3Field("ModelPos", character.modelPos);
        character.SetModel(model, modelPos);

        if (!model) return;

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

        useControllerToggle = EditorGUILayout.BeginToggleGroup("UseCharacterController", useControllerToggle);
        characterControllerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(characterControllerFoldout, "CharacterController");
        if (characterControllerFoldout)
        {
            Vector3 controllerCenter = EditorGUILayout.Vector3Field("Center", character.controllerCenter);
            float radius = EditorGUILayout.FloatField("Radius:", character.controllerRadius);
            float height = EditorGUILayout.FloatField("Height:", character.controllerHeight);
            character.SetController(useControllerToggle, controllerCenter, radius, height);
            Debug.Log("AA");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.EndToggleGroup();

        if (GUILayout.Button("SetObject"))
        {
            character.SetObject();
        }

    }

    private void OnSelectionChanged()
    {
        Selection.selectionChanged -= OnSelectionChanged;

        if (Selection.activeGameObject?.GetComponent<CharacterEditer>())
        {
            selectionObj = Selection.activeGameObject.GetComponent<CharacterEditer>();
        }

        Repaint();
        Selection.selectionChanged += OnSelectionChanged;
    }
}
