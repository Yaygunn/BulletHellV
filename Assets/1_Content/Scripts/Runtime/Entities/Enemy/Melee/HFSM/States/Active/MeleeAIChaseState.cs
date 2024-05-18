using BH.Runtime.Entities;
using BH.Runtime.StateMachines;
using UnityEngine;

public class MeleeAIChaseState : MeleeAIActiveState
{
    public MeleeAIChaseState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        _meleeAI.StateName = "Chase";
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector2 direction = GetAttackTargetDirection();
        _meleeAI.Movement.Move(direction, _meleeAI.Stats.CurrentSpeed);
        
        ShouldIdle();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
    
    private void ShouldIdle()
    {
        if (!_meleeAI.AttackTarget.gameObject.activeSelf)
        {
            _stateMachine.ChangeState(_meleeAI.IdleState);
        }
    }
    
    private Vector2 GetAttackTargetDirection()
    {
        Vector2 attackTaget = (Vector2)_meleeAI.AttackTarget.transform.position;
        Vector2 attackTargetDirection = (attackTaget - (Vector2)_meleeAI.transform.position).normalized;
        return attackTargetDirection;
    }
}
