using UnityEditor;
using UnityEngine;

public class CharacterEditer : MonoBehaviour
{
    public GameObject model;
    public Vector3 modelPos;
    public ColliderType colliderType;

    public Vector3 colliderCenter;
    public Vector3 boxSize;
    public float capsuleRadius;
    public float capsuleHeight;
    public float sphereRadius;

    private bool useController;
    private CharacterController controller;
    public Vector3 controllerCenter;
    public float controllerRadius;
    public float controllerHeight;

    public void SetModel(GameObject _model, Vector3 _modelPos)
    {
        model = _model;
        modelPos = _modelPos;
    }

    public void SetCollider(ColliderType _colliderType, Vector3 _colliderCenter, Vector3 _boxSize)
    {
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        boxSize = _boxSize;
    }

    public void SetCollider(ColliderType _colliderType, Vector3 _colliderCenter, float _capsuleRadius, float _capsuleHeight)
    {
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        capsuleRadius = _capsuleRadius;
        capsuleHeight = _capsuleHeight;
    }

    public void SetCollider(ColliderType _colliderType, Vector3 _colliderCenter, float _sphereRadius)
    {
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        sphereRadius = _sphereRadius;
    }

    public void SetController(bool _useController)
    {
        useController = _useController;
    }

    public void SetController(bool _useController, Vector3 _controllerCenter, float _controllerRadius, float _controllerHeight)
    {
        useController = _useController;
        controllerCenter = _controllerCenter;
        controllerRadius = _controllerRadius;
        controllerHeight = _controllerHeight;
    }

    public void SetObject()
    {
        // 設定碰撞框
        SetCollider();

        // 設定模型
        SetModel();

        Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
        if (parentObject)
        {
            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
        }

        Debug.Log("角色設定完成");
    }

    private void SetModel()
    {
        if (model)
            model.transform.localPosition = modelPos;
    }

    private void SetCollider()
    {
        // 刪除所有碰撞框
        Component[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        switch (colliderType)
        {
            case ColliderType.Box:
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = boxSize;
                boxCollider.center = colliderCenter;
                break;
            case ColliderType.Capsule:
                CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                capsuleCollider.height = capsuleHeight;
                capsuleCollider.radius = capsuleRadius;
                capsuleCollider.center = colliderCenter;
                break;
            case ColliderType.Sphere:
                SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = sphereRadius;
                sphereCollider.center = colliderCenter;
                break;
        }

        if (useController)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.center = controllerCenter;
            controller.radius = controllerRadius;
            controller.height = controllerHeight;
        }
    }
}
