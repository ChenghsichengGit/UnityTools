using System;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField, FoldoutGroup("Components")] public InputReader InputReader { get; private set; }
    [field: SerializeField, FoldoutGroup("Components")] public CharacterController Controller { get; private set; }
    [field: SerializeField, FoldoutGroup("Components")] public Animator Animator { get; private set; }
    [field: SerializeField, FoldoutGroup("Components")] public Rig Rig { get; private set; }
    [field: SerializeField, FoldoutGroup("Components")] public CameraControls CameraControls { get; private set; }
    [field: SerializeField, FoldoutGroup("Components")] public PlayerAimTarget AimTarget { get; private set; }

    [field: SerializeField, FoldoutGroup("Value")] public float RotationDamping { get; private set; } // 旋轉速度
    [field: SerializeField, FoldoutGroup("Value")] public float FreeLookMoventSpeed { get; private set; } // 自由視角移動速度
    [field: SerializeField, FoldoutGroup("Value")] public float AimMoventSpeed { get; private set; } // 自由視角移動速度

    [field: SerializeField] public GameObject AttackPos { get; private set; }
    [field: SerializeField] public Skills[] AttackType { get; private set; }
    [field: SerializeField] public PlayerAttackAnimation[] AttackAnimations { get; private set; }

    public Transform MainCameraTransform { get; private set; } // 主攝影機

    private void Start()
    {
        // 隱藏滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;
        Rig.weight = 0;

        SwitchState(new PlayerFreeLookState(this));
    }

    /// <summary>
    /// 計算玩家移動向量
    /// </summary>
    public Vector3 CalculateMovement()
    {
        Vector3 forward = MainCameraTransform.forward;
        Vector3 right = MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 根據玩家輸入的移動值和相機的前方與右方向量計算最終的移動向量
        return forward * InputReader.MovementValue.y +
            right * InputReader.MovementValue.x;
    }

    /// <summary>
    /// 面向移動方向
    /// </summary>
    public void FaceMovementDirection(Vector3 movemnt, float deltaTime)
    {
        // 使用插值的方式將角色的旋轉逐漸調整為面向移動方向
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(movemnt),
            deltaTime * RotationDamping);
    }

    /// <summary>
    /// 面對目標
    /// </summary>
    public void FaceForword()
    {
        Vector3 lookPos = new Vector3(transform.rotation.x, MainCameraTransform.transform.localRotation.eulerAngles.y, transform.rotation.z);
        transform.rotation = Quaternion.Euler(lookPos);
    }
}

[Serializable]
[LabelText("@$value.animationsName")]
public class PlayerAttackAnimation
{
    [field: SerializeField] public string animationsName { get; private set; }
    [field: SerializeField] public float minComboAttackTime { get; private set; }
    [field: SerializeField] public float maxComboAttackTime { get; private set; }
    [field: SerializeField] public float attackTime { get; private set; }
    [field: SerializeField] public int comboStateIndex { get; private set; }

}

