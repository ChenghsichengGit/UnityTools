using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

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

    private GameObject modelObj;

    private CharacterController controller;

    public void SetProperties(GameObject _model, ColliderType _colliderType, Vector3 _colliderCenter, Vector3 _boxSize, Vector3 _modelPos)
    {
        model = _model;
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        boxSize = _boxSize;
        modelPos = _modelPos;
    }

    public void SetProperties(GameObject _model, ColliderType _colliderType, Vector3 _colliderCenter, float _capsuleRadius, float _capsuleHeight, Vector3 _modelPos)
    {
        model = _model;
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        capsuleRadius = _capsuleRadius;
        capsuleHeight = _capsuleHeight;
        modelPos = _modelPos;
    }

    public void SetProperties(GameObject _model, ColliderType _colliderType, Vector3 _colliderCenter, float _sphereRadius, Vector3 _modelPos)
    {
        model = _model;
        colliderType = _colliderType;
        colliderCenter = _colliderCenter;
        sphereRadius = _sphereRadius;
        modelPos = _modelPos;
    }

    public void SetObject()
    {
        // 設定模型
        SetModel();

        // 設定碰撞框
        SetCollider();

        Debug.Log("角色設定完成");
    }

    private void SetModel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log(i + transform.GetChild(i).name);
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        modelObj = Instantiate(model, transform);
        modelObj.transform.position = modelPos;
    }

    private void SetCollider()
    {
        // 刪除所有碰撞框
        Component[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        controller = gameObject.AddComponent<CharacterController>();

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
    }
}
