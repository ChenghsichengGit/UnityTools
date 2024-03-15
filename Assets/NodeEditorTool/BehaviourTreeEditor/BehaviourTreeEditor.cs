using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTree tree;
    BehaviourTreeView treeView;
    InspectorView inspectorView;
    IMGUIContainer variableView;
    ToolbarMenu assestMenu;
    ToolbarMenu addMenu;

    SerializedObject treeObject;
    SerializedProperty variableProperty;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/NodeEditorTool/BehaviourTreeEditor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/NodeEditorTool/BehaviourTreeEditor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        // 实例化 treeView
        treeView = new BehaviourTreeView();
        root.Add(treeView);

        // AssestMenu
        assestMenu = root.Q<ToolbarMenu>("AssestMenu");
        SearchAndAppendBehaviourTreeAssets();

        // AddMenu
        addMenu = root.Q<ToolbarMenu>("AddMenu");
        AddMenuAppendAction();

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();

        variableView = root.Q<IMGUIContainer>("variable");
        variableView.onGUIHandler = () =>
        {
            if (treeObject != null)
            {
                treeObject.Update();
                EditorGUILayout.PropertyField(variableProperty);
                treeObject.ApplyModifiedProperties();
            }
        };

        treeView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayerModeStateChanged;
        SearchAndAppendBehaviourTreeAssets();
        EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayerModeStateChanged;
    }

    private void OnPlayerModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;

        }
    }


    private void OnSelectionChange()
    {
        tree = Selection.activeObject as BehaviourTree;

        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                treeView?.PopulateView(tree);
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView?.PopulateView(tree);
            }
        }

        if (tree != null)
        {
            treeObject = new SerializedObject(tree);
            variableProperty = treeObject.FindProperty("variable");
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView.UpdateNodeStates();
        SearchAndAppendBehaviourTreeAssets();
    }

    private void AddMenuAppendAction()
    {
        // addMenu.menu.AppendAction("Int", (a) => { tree.variable.ints.Add(new Int()); });

        // addMenu.menu.AppendAction("Float", (a) => { tree.variable.floats.Add(new Float()); });

        // addMenu.menu.AppendAction("Bool", (a) => { tree.variable.bools.Add(new Bool()); });

        // addMenu.menu.AppendAction("String", (a) => { tree.variable.strings.Add(new String()); });

    }

    // 搜索所有的 BehaviourTree 资源并在 AssestMenu 中添加对应操作
    private void SearchAndAppendBehaviourTreeAssets()
    {
        string[] guids = AssetDatabase.FindAssets("t:BehaviourTree"); // 搜索所有的 BehaviourTree 资源
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid); // 获取资源路径
            BehaviourTree behaviourTree = AssetDatabase.LoadAssetAtPath<BehaviourTree>(path); // 加载资源对象
            if (behaviourTree != null)
            {
                assestMenu.menu.AppendAction(behaviourTree.name, (a) =>
                {
                    Selection.SetActiveObjectWithContext(behaviourTree, behaviourTree);
                });
            }
        }
    }
}