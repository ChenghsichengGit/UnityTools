using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedString = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        stateMachine.CameraControls.SwitchCamera("FreeLook");
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.isAim)
        {
            SwitchAimState();
            return;
        }

        Vector3 movemnt = stateMachine.CalculateMovement();

        Move(movemnt * stateMachine.FreeLookMoventSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedString, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.FaceMovementDirection(movemnt, deltaTime);
        stateMachine.Animator.SetFloat(FreeLookSpeedString, 1, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }

    private void SwitchAimState()
    {
        stateMachine.SwitchState(new PlayerAimState(stateMachine));
    }

}
