using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterEditer : MonoBehaviour
{
    public GameObject model { get; private set; }
    public float radius { get; private set; }
    public float height { get; private set; }
    public Vector3 modelPos { get; private set; }

    public ColliderType colliderType;

    private GameObject modelObj;

    private CharacterController controller;
    private Collider myCollider;

    public void SetProperties(GameObject _model, float _radius, float _height, ColliderType _colliderType)
    {
        SetProperties(_model, _radius, _height, _colliderType, Vector3.zero);
    }

    public void SetProperties(GameObject _model, float _radius, float _height, ColliderType _colliderType, Vector3 _modelPos)
    {
        model = _model;
        radius = _radius;
        height = _height;
        colliderType = _colliderType;
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
            case ColliderType.Sphere:
                myCollider = gameObject.AddComponent<SphereCollider>();
                break;
            case ColliderType.Capsule:
                myCollider = gameObject.AddComponent<CapsuleCollider>();
                break;
            case ColliderType.Box:
                myCollider = gameObject.AddComponent<BoxCollider>();
                break;
        }

        // if (!controller || !myCollider) return;

        // if (height > 0)
        // {
        //     controller.height = height;
        //     myCollider.height = height;
        // }
        // if (radius > 0)
        // {
        //     controller.radius = radius;
        //     myCollider.radius = radius;
        // }

        // controller.center = new Vector3(0, controller.height / 2, 0);
        // myCollider.center = new Vector3(0, controller.height / 2, 0);
    }
}
