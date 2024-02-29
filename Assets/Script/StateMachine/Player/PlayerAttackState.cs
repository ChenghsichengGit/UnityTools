using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private readonly int AimForwardString = Animator.StringToHash("AimForward");
    private readonly int AimRightString = Animator.StringToHash("AimRight");
    private readonly int AimBlendTreeHash = Animator.StringToHash("AimBlendTree");

    private PlayerAttackAnimation attackAnimation;
    private const float CrossFadeDuration = 0.1f;

    private Skill attack;

    private bool isAttack;

    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attackAnimation = stateMachine.AttackAnimations[attackIndex];
        attack = stateMachine.AttackType[0].skill;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AimBlendTreeHash, CrossFadeDuration);

        stateMachine.Animator.CrossFadeInFixedTime(attackAnimation.animationsName, CrossFadeDuration, 1);

        stateMachine.Animator.SetLayerWeight(1, 1);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, 1);

        if (normalizedTime >= 0.6f)
        {
            stateMachine.SwitchState(new PlayerAimState(stateMachine));
            return;
        }

        if (normalizedTime >= attackAnimation.attackTime && !isAttack)
        {
            attack.SetStartPos(stateMachine.AttackPos.transform.position);
            attack.SetTargetPos(stateMachine.AimTarget.transform.position);
            attack.UseSkill();
            isAttack = true;
        }

        _Move(deltaTime);

        if (stateMachine.InputReader.isAttack)
        {
            TryComboAttack(normalizedTime);
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.SetLayerWeight(1, 0);
        stateMachine.Animator.CrossFadeInFixedTime("Idle", 0.05f, 1);
    }

    private void _Move(float deltaTime)
    {
        Vector3 movemnt = stateMachine.CalculateMovement();
        Move(movemnt * stateMachine.AimMoventSpeed, deltaTime);

        stateMachine.FaceForword();

        UpdataAnimator(deltaTime);
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

    /// <summary>
    /// 嘗試繼續攻擊
    /// </summary>
    private void TryComboAttack(float normalizedTime)
    {
        if (normalizedTime < attackAnimation.minComboAttackTime || normalizedTime > attackAnimation.maxComboAttackTime)
            return;

        stateMachine.SwitchState(new PlayerAttackState(stateMachine, attackAnimation.comboStateIndex));
    }
}
