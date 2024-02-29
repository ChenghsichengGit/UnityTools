using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerBaseState
{
    private readonly int AimForwardString = Animator.StringToHash("AimForward");
    private readonly int AimRightString = Animator.StringToHash("AimRight");
    private readonly int AimBlendTreeHash = Animator.StringToHash("AimBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerAimState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AimBlendTreeHash, CrossFadeDuration);

        stateMachine.Rig.weight = 1;
        stateMachine.CameraControls.SwitchCamera("Aim");
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.InputReader.isAim)
        {
            SwitchAimState();
            return;
        }

        if (stateMachine.InputReader.isAttack)
        {
            SwitchAttackState();
            return;
        }

        Vector3 movemnt = stateMachine.CalculateMovement();

        Move(movemnt * stateMachine.AimMoventSpeed, deltaTime);
        stateMachine.FaceForword();

        UpdataAnimator(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Rig.weight = 0;
    }

    /// <summary>
    /// 更新動畫控制器的參數值
    /// </summary>
    private void UpdataAnimator(float deltaTime)
    {

        if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(AimForwardString, 0, 0.1f, deltaTime);

            if (stateMachine.Animator.GetFloat(AimForwardString) <= 0.1f && stateMachine.Animator.GetFloat(AimForwardString) >= -0.1f)
            {
                stateMachine.Animator.SetFloat(AimForwardString, 0);
            }
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(AimForwardString, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(AimRightString, 0, 0.1f, deltaTime);

            if (stateMachine.Animator.GetFloat(AimRightString) <= 0.1f && stateMachine.Animator.GetFloat(AimRightString) >= -0.1f)
            {
                stateMachine.Animator.SetFloat(AimRightString, 0);
            }
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(AimRightString, value, 0.1f, deltaTime);
        }
    }

    private void SwitchAimState()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void SwitchAttackState()
    {
        stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
    }
}
