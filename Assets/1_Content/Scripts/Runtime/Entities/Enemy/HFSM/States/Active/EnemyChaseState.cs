using BH.Runtime.Entities;
using BH.Runtime.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyActiveState
{
    public EnemyChaseState(EnemyController enemy, StateMachine<EnemyState> stateMachine) : base(enemy, stateMachine)
    {
    }

    private Vector2 GetAttackTargetDirection()
    {
        Vector2 attackTaget = (Vector2)_enemy.AttackTarget.transform.position;
        Vector2 attackTargetDirection = (attackTaget - (Vector2)_enemy.transform.position).normalized;
        return attackTargetDirection;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector2 direction = GetAttackTargetDirection();
        _enemy.Movement.Move(direction);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
