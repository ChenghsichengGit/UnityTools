using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家基本狀態
/// </summary>
public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    // 在new時取得stateMachine
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// 移動(沒有位移旦有重力)
    /// </summary>
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="motion">移動向量</param>
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move(motion * deltaTime);
    }

    // /// <summary>
    // /// 面對目標
    // /// </summary>
    // protected void FaceTarget()
    // {
    //     if (stateMachine.Targeter.CurrentTarget == null)
    //         return;

    //     Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
    //     lookPos.y = 0;

    //     stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    // }

    // /// <summary>
    // /// 判斷回到地State(FreeLook & Targeting)
    // /// </summary>
    // protected void ReturnToLocomotion()
    // {
    //     if (stateMachine.Targeter.CurrentTarget != null)
    //     {
    //         stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    //     }
    //     else
    //     {
    //         stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));

    //     }
    // }
}

